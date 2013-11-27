#define HAVE_REMOTE

#include <stdio.h>
#include <string.h>
#include <pcap.h>
#include <Windows.h>
#include <Shlwapi.h>
#include "IPAccounting.h"
#include "NetPackets.h"
#include "AccountingDestination.h"
#include "LinkedListHosts.h"
#include "GeneralFunctions.h"


extern PHOSTNODE gHosts; 
extern int gXML;
extern int gFILTER;


/* 
 * Callback function invoked by libpcap for every incoming packet 
 *
 */
void DestinationAccountingCallback(unsigned char *pScanParams, struct pcap_pkthdr *pPcapHdr, unsigned char *pPktData)
{
  PSCANPARAMS lScanParams = (PSCANPARAMS) pScanParams;
  PETHDR lEthHdr = (PETHDR) pPktData;
  PIPHDR lIPHdr = NULL;
  PTCPHDR lTCPHdr = NULL;
  PUDPHDR lUDPHdr = NULL;
  int lIPHdrLen = 0;
  int lTotLen = 0;
  int lTCPHdrLen = 0;
  int lTCPDataLen = 0;
  int lUDPDataLen = 0;
  int lUDPHdrLen = 0;
  int lICMPDataLen = 0;
  unsigned short lType = htons(lEthHdr->ether_type);
  int lSrcPort = 0;
  int lDstPort = 0;

  unsigned char lSrcIP[128];
  unsigned char lDstIP[128];

  if (lType == ETHERTYPE_IP)
  {
    lIPHdr = (PIPHDR) (pPktData + 14);
    lIPHdrLen = (lIPHdr->ver_ihl & 0xf) * 4;
   	lTotLen = ntohs(lIPHdr->tlen);

    ZeroMemory(lSrcIP, sizeof(lSrcIP));
    ZeroMemory(lDstIP, sizeof(lDstIP));

    snprintf((char *) lSrcIP, sizeof(lSrcIP)-1, "%d.%d.%d.%d", lIPHdr->saddr.byte1, lIPHdr->saddr.byte2, lIPHdr->saddr.byte3, lIPHdr->saddr.byte4);
    snprintf((char *) lDstIP, sizeof(lDstIP)-1, "%d.%d.%d.%d", lIPHdr->daddr.byte1, lIPHdr->daddr.byte2, lIPHdr->daddr.byte3, lIPHdr->daddr.byte4);

   	/*
	    * TCP
	    */
   	if (lIPHdr->proto == IP_TCP)
   	{
      lTCPHdr = (PTCPHDR) ((u_char*) lIPHdr + lIPHdrLen);
      lTCPHdrLen = lTCPHdr->doff * 4;
      lTCPDataLen = lTotLen - lIPHdrLen - lTCPHdrLen;

      lSrcPort = ntohs(lTCPHdr->sport);
      lDstPort = ntohs(lTCPHdr->dport);
              
//printf("DestinationAccountingCallback  : (TCP)          %s:%d->%s:%d   %d\n", lSrcIP, lSrcPort, lDstIP, lDstPort, lTCPDataLen);
      ProcessDestinationIPTraffic(lScanParams, "TCP", lIPHdr->saddr, lIPHdr->daddr, lSrcPort, lDstPort, lTCPDataLen);


   	/*
   	 * UDP
   	 */
    }
    else if (lIPHdr->proto == IP_UDP)
    {
      lUDPHdr = (PUDPHDR) ((unsigned char*) lIPHdr + lIPHdrLen);
      lSrcPort = ntohs(lUDPHdr->sport);
      lDstPort = ntohs(lUDPHdr->dport);
      lUDPDataLen = ntohs(lUDPHdr->ulen) - lIPHdrLen - lUDPHdrLen;

//printf("DestinationAccountingCallback  : (UDP)          %s:%d->%s:%d   %d\n", lSrcIP, lSrcPort, lDstIP, lDstPort, lTCPDataLen);
      ProcessDestinationIPTraffic(lScanParams, "UDP", lIPHdr->saddr, lIPHdr->daddr, lSrcPort, lDstPort, lUDPDataLen);


   	/*
   	 * ICMP
   	 */
    }
    else if (lIPHdr->proto == IP_ICMP)
    {
      lSrcPort = ICMP_PSEUDO_PORT;
      lDstPort = ICMP_PSEUDO_PORT;
      lICMPDataLen = lTotLen - lIPHdrLen - lTCPHdrLen;
//printf("SourceAccountingCallback  : (UDP)          %s:%d->%s:%d   %d\n", lSrcIP, lSrcPort, lDstIP, lDstPort, lTCPDataLen);
      ProcessDestinationIPTraffic(lScanParams, "ICMP", lIPHdr->saddr, lIPHdr->daddr, lSrcPort, lDstPort, lICMPDataLen);


    } // if (pIPHdr->pro...
  } // if (lType == ET...
}






/*
 *
 *
 */
