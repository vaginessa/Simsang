#define HAVE_REMOTE

#include <stdio.h>
#include <pcap.h>
#include <Windows.h>
#include <Shlwapi.h>
#include <iphlpapi.h>

#include "ARPScan.h"
#include "LinkedListSystems.h"


#pragma comment(lib, "iphlpapi.lib")
#pragma comment(lib, "ws2_32.lib")
#pragma comment(lib, "Shlwapi.lib")
#pragma comment(lib, "wpcap.lib")

// Warning C4996: This function or variable may be unsafe ... use _CRT_SECURE_NO_WARNINGS. 
// See online help for details.
#pragma warning(disable: 4996)


/*
* Global variables
*
*/
CRITICAL_SECTION gWriteLog; 
CRITICAL_SECTION gCSSystemsLL; 
PSYSTEMNODE gSystemsList = NULL;




/*
* Program entry point
* Programm call : argv[0] Ifc StartIP StopIP
*
*/
int main(int argc, char **argv)
{
  DWORD lRetVal = 0;
  ARPPacket lARPPacket;
  SCANPARAMS lScanParams;
  unsigned long lIPCounter = 0;
  unsigned long lIPCounterHostOrder = 0;
  unsigned long lStartIP = 0;
  unsigned long lStopIP = 0;
  unsigned long lDstIP = 0;
  HANDLE lThreadHandle = INVALID_HANDLE_VALUE;
  DWORD lThreadId = 0;
  int lCounter = 0;
  pcap_if_t *lAllDevs = NULL;
  pcap_if_t *lDevice = NULL;
  char lTemp[PCAP_ERRBUF_SIZE];

  HANDLE lICMPFile = INVALID_HANDLE_VALUE;
  char lSendData[32] = "Data Buffer";
  DWORD lReplySize = 0;
  LPVOID lReplyBuffer = NULL;
  unsigned long ipaddr = 0;
  HANDLE lARPReplyThreadHandle = INVALID_HANDLE_VALUE;
  DWORD lARPReplyThreadID = 0;
  struct sockaddr_in lPeerIP;
  char lPeerIPStr[MAX_BUF_SIZE + 1];
  char lAdapter[MAX_BUF_SIZE + 1];

  char lFilter[1024];
  bpf_u_int32 lNetMask;
  struct bpf_program lFCode;

  ZeroMemory(&lFCode, sizeof(lFCode));
  ZeroMemory(lFilter, sizeof(lFilter));

  _snprintf(lFilter, sizeof(lFilter) - 1, "arp");
  lNetMask = 0xffffff; // "255.255.255.0"

  /*
  * Initialisation
  */
  InitializeCriticalSectionAndSpinCount(&gWriteLog, 0x00000400);
  InitializeCriticalSectionAndSpinCount(&gCSSystemsLL, 0x00000400);

  gSystemsList = InitSystemList();


  if (argc >= 4)
  {
    /*
    * Open device list
    */
    if (pcap_findalldevs_ex(PCAP_SRC_IF_STRING, NULL, &lAllDevs, lTemp) == -1)
    {
      lRetVal = 1;
      goto END;
    }


    ZeroMemory(lAdapter, sizeof(lAdapter));
    lCounter = 0;

    for(lCounter = 0, lDevice = lAllDevs; lDevice; lDevice = lDevice->next, lCounter++)
    {
      if (StrStrI(lDevice->name, argv[1]))
      {
        strcpy(lAdapter, lDevice->name);
        break;
      } // if (StrSt...
    } // for(lCoun...


    // We dont need this list anymore.
    pcap_freealldevs(lAllDevs);


    if (lAdapter == NULL || strnlen(lAdapter, sizeof(lAdapter)-1) <= 0)
    {
      lRetVal = 2;
      goto END;
    }

    ZeroMemory(&lScanParams, sizeof(lScanParams));
    ZeroMemory(&lARPPacket, sizeof(lARPPacket));
    GetIFCDetails(lAdapter, &lScanParams);
    strncpy(lScanParams.IFCString, lAdapter, sizeof(lScanParams.IFCString)-1);
    lStartIP = ntohl(inet_addr(argv[2]));
    lStopIP = ntohl(inet_addr(argv[3]));



    /*
    * Start ARP Reply listener thread
    */
    if (lStartIP <= lStopIP)
    {
      if ((lScanParams.IfcWriteHandle = pcap_open(lAdapter, 48, PCAP_OPENFLAG_NOCAPTURE_LOCAL|PCAP_OPENFLAG_MAX_RESPONSIVENESS, 1, NULL, lTemp)) != NULL)      
      {
        if (pcap_compile((pcap_t *) lScanParams.IfcWriteHandle, &lFCode, (const char *) lFilter, 1, lNetMask) >= 0)
        {
          // Set the filter
          if (pcap_setfilter((pcap_t *) lScanParams.IfcWriteHandle, &lFCode) >= 0)
          {
            if ((lARPReplyThreadHandle = CreateThread(NULL, 0, CaptureARPReplies, &lScanParams, 0, &lARPReplyThreadID)) != NULL)      
            {
              for (lIPCounter = lStartIP; lIPCounter <= lStopIP; lIPCounter++)
              {
                if (memcmp(lScanParams.LocalIP, &lIPCounter, BIN_IP_LEN) &&
                  memcmp(lScanParams.GWIP, &lIPCounter, BIN_IP_LEN))
                {
                  // Send WhoHas ARP request and sleep ...
                  SendARPWhoHas(&lScanParams, lIPCounter);

                  lPeerIP.sin_addr.s_addr = htonl(lIPCounter);
                  strncpy(lPeerIPStr, inet_ntoa(lPeerIP.sin_addr), sizeof(lPeerIPStr)-1);

                  Sleep(SLEEP_BETWEEN_ARPS);
                } // if (memcmp...
              } // for (lIPCounter = lStartI...

              /*
              * Wait for all ARP replies and terminate thread.
              */
              Sleep(5000);
              TerminateThread(lARPReplyThreadHandle, 0);
              CloseHandle(lARPReplyThreadHandle);


              if (lScanParams.IfcWriteHandle)
                pcap_close((pcap_t *) lScanParams.IfcWriteHandle);

            } // if ((lIFCHandle...
          } // if(pcap_setfilter(...
        } // if(pcap_compile((pc...
      } // if (lStart...
    } // if ((lPOISO...
  } // if (argc >= 4)...


END:

  DeleteCriticalSection(&gWriteLog);
  DeleteCriticalSection(&gCSSystemsLL);


  return(lRetVal);
}



