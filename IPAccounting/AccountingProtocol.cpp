#define HAVE_REMOTE


#include <stdlib.h>
#include <stdio.h>
#include <time.h>
#include <pcap.h>
#include <Shlwapi.h>
#include "IPAccounting.h"
#include "NetPackets.h"
#include "AccountingProtocol.h"
#include "GeneralFunctions.h"

extern SERVICEDATA gServices[MAX_SERVICES];
extern int gNumServices;
extern int gXML;
extern int gFILTER;


/*
 *
 *
 */
void StartAccounting(PSCANPARAMS pScanParams)
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


  gNumServices = ParseServicesConfigFile("Service_Definitions.txt");

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
      } // if (gFILTER...


      if (pcap_compile((pcap_t *) lScanParams.IfcReadHandle, &lFCode, (const char *) lFilter, 1, lNetMask) >= 0)
      {
        if (pcap_setfilter((pcap_t *) lScanParams.IfcReadHandle, &lFCode) >= 0)
        {
          LogMsg(DBG_INFO, "startSniffer() : Scanner started. Waiting for replies on device \"%s\" ...", lAdapter);
          // Start intercepting data packets.
          pcap_loop((pcap_t *) lScanParams.IfcReadHandle, 0, (pcap_handler) AccountingCallback, (unsigned char *) &lScanParams);
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
 * Callback function invoked by libpcap for every incoming packet 
 *
 */
void AccountingCallback(unsigned char *pScanParams, struct pcap_pkthdr *pPcapHdr, unsigned char *pPktData)
{
  PETHDR lEthHdr = (PETHDR) pPktData;
  PIPHDR lIPHdr = NULL;
  PTCPHDR lTCPHdr = NULL;
  PUDPHDR lUDPHdr = NULL;
  int lIPHdrLen = 0;
  int lTotLen = 0;
  int lTCPHdrLen = 0;
  int lTCPDataLen = 0;
  int lICMPDataLen = 0;
  int lUDPDataLen = 0;
  int lUDPHdrLen = 0;

  unsigned short lType = htons(lEthHdr->ether_type);
  int lSrcPort = 0;
  int lDstPort = 0;


  if (lType == ETHERTYPE_IP)
  {
    lIPHdr = (PIPHDR) (pPktData + 14);
    lIPHdrLen = (lIPHdr->ver_ihl & 0xf) * 4;
   	lTotLen = ntohs(lIPHdr->tlen);


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

      ProcessIPTraffic("TCP", lSrcPort, lDstPort, lTCPDataLen);


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
      ProcessIPTraffic("UDP", lSrcPort, lDstPort, lUDPDataLen);

   	/*
   	 * ICMP
   	 */
    }
    else if (lIPHdr->proto == IP_ICMP)
    {
      lSrcPort = ICMP_PSEUDO_PORT;
      lDstPort = ICMP_PSEUDO_PORT;
      lICMPDataLen = lTotLen - lIPHdrLen;

      ProcessIPTraffic(ICMP_PSEUDO_SERVICE, lSrcPort, lDstPort, lICMPDataLen);

    } // if (pIPHdr->pro...
  } // if (lType == ET...
}


/*
 *
 *
 */
void ProcessIPTraffic(char *pProtocol, unsigned int pSrcPort, unsigned int pDstPort, int pDataLen)
{
  PSERVICEDATA lService = NULL;
  char lTimeStamp[SERVICE_NAME_BUFFER_LENGTH + 1];
  time_t lRawtime;
  struct tm * lTimeinfo;
  int lPort = 0;

  time (&lRawtime);
  lTimeinfo = localtime (&lRawtime);


  ZeroMemory(lTimeStamp, sizeof(lTimeStamp));
  //  strftime(lTimeStamp, sizeof(lTimeStamp) - 1, "%Y.%m.%d %k:%M:%S", lTimeinfo);
  strftime(lTimeStamp, sizeof(lTimeStamp) - 1, "%H:%M:%S", lTimeinfo);
  lPort = MapServicePort(pSrcPort, pDstPort);
  

  if ((lService = GetServiceEntry(lPort)) != NULL)
  {
    lService->PacketCounter++;
   	lService->DataVolume = lService->DataVolume + pDataLen;
	   strncpy((char *) lService->LastUpdate, lTimeStamp, sizeof(lService->LastUpdate)-1);
  } // if ((lSe...
}



/*
 *
 *
 */
int MapServicePort(int pSrcPort, int pDstPort)
{
  int lCounter;

  for (lCounter = 0 ; lCounter < gNumServices; lCounter++)
  {    
    if (gServices[lCounter].PortNumberLow >= pSrcPort && gServices[lCounter].PortNumberUp <= pSrcPort)
      return(pSrcPort);
    else if (gServices[lCounter].PortNumberLow >= pDstPort && gServices[lCounter].PortNumberUp <= pDstPort)
      return(pDstPort);
  } // for (lService...

  return(0);
}



/*
 *
 *
 */
PSERVICEDATA GetServiceEntry(int pPortNumber)
{
  int lCounter;

  for (lCounter = 0 ; lCounter < gNumServices; lCounter++)
    if (gServices[lCounter].PortNumberLow >= pPortNumber && gServices[lCounter].PortNumberUp <= pPortNumber)
      break;
  
  return(&gServices[lCounter]);
}




/*
 *
 *
 */
DWORD WINAPI ProtocolVisualisationThread (LPVOID lpParam)
{
  char lService[MAX_BUF_SIZE + 1];
  int lCounter = 0;
  PSERVICEDATA lSrvc = NULL;
  

  while (1)
  {
    
    if (gXML == 1)
    {
     	printf("<traffic>\n");

      for (lCounter = 0 ; lCounter < gNumServices; lCounter++)
      {
        if (gServices[lCounter].PacketCounter > 0)
	       {
          printf("  <entry>\n");
          if (gServices[lCounter].PortNumberLow == gServices[lCounter].PortNumberUp)
            snprintf(lService, sizeof(lService)-1, "%s (%d)", gServices[lCounter].ServiceName, gServices[lCounter].PortNumberLow);
          else
            snprintf(lService, sizeof(lService)-1, "%s (%d-%d)", gServices[lCounter].ServiceName, gServices[lCounter].PortNumberLow, gServices[lCounter].PortNumberUp);

          printf("    <basis>%s</basis>\n", lService);
          printf("    <packetcounter>%d</packetcounter>\n", gServices[lCounter].PacketCounter);
      		  printf("    <datavolume>%d</datavolume>\n", gServices[lCounter].DataVolume);
	      	  printf("    <lastupdate>%s</lastupdate>\n", gServices[lCounter].LastUpdate);
          printf("  </entry>\n");
	       } // if (lSr...
	     } // for (lSrvc = ...

	     printf("</traffic>\n");
	     printf("<EOF>\n");
    }
    else
    {
      system("cls");
      printf("\n%-25s %10s %12s %12s\n\n", "Service (Port)", "Packets", "Volume", "Timestamp");

      for (lCounter = 0 ; lCounter < gNumServices; lCounter++)
      {
        if (gServices[lCounter].PacketCounter > 0)
	       {
          if (gServices[lCounter].PortNumberLow == gServices[lCounter].PortNumberUp)
            snprintf(lService, sizeof(lService)-1, "%s (%d)", gServices[lCounter].ServiceName, gServices[lCounter].PortNumberLow);
          else
            snprintf(lService, sizeof(lService)-1, "%s (%d-%d)", gServices[lCounter].ServiceName, gServices[lCounter].PortNumberLow, gServices[lCounter].PortNumberUp);

		        printf("%-25s %10d %12lu %12s\n", lService, gServices[lCounter].PacketCounter, gServices[lCounter].DataVolume, gServices[lCounter].LastUpdate);
	       } // if (lSr...
	     } // for (lSrvc = ...
    } // if (gXML... 

   	fflush(stdout);
	   fflush(stderr);

    Sleep(1000);
  } // while (1)
}







/*
 *
 *
 */
int ParseServicesConfigFile(char *pTargetsFile)
{
  unsigned char lPortLowStr[128];
  unsigned char lPortUpStr[128];
  unsigned char lServiceName[128];
  unsigned int lPortLow;
  unsigned int lPortUp;
  int lCounter = 0;

  FILE *lFH = NULL;
  char lLine[MAX_BUF_SIZE + 1];
  char lTemp[8];


  if (pTargetsFile != NULL && (lFH = fopen(pTargetsFile, "r")) != NULL)
  {
    ZeroMemory(lLine, sizeof(lLine));
   	ZeroMemory(lPortLowStr, sizeof(lPortLowStr));
    ZeroMemory(lPortUpStr, sizeof(lPortUpStr));
	   ZeroMemory(lServiceName, sizeof(lServiceName));
    lPortLow = lPortUp = 0;

    ZeroMemory(&gServices, sizeof(gServices));

    while (fgets(lLine, sizeof(lLine), lFH) != NULL)
    {
      while (lLine[strlen(lLine)-1] == '\r' || lLine[strlen(lLine)-1] == '\n')
        lLine[strlen(lLine)-1] = '\0';


      // parse values and add them to the list.
      if (sscanf(lLine, "%[^,],%[^,],%s", lPortLowStr, lPortUpStr, lServiceName) == 3)
      {
        lPortLow = atoi((char *) lPortLowStr);
        lPortUp = atoi((char *) lPortUpStr);
        gServices[lCounter].PortNumberLow = lPortLow;
        gServices[lCounter].PortNumberUp = lPortUp;
        strncpy((char *) gServices[lCounter].ServiceName, (char *) lServiceName, strnlen((char *) lServiceName, sizeof(lServiceName)-1));

        lCounter++;
      } // if (sscanf(lL...

      ZeroMemory(lLine, sizeof(lLine));
   	  ZeroMemory(lPortLowStr, sizeof(lPortLowStr));
      ZeroMemory(lPortUpStr, sizeof(lPortUpStr));
	     ZeroMemory(lServiceName, sizeof(lServiceName));
      lPortLow = lPortUp = 0;
    } // while (fgets(...

  // Adding ICMP as a protocol. Somewhat of a hack but ignoring ICMP is no option.    
  strncpy((char *) gServices[lCounter].ServiceName, ICMP_PSEUDO_SERVICE, strlen(ICMP_PSEUDO_SERVICE));
  gServices[lCounter].PortNumberLow = ICMP_PSEUDO_PORT;
  gServices[lCounter].PortNumberUp = ICMP_PSEUDO_PORT;
  lCounter++;



    fclose(lFH);
  } // if (pTargetsFile != NULL ...

  return(lCounter);
}