void DestinationStartAccounting(PSCANPARAMS pScanParams)
{
  int lRetVal = 0;
  pcap_if_t *lAllDevs = NULL;
  pcap_if_t *lDevice = NULL;
  char lTemp[PCAP_ERRBUF_SIZE];
  char lAdapter[MAX_BUF_SIZE + 1];
  int lCounter = 0;
  int lIFCnum = 0;

  char lFilter[1024];
  bpf_u_int32 lNetMask;
  struct bpf_program lFCode;
 
  PSCANPARAMS lTmpParams = (PSCANPARAMS) pScanParams;
  SCANPARAMS lScanParams;
  SECURITY_ATTRIBUTES lPipeSA = {sizeof (SECURITY_ATTRIBUTES), NULL, TRUE};

  ZeroMemory(&lScanParams, sizeof(lScanParams));
  CopyMemory(&lScanParams, lTmpParams, sizeof(lScanParams));

  /*
   * Open device list.
   */
  if (pcap_findalldevs_ex(PCAP_SRC_IF_STRING, NULL, &lAllDevs, lTemp) != -1)
  {
    ZeroMemory(lAdapter, sizeof(lAdapter));
 
    /*
     * Enum through all available interfaces and pick the
     * right one out.
     */
    for (lCounter = 0, lDevice = lAllDevs; lDevice; lDevice = lDevice->next, lCounter++)
    {
      if (StrStrI(lDevice->name, (char *) lScanParams.IFCName)) //pIFCName))
   	  {
        strcpy(lAdapter, lDevice->name);
        break;
	     } // if (StrS...
    } // for(lCounter = 0, ...

    if (lAllDevs)
      pcap_freealldevs(lAllDevs);


    /*
     * Open interface.
     */ 
    if ((lScanParams.IfcReadHandle = pcap_open(lAdapter, 65536, PCAP_OPENFLAG_PROMISCUOUS, PCAP_READTIMEOUT, NULL, lTemp)) != NULL)
    {
      /* 
       * Compiling + setting the filter
       */
      ZeroMemory(&lFCode, sizeof(lFCode));
      ZeroMemory(lFilter, sizeof(lFilter));

      if (gFILTER == 1)
      {
        _snprintf(lFilter, sizeof(lFilter) - 1, "not host %s", pScanParams->LocalIPStr);
        lNetMask = 0xffffff; // "255.255.255.0"
      } // if (gFILT...


      if (pcap_compile((pcap_t *) lScanParams.IfcReadHandle, &lFCode, (const char *) lFilter, 1, lNetMask) >= 0)
      {
        if (pcap_setfilter((pcap_t *) lScanParams.IfcReadHandle, &lFCode) >= 0)
        {
          LogMsg(DBG_INFO, "startSniffer() : Scanner started. Waiting for replies on device \"%s\" ...", lAdapter);
          // Start intercepting data packets.
          pcap_loop((pcap_t *) lScanParams.IfcReadHandle, 0, (pcap_handler) DestinationAccountingCallback, (unsigned char *) &lScanParams);
        } // if (pcap_setfilter...
      } // if (pcap_compile((...
   	}
   	else
	   {
      LogMsg(DBG_ERROR, "startSniffer() : Unable to open the adapter \"%s\"", lScanParams.IFCName);
    } // if ((lIFCHandle ...
  }
  else
  {
    LogMsg(DBG_ERROR, "startSniffer() : Error in pcap_findalldevs_ex() : %s", lTemp);
  } // if (pcap_finda... 
}




/*
 *
 *
 */