/*
*
*
*/
DWORD WINAPI CaptureARPReplies(LPVOID pScanParams)
{
  pcap_t *lIFCHandle = NULL;
  char lTemp[1024];
  int lPcapRetVal = 0;
  PETHDR lEHdr = NULL;
  PARPHDR lARPHdr = NULL;
  u_char *lPktData = NULL;
  struct pcap_pkthdr *lPktHdr = NULL;
  PSCANPARAMS lScanParams = (PSCANPARAMS) pScanParams;
  unsigned char lTmpPkt[256];
  unsigned int lTmpSize;
  unsigned char lEthDstStr[MAX_MAC_LEN+1];
  unsigned char lEthSrcStr[MAX_MAC_LEN+1];
  unsigned char lARPEthDstStr[MAX_MAC_LEN+1];
  unsigned char lARPEthSrcStr[MAX_MAC_LEN+1];
  unsigned char lARPIPDstStr[MAX_IP_LEN+1];
  unsigned char lARPIPSrcStr[MAX_IP_LEN+1];


  if ((lIFCHandle = pcap_open((char *) lScanParams->IFCString, 64, PCAP_OPENFLAG_PROMISCUOUS, 1, NULL, lTemp)) != NULL)
  {
    while ((lPcapRetVal = pcap_next_ex(lIFCHandle,  &lPktHdr, (const u_char **) &lPktData)) >= 0)
    {
      if (lPcapRetVal == 1)      
      {
        lTmpSize = lPktHdr->len>255?255:lPktHdr->len;
        ZeroMemory(lTmpPkt, 256);
        CopyMemory(lTmpPkt, lPktData, lTmpSize);

        lEHdr = (PETHDR) lTmpPkt;
        lARPHdr = (PARPHDR) (lTmpPkt + sizeof(ETHDR));

        if (ntohs(lARPHdr->oper) == ARP_REPLY)
        {
          ZeroMemory(lEthDstStr, sizeof(lEthDstStr));
          ZeroMemory(lEthSrcStr, sizeof(lEthSrcStr));
          ZeroMemory(lARPEthSrcStr, sizeof(lARPEthSrcStr));
          ZeroMemory(lARPEthDstStr, sizeof(lARPEthDstStr));
          ZeroMemory(lARPIPDstStr, sizeof(lARPIPDstStr));
          ZeroMemory(lARPIPSrcStr, sizeof(lARPIPSrcStr));

          MAC2String(lEHdr->ether_shost, lEthSrcStr, sizeof(lEthSrcStr)-1);
          MAC2String(lEHdr->ether_dhost,  lEthDstStr, sizeof(lEthDstStr)-1);
          MAC2String(lARPHdr->sha,  lARPEthSrcStr, sizeof(lARPEthSrcStr)-1);
          MAC2String(lARPHdr->tha,  lARPEthDstStr, sizeof(lARPEthDstStr)-1);

          IP2String(lARPHdr->tpa, lARPIPDstStr, sizeof(lARPIPDstStr)-1);
          IP2String(lARPHdr->spa, lARPIPSrcStr, sizeof(lARPIPSrcStr)-1);


          if (GetNodeByMAC(gSystemsList, lARPHdr->sha) == NULL)
          {

            AddToList(&gSystemsList, lARPHdr->spa, lARPHdr->sha);

            ZeroMemory(lTemp, sizeof(lTemp));                
            _snprintf(lTemp, sizeof(lTemp)-1, "<arp>\n  <type>reply</type>\n  <ip>%s</ip>\n  <mac>%s</mac>\n</arp>\n<EOF>", lARPIPSrcStr, lEthSrcStr);
            LogMsg(lTemp);
          }
        }
      }
    }
  }

  return(0);
}





