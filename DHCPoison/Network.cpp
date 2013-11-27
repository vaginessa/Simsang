#define HAVE_REMOTE

#include <pcap.h>
#include <stdio.h>
#include <stdlib.h>
#include <iphlpapi.h>
#include <Shlwapi.h>

#include "DHCPoison.h"
#include "HelperFunctions.h"



/*
 *
 *
 */
int ListIFCDetails()
{
  int lRetVal = 0;
  PIP_ADAPTER_INFO lAdapterInfoPtr = NULL;
  PIP_ADAPTER_INFO lAdapter = NULL;
  DWORD lFuncRetVal = 0;
  UINT lCounter;
  struct tm lTimeStamp;
  char lTemp[MAX_BUF_SIZE +1 ];
  errno_t error;
  ULONG lOutBufLen = sizeof (IP_ADAPTER_INFO);



  if ((lAdapterInfoPtr = (IP_ADAPTER_INFO *) HeapAlloc(GetProcessHeap(), 0, sizeof (IP_ADAPTER_INFO))) == NULL)
  {
    LogMsg(DBG_ERROR, "listIFCDetails() : Error allocating memory needed to call GetAdaptersinfo");
    lRetVal = 1;
    goto END;
  } // if ((lAdapter...


  if (GetAdaptersInfo(lAdapterInfoPtr, &lOutBufLen) == ERROR_BUFFER_OVERFLOW) 
  {
    HeapFree(GetProcessHeap(), 0, lAdapterInfoPtr);
    if ((lAdapterInfoPtr = (IP_ADAPTER_INFO *) HeapAlloc(GetProcessHeap(), 0, lOutBufLen)) == NULL)
    {
      LogMsg(DBG_ERROR, "listIFCDetails() : Error allocating memory needed to call GetAdaptersinfo");
      lRetVal = 2;

      goto END;
    } // if ((lAdapte...
  } // if (GetA...





  /*
   *
   *
   */
  if ((lFuncRetVal = GetAdaptersInfo(lAdapterInfoPtr, &lOutBufLen)) == NO_ERROR) 
  {
    for (lAdapter = lAdapterInfoPtr; lAdapter; lAdapter = lAdapter->Next)
    {
      printf("\n\nIfc no : %d\n", lAdapter->ComboIndex);
      printf("\tAdapter Name: \t%s\n", lAdapter->AdapterName);
      printf("\tAdapter Desc: \t%s\n", lAdapter->Description);
      printf("\tAdapter Addr: \t");

      for (lCounter = 0; lCounter < lAdapter->AddressLength; lCounter++) 
      {
        if (lCounter == (lAdapter->AddressLength - 1))
          printf("%.2X\n", (int) lAdapter->Address[lCounter]);
        else
          printf("%.2X-", (int) lAdapter->Address[lCounter]);
      }

      printf("\tIndex: \t%d\n", lAdapter->Index);
      printf("\tType: \t");

      switch (lAdapter->Type) 
      {
         case MIB_IF_TYPE_OTHER:
              printf("Other\n");
              break;
         case MIB_IF_TYPE_ETHERNET:
              printf("Ethernet\n");
              break;
         case MIB_IF_TYPE_TOKENRING:
              printf("Token Ring\n");
              break;
         case MIB_IF_TYPE_FDDI:
              printf("FDDI\n");
              break;
         case MIB_IF_TYPE_PPP:
              printf("PPP\n");
              break;
         case MIB_IF_TYPE_LOOPBACK:
              printf("Lookback\n");
              break;
         case MIB_IF_TYPE_SLIP:
              printf("Slip\n");
              break;
         default:
              printf("Unknown type %ld\n", lAdapter->Type);
              break;
      }

      printf("\tIP Address: \t%s\n", lAdapter->IpAddressList.IpAddress.String);
      printf("\tIP Mask: \t%s\n", lAdapter->IpAddressList.IpMask.String);
      printf("\tGateway: \t%s\n", lAdapter->GatewayList.IpAddress.String);

      if (lAdapter->DhcpEnabled) 
      {
        printf("\tDHCP Enabled: Yes\n");
        printf("\t  DHCP Server: \t%s\n", lAdapter->DhcpServer.IpAddress.String);
        printf("\t  Lease Obtained: ");

        if (error = _localtime32_s(&lTimeStamp, (__time32_t*) &lAdapter->LeaseObtained))
          printf("Invalid Argument to _localtime32_s\n");
        else {
          if (error = asctime_s(lTemp, sizeof(lTemp), &lTimeStamp))
            printf("Invalid Argument to asctime_s\n");
          else
            printf("%s", lTemp);
        }

        printf("\t  Lease Expires:  ");

        if (error = _localtime32_s(&lTimeStamp, (__time32_t*) &lAdapter->LeaseExpires))
          printf("Invalid Argument to _localtime32_s\n");
        else {
          // Convert to an ASCII representation 
          if (error = asctime_s(lTemp, sizeof(lTemp), &lTimeStamp))
            printf("Invalid Argument to asctime_s\n");
          else
            printf("%s", lTemp);
        }
      } else
        printf("\tDHCP Enabled: No\n");

      if (lAdapter->HaveWins) 
      {
        printf("\tHave Wins: Yes\n");
        printf("\t  Primary Wins Server:    %s\n", lAdapter->PrimaryWinsServer.IpAddress.String);
        printf("\t  Secondary Wins Server:  %s\n", lAdapter->SecondaryWinsServer.IpAddress.String);
      } else
         printf("\tHave Wins: No\n");
    }
  }
  else
    LogMsg(DBG_ERROR, "listIFCDetails() : GetAdaptersInfo failed with error: %d\n", lFuncRetVal);


END:
  if (lAdapterInfoPtr)
    HeapFree(GetProcessHeap(), 0, lAdapterInfoPtr);
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
    LogMsg(DBG_ERROR, "getIFCDetails() : Error allocating memory needed to call GetAdaptersinfo");
    lRetVal = 1;
    goto END;
  } // if ((lAdapterInfo...


  if (GetAdaptersInfo(lAdapterInfoPtr, &lOutBufLen) == ERROR_BUFFER_OVERFLOW) 
  {
    HeapFree(GetProcessHeap(), 0, lAdapterInfoPtr);
    if ((lAdapterInfoPtr = (IP_ADAPTER_INFO *) HeapAlloc(GetProcessHeap(), 0, lOutBufLen)) == NULL)
    {
      LogMsg(DBG_ERROR, "getIFCDetails() : Error allocating memory needed to call GetAdaptersinfo");
      lRetVal = 2;
      goto END;
    } // if ((lAdap...
  } // if (GetAdapte...




  /*
   *
   *
   */
  if ((lFuncRetVal = GetAdaptersInfo(lAdapterInfoPtr, &lOutBufLen)) == NO_ERROR) 
  {
    for (lAdapter = lAdapterInfoPtr; lAdapter; lAdapter = lAdapter->Next)
    {
      if (StrStrI(lAdapter->AdapterName, pIFCName))
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

		      // Get interface alias.
		      GetAliasByIfcIndex(pScanParams->Index, (char *) pScanParams->IFCAlias, sizeof(pScanParams->IFCAlias)-1);

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
int GetAliasByIfcIndex(int pIfcIndex, char *pAliasBuf, int pBufLen)
{
  int lRetVal = NOK;
  MIB_IF_ROW2 lIfcRow;

  if (pAliasBuf != NULL && pBufLen > 0)
  {
    SecureZeroMemory((PVOID) &lIfcRow, sizeof(MIB_IF_ROW2) );
    lIfcRow.InterfaceIndex = pIfcIndex;

    if (GetIfEntry2(&lIfcRow) == NO_ERROR) 
    {
	     _snprintf(pAliasBuf, pBufLen-1, "%ws", lIfcRow.Alias);
    } //  if (GetIfEntry2...
  } // if (pAliasBuf != N...

  return(lRetVal);
}



/*
 *
 *
 */
void SetMACStatic(char *pIfcAlias, char *pIP, char *pMAC)
{
  char lTemp[MAX_BUF_SIZE + 1];
  char lGWIPString[MAX_BUF_SIZE + 1];
  char *lTmpPtr = NULL;


printf("SetMACStatic(0) : arp -d %s & netsh interface ip add neighbors \"%s\" %s %s\n\n", pIP, pIfcAlias, pIP, pMAC);


  /*
   * Set IP static
   */
  if (pIfcAlias != NULL && pIP != NULL && pMAC != NULL)
  {
    ZeroMemory(lTemp, sizeof(lTemp));
    ZeroMemory(lGWIPString, sizeof(lGWIPString));


    /*
     * The arp tool needs '-' as octet separator
     */
    if (strchr(pMAC, ':'))
      for (lTmpPtr = pMAC; lTmpPtr[0] != NULL; lTmpPtr++)
        if (lTmpPtr[0] == ':')
          lTmpPtr[0] = '-';


printf("SetMACStatic(1) : arp -d %s & netsh interface ip add neighbors \"%s\" %s %s\n\n", pIP, pIfcAlias, pIP, pMAC);

    _snprintf(lTemp, sizeof(lTemp) - 1, "arp -d %s & netsh interface ip add neighbors \"%s\" %s %s", pIP, pIfcAlias, pIP, pMAC);
    ExecCommand(lTemp);
  } // if (pIfcAlia...
}




/*
 *
 *
 */
void RemoveMAC(char *pIfcAlias, char *pIPAddr)
{
  char lTemp[MAX_BUF_SIZE + 1];

  if (pIfcAlias != NULL && pIPAddr != NULL)
  {
    ZeroMemory(lTemp, sizeof(lTemp)-1);
    _snprintf(lTemp, sizeof(lTemp) - 1, "netsh interface ip delete neighbors \"%s\" %s", pIfcAlias, pIPAddr);
   	ExecCommand(lTemp);

    ZeroMemory(lTemp, sizeof(lTemp)-1);
    _snprintf(lTemp, sizeof(lTemp) - 1, "arp -d %s", pIPAddr);
    ExecCommand(lTemp);
  } // if (pIfcAlias != NU...
}




/*
 *
 *
 */
int GetIFCName(char *pIFCName, char *pRealIFCName, int pBufLen)
{
  int lRetVal = 0;
  pcap_if_t *lAllDevs = NULL;
  pcap_if_t *lDevice = NULL;
  char lTemp[PCAP_ERRBUF_SIZE];
  char lAdapter[MAX_BUF_SIZE + 1];
  int lCounter = 0;
  int lIFCnum = 0;
 

  /*
   * Open device list.
   */ 
  if (pcap_findalldevs_ex(PCAP_SRC_IF_STRING, NULL, &lAllDevs, lTemp) == -1)
  {
    LogMsg(DBG_ERROR, "getIFCName() : Error in pcap_findalldevs_ex() : %s", lTemp);
    lRetVal = 1;
    goto END;
  } // if (pcap_fi...

 
  ZeroMemory(lAdapter, sizeof(lAdapter));
  lCounter = 0;
 
  for(lCounter = 0, lDevice = lAllDevs; lDevice; lDevice = lDevice->next, lCounter++)
  {
    if (StrStrI(lDevice->name, pIFCName))
    {
      strncpy(pRealIFCName, lDevice->name, pBufLen);
      break;
    } // if (StrStr..
  } // for(lCount...
 
 
END:
 
  /*
   * Release all allocated resources.
   */ 
  if (lAllDevs)
    pcap_freealldevs(lAllDevs);
 
  return(lRetVal);
}