DWORD WINAPI DestinationVisualisationThread(LPVOID lpParam)
{
  char lService[MAX_BUF_SIZE + 1];
  int lCounter = 0;
  PHOSTNODE lNode;
  DWORD lNSLookupThreadID = 0;
  WSADATA lWSAData = {0};
  IPREVERSELOOKUP lIPLookup;

  if (WSAStartup(MAKEWORD(2, 2), &lWSAData)!= 0) 
  {
    printf("WSAStartup failed: %d\n", GetLastError());
    return 1;
  }




  while (1)
  {
    

   
    /*
     * Generate XML output
     */
    if (gXML == 1)
    {
	     printf("<traffic>\n");

      if ((lNode = gHosts) != NULL)
      {
        for (lNode = gHosts; lNode; lNode = lNode->next)
        {
          if (lNode->first == 0)
          {
            /*
             * Reverse look up the IP address
             */
            if (lNode->sData.Hostname[0] == NULL)
            {
              ZeroMemory(&lIPLookup, sizeof(lIPLookup));
              lIPLookup.Hostname = lNode->sData.Hostname;
              lIPLookup.IPAddressStr = lNode->sData.IPaddressStr;
              if (CreateThread(NULL, 0, IPLookup, (LPVOID) &lIPLookup, 0, &lNSLookupThreadID) == NULL)              
                strcpy((char *) lNode->sData.Hostname, "unknown");

            } // if (lNod...


            printf("  <entry>\n");
            printf("    <basis>%-20s %s</basis>\n", lNode->sData.IPaddressStr, lNode->sData.Hostname);
            printf("    <packetcounter>%d</packetcounter>\n", lNode->sData.PacketCounter);
    		      printf("    <datavolume>%d</datavolume>\n", lNode->sData.DataVolume);
     	      printf("    <lastupdate>%s</lastupdate>\n", lNode->sData.LastUpdate);
            printf("  </entry>\n");
          } // if (lNod...
        } // for (lCount = ...
      } // if (pMAC != ...

	     printf("</traffic>\n");
	     printf("<EOF>\n");
    }
    else
    {
      system("cls");
     	printf("%10s %12s %15s    %-20s    %s\n\n", "No. packets", "Data volume", "Last packet", "Destination IP", "Host name");

      if ((lNode = gHosts) != NULL)
      {
        for (lNode = gHosts; lNode; lNode = lNode->next)
        {
          if (lNode->first == 0)
          {
            /*
             * Reverse look up the IP address
             */
            if (lNode->sData.Hostname[0] == NULL)
            {
              ZeroMemory(&lIPLookup, sizeof(lIPLookup));
              lIPLookup.Hostname = lNode->sData.Hostname;
              lIPLookup.IPAddressStr = lNode->sData.IPaddressStr;
              if (CreateThread(NULL, 0, IPLookup, (LPVOID) &lIPLookup, 0, &lNSLookupThreadID) == NULL)
                strcpy((char *) lNode->sData.Hostname, "unknown");
            } // if (lNod...

            printf("%10d %12d %15s    %-20s    %s\n", lNode->sData.PacketCounter, lNode->sData.DataVolume, 
               lNode->sData.LastUpdate, lNode->sData.IPaddressStr, lNode->sData.Hostname);

          } // if (lNo...
        } // for (lNo...
      } // if ((lNode..
    } // if (gXML ==...


   	fflush(stdout);
	   fflush(stderr);

    Sleep(1000);
  } // while (1)
}




/*
 *
 *
 */
void ProcessDestinationIPTraffic(PSCANPARAMS pScanParams, char *pProtocol, IPADDRESS pSrcIP, IPADDRESS pDstIP, unsigned int pSrcPort, unsigned int pDstPort, int pDataLen)
{
  char lDstIP[MAX_IP_LEN];
  char lSrcIP[MAX_IP_LEN];
  PHOSTNODE lNode = NULL;
  unsigned long lLocalIP = 0;
  unsigned long lPeerIP1 = 0;
  unsigned long lPeerIP2 = 0;
  char lTimeStamp[128];
  time_t lRawtime;
  struct tm * lTimeinfo;

  CopyMemory(&lLocalIP, pScanParams->LocalIP, 4);
  CopyMemory(&lPeerIP1, &pSrcIP, 4);
  CopyMemory(&lPeerIP2, &pDstIP, 4);

  _snprintf(lSrcIP, sizeof(lSrcIP) - 1, "%d.%d.%d.%d", pSrcIP.byte1, pSrcIP.byte2, pSrcIP.byte3, pSrcIP.byte4);
  _snprintf(lDstIP, sizeof(lDstIP) - 1, "%d.%d.%d.%d", pDstIP.byte1, pDstIP.byte2, pDstIP.byte3, pDstIP.byte4);

  time (&lRawtime);
  lTimeinfo = localtime (&lRawtime);
  ZeroMemory(lTimeStamp, sizeof(lTimeStamp));
  strftime(lTimeStamp, sizeof(lTimeStamp) - 1, "%H:%M:%S", lTimeinfo);


  if (IPAddrBelongsToLocalNet(lLocalIP, pScanParams->Netmask, lPeerIP1) != 0)
  {
    if ((lNode = GetNodeByIP(gHosts, lPeerIP1)) != NULL || (lNode = AddRecordToList(&gHosts, lPeerIP1)) != NULL)
    {
      lNode->sData.DataVolume += pDataLen;
      lNode->sData.PacketCounter++;
      strncpy((char *) lNode->sData.LastUpdate, lTimeStamp, sizeof(lNode->sData.LastUpdate)-1);
    }   
  }
  else if (IPAddrBelongsToLocalNet(lLocalIP, pScanParams->Netmask, lPeerIP2) != 0)
  {
    if ((lNode = GetNodeByIP(gHosts, lPeerIP2)) != NULL || (lNode = AddRecordToList(&gHosts, lPeerIP2)) != NULL)
    {
      lNode->sData.DataVolume += pDataLen;
      lNode->sData.PacketCounter++;
      strncpy((char *) lNode->sData.LastUpdate, lTimeStamp, sizeof(lNode->sData.LastUpdate)-1);
    }
  }
  else
  {
//    printf("ProcessDestinationIPTraffic(2) : (%s) Unknown  %s:%d->%s:%d   %d\n", pProtocol, lSrcIP, pSrcPort, lDstIP, pDstPort, pDataLen);
  }
}