/*
* Ethr:	LocalMAC -> 255:255:255:255:255:255a
* ARP :	LocMAC/LocIP -> 0:0:0:0:0:0/VicIP
*
*/
int SendARPWhoHas(PSCANPARAMS pScanParams, unsigned long lIPAddress)
{
  int lRetVal = OK;
  unsigned long lDstIP = 0;
  ARPPacket lARPPacket;  
  int i = 0;

  lDstIP = htonl(lIPAddress);
  lARPPacket.lReqType = ARP_REQUEST;

  // Set src/dst MAC values
  CopyMemory(lARPPacket.Eth_SrcMAC, pScanParams->LocalMAC, BIN_MAC_LEN);
  memset(lARPPacket.Eth_DstMAC, 255, sizeof(lARPPacket.Eth_DstMAC));


  // Set ARP request values
  CopyMemory(lARPPacket.ARP_LocalMAC, pScanParams->LocalMAC, BIN_MAC_LEN);
  CopyMemory(lARPPacket.ARP_LocalIP, pScanParams->LocalIP, BIN_IP_LEN);
  CopyMemory(&lARPPacket.ARP_DstIP[0], &lDstIP, BIN_IP_LEN);

  // Send packet
  if (SendARPPacket(pScanParams->IfcWriteHandle, &lARPPacket) != 0)
  {
    //    LogMsg("SendARPWhoHas() : Unable to send ARP packet.\n");
    lRetVal = NOK;
  } // if (SendARPPacket(lIF...

  return(lRetVal);
}




/*
*
*
*/
int SendARPPacket(void *pIFCHandle, PARPPacket pARPPacket)
{
  int lRetVal = NOK;
  unsigned char lARPPacket[sizeof(ETHDR) + sizeof(ARPHDR)];
  int lCounter = 0;
  PETHDR lEHdr = (PETHDR) lARPPacket;
  PARPHDR lARPHdr = (PARPHDR) (lARPPacket + 14);


  ZeroMemory(lARPPacket, sizeof(lARPPacket));

  /*
  * Layer 2 (Physical)
  */
  CopyMemory(lEHdr->ether_shost, pARPPacket->Eth_SrcMAC, BIN_MAC_LEN);
  CopyMemory(lEHdr->ether_dhost, pARPPacket->Eth_DstMAC, BIN_MAC_LEN);
  lEHdr->ether_type = htons(ETHERTYPE_ARP);


  /*
  * Layer 2/3
  */
  lARPHdr->htype = htons(0x0001); // Ethernet
  lARPHdr->ptype = htons(0x0800); // Protocol type on the upper layer : IP
  lARPHdr->hlen = 0x0006; // Ethernet address length : 6
  lARPHdr->plen = 0x0004; // Number of octets in upper protocol layer : 4
  lARPHdr->oper = htons(pARPPacket->lReqType);

  CopyMemory(lARPHdr->tpa, pARPPacket->ARP_DstIP, BIN_IP_LEN);
  CopyMemory(lARPHdr->tha, pARPPacket->ARP_Dst_MAC, BIN_MAC_LEN);

  CopyMemory(lARPHdr->spa, pARPPacket->ARP_LocalIP, BIN_IP_LEN);
  CopyMemory(lARPHdr->sha, pARPPacket->ARP_LocalMAC, BIN_MAC_LEN);


  /* 
  * Send down the packet
  */
  if (pIFCHandle != NULL && pcap_sendpacket((pcap_t *) pIFCHandle, lARPPacket, sizeof(ETHDR) + sizeof(ARPHDR)) == 0)
    lRetVal = OK;
  //  else
  // 	  LogMsg("SendARPPacket() : Error occured while sending the packet: %s\n", pcap_geterr((pcap_t *) pIFCHandle));


  return(lRetVal);
}




void MAC2String(unsigned char pMAC[BIN_MAC_LEN], unsigned char *pOutput, int pOutputLen)
{
  if (pOutput && pOutputLen > 0)
    _snprintf((char *) pOutput, pOutputLen, "%02X:%02X:%02X:%02X:%02X:%02X", pMAC[0], pMAC[1], pMAC[2], pMAC[3], pMAC[4], pMAC[5]);

}

void IP2String(unsigned char pIP[BIN_IP_LEN], unsigned char *pOutput, int pOutputLen)
{
  if (pOutput && pOutputLen > 0)
    _snprintf((char *) pOutput, pOutputLen, "%d.%d.%d.%d", pIP[0], pIP[1], pIP[2], pIP[3]);
}



/*
*
*
*/
int GetIFCDetails(char *pIFCName, PSCANPARAMS pScanParams)
{
  int lRetVal = 0;
  unsigned long lLocalIPAddr = 0;
  unsigned long lGWIPAddr = 0;
  ULONG lGWMACAddr[2];
  ULONG lGWMACAddrLen = 6;
  PIP_ADAPTER_INFO lAdapterInfoPtr = NULL;
  PIP_ADAPTER_INFO lAdapter = NULL;
  DWORD lFuncRetVal = 0;
  ULONG lOutBufLen = sizeof (IP_ADAPTER_INFO);


  if ((lAdapterInfoPtr = (IP_ADAPTER_INFO *) HeapAlloc(GetProcessHeap(), 0, sizeof (IP_ADAPTER_INFO))) == NULL)
  {
    lRetVal = 1;
    goto END;
  } // if ((lAdapterInfo...


  if (GetAdaptersInfo(lAdapterInfoPtr, &lOutBufLen) == ERROR_BUFFER_OVERFLOW) 
  {
    HeapFree(GetProcessHeap(), 0, lAdapterInfoPtr);
    if ((lAdapterInfoPtr = (IP_ADAPTER_INFO *) HeapAlloc(GetProcessHeap(), 0, lOutBufLen)) == NULL)
    {
      lRetVal = 2;
      goto END;
    } // if ((lAdap...
  } // if (GetAdapte...



  /*
  *
  */
  if ((lFuncRetVal = GetAdaptersInfo(lAdapterInfoPtr, &lOutBufLen)) == NO_ERROR) 
  {
    for (lAdapter = lAdapterInfoPtr; lAdapter; lAdapter = lAdapter->Next)
    {
      if (StrStrI(pIFCName, lAdapter->AdapterName))
      {
        // Get local MAC address
        CopyMemory(pScanParams->LocalMAC, lAdapter->Address, BIN_MAC_LEN);

        // Get local IP address
        lLocalIPAddr = inet_addr(lAdapter->IpAddressList.IpAddress.String);
        CopyMemory(pScanParams->LocalIP, &lLocalIPAddr, 4);


        // Get gateway IP address
        lGWIPAddr = inet_addr(lAdapter->GatewayList.IpAddress.String);
        CopyMemory(pScanParams->GWIP, &lGWIPAddr, 4);


        // Get gateway MAC address
        CopyMemory(pScanParams->GWIP, &lGWIPAddr, 4); // ????
        ZeroMemory(&lGWMACAddr, sizeof(lGWMACAddr));
        SendARP(lGWIPAddr, 0, lGWMACAddr, &lGWMACAddrLen);
        CopyMemory(pScanParams->GWMAC, lGWMACAddr, 6);

        // Get interface index.
        pScanParams->Index = lAdapter->Index;

        // Get interface description
        CopyMemory(pScanParams->IFCDescr, lAdapter->Description, sizeof(pScanParams->IFCDescr) - 1);

        break;
      } // if (StrSt...
    } // for (lAdapt...


  }
  else
  {
    lRetVal = 1;
  } // if ((lFunc...

END:
  if (lAdapterInfoPtr)
    HeapFree(GetProcessHeap(), 0, lAdapterInfoPtr);


  return(lRetVal);
}


/*
*
*
*/
void LogMsg(char *pMsg, ...)
{
  HANDLE lFH = INVALID_HANDLE_VALUE;
  OVERLAPPED lOverl = { 0 };
  char lTemp[MAX_BUF_SIZE + 1];
  char lLogMsg[MAX_BUF_SIZE + 1];
  DWORD lBytedWritten = 0;
  va_list lArgs;


  EnterCriticalSection(&gWriteLog);

  /*
  * Create log message
  */
  ZeroMemory(lTemp, sizeof(lTemp));
  ZeroMemory(lLogMsg, sizeof(lLogMsg));
  va_start (lArgs, pMsg);
  vsprintf(lTemp, pMsg, lArgs);
  va_end(lArgs);
  _snprintf(lLogMsg, sizeof(lLogMsg) - 1, "%s\n", lTemp);
  fprintf(stdout, lLogMsg);
  //fprintf(stderr, lLogMsg);
  fflush(stdout);
  //fflush(stderr);

  LeaveCriticalSection(&gWriteLog);
}