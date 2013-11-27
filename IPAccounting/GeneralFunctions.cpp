#define HAVE_REMOTE

#include <stdlib.h>
#include <stdio.h>
#include <stdarg.h>
#include <time.h>
#include <pcap.h>

#include <windows.h>
#include <Shlwapi.h>
#include <iphlpapi.h>

#include "IPAccounting.h"



/*
 *
 *
 */
void LogMsg(int pPriority, char *pMsg, ...)
{
  HANDLE lFH = INVALID_HANDLE_VALUE;
  OVERLAPPED lOverl = { 0 };
  char lDateStamp[MAX_BUF_SIZE + 1];
  char lTimeStamp[MAX_BUF_SIZE + 1];
  char lTime[MAX_BUF_SIZE + 1];
  char lTemp[MAX_BUF_SIZE + 1];
  char lLogMsg[MAX_BUF_SIZE + 1];
  DWORD lBytedWritten = 0;
  va_list lArgs;



  if (pPriority >= DEBUG_LEVEL && DEBUG_LEVEL != DBG_OFF)
  { 
    if ((lFH = CreateFile(DBG_LOGFILE, GENERIC_READ|GENERIC_WRITE, 0, 0, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0)) != INVALID_HANDLE_VALUE)
    {
      ZeroMemory(&lOverl, sizeof(lOverl));

      if (LockFileEx(lFH, LOCKFILE_EXCLUSIVE_LOCK, 0, 0, 0, &lOverl) == TRUE)
      {
        ZeroMemory(lTime, sizeof(lTime));
        ZeroMemory(lTimeStamp, sizeof(lTimeStamp));
        ZeroMemory(lDateStamp, sizeof(lDateStamp));


        /*
         * Create timestamp
         */
        _strtime(lTimeStamp);
        _strdate(lDateStamp);
        snprintf(lTime, sizeof(lTime) - 1, "%s %s", lDateStamp, lTimeStamp);

        /*
         * Create log message
         */
        ZeroMemory(lTemp, sizeof(lTemp));
        ZeroMemory(lLogMsg, sizeof(lLogMsg));
        va_start (lArgs, pMsg);
        vsprintf(lTemp, pMsg, lArgs);
        va_end(lArgs);
		snprintf(lLogMsg, sizeof(lLogMsg) - 1, "%s : %s\n", lTime, lTemp);

        /*
         * Write message to the logfile.
         */
        SetFilePointer(lFH, 0, NULL, FILE_END);
        WriteFile(lFH, lLogMsg, strnlen(lLogMsg, sizeof(lLogMsg) - 1), &lBytedWritten, NULL);
        UnlockFileEx(lFH, 0, 0, 0, &lOverl);
	  } // if (LockFileEx(lF...
      CloseHandle(lFH);
    } // if ((lFH = CreateF...
  } // if (pPriori...
}




/*
 *
 *
 */
int UserIsAdmin()
{
  BOOL lRetVal = FALSE;
  SID_IDENTIFIER_AUTHORITY lNtAuthority = SECURITY_NT_AUTHORITY;
  PSID lAdmGroup = NULL;


  if(AllocateAndInitializeSid(&lNtAuthority, 2, SECURITY_BUILTIN_DOMAIN_RID,
    DOMAIN_ALIAS_RID_ADMINS, 0, 0, 0, 0, 0, 0, &lAdmGroup)) 
  {
    if (! CheckTokenMembership(NULL, lAdmGroup, &lRetVal))
         lRetVal = FALSE;
    FreeSid(lAdmGroup); 
  }

  return(lRetVal);
}






/*
 *
 *
 */
void adminCheck(char *pProgName)
{
  if(! UserIsAdmin())
  {
    system("cls");
    printf("\nIPAccounting Version %s\n", IPACCOUNTING_VERSION);
   	printf("---------------------------------------\n\n");
    printf("Web\t http://www.megapanzer.com/\n");
    printf("Mail\t megapanzer@gmail.com\n\n\n");
    printf("You need Administrator permissions to run %s successfully!\n\n", pProgName);
   	exit(1);
  }
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
   * Open device lis.t
   *
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
   *
   */
 
  if (lAllDevs)
    pcap_freealldevs(lAllDevs);
 


  return(lRetVal);
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
    LogMsg(DBG_ERROR, "GetIFCDetails() : Error allocating memory needed to call GetAdaptersinfo");
    lRetVal = 1;
    goto END;
  } // if ((lAdapterInfo...


  if (GetAdaptersInfo(lAdapterInfoPtr, &lOutBufLen) == ERROR_BUFFER_OVERFLOW) 
  {
    HeapFree(GetProcessHeap(), 0, lAdapterInfoPtr);
    if ((lAdapterInfoPtr = (IP_ADAPTER_INFO *) HeapAlloc(GetProcessHeap(), 0, lOutBufLen)) == NULL)
    {
      LogMsg(DBG_ERROR, "GetIFCDetails() : Error allocating memory needed to call GetAdaptersinfo");
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


        // Get Netmask
        pScanParams->Netmask = inet_addr(lAdapter->IpAddressList.IpMask.String);


        // Get gateway MAC address
//        CopyMemory(pScanParams->GWIP, &lGWIPAddr, 4); // ????
//        ZeroMemory(&lGWMACAddr, sizeof(lGWMACAddr));
//        SendARP(lGWIPAddr, 0, lGWMACAddr, &lGWMACAddrLen);
//        CopyMemory(pScanParams->GWMAC, lGWMACAddr, 6);



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
	  snprintf(pAliasBuf, pBufLen-1, "%ws", lIfcRow.Alias);
    } //  if (GetIfEntry2...
  } // if (pAliasBuf != N...

  return(lRetVal);
}




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
        else 
        {
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

  return(lRetVal);
}



/*
 *
 *
 */
int IPAddrBelongsToLocalNet(unsigned long pLocalIP, unsigned long pNetmask, unsigned long pTestAddr)
{
	unsigned long lLocalNet;
	unsigned long lOtherNet;

	lLocalNet = pLocalIP & pNetmask;
	lOtherNet = pTestAddr & pNetmask;

	return(lLocalNet==lOtherNet ? 0 : 1);
}



/*
 * 
 *
 */
DWORD WINAPI IPLookup(void *pParams)
{
  PIPREVERSELOOKUP lParams = (PIPREVERSELOOKUP) pParams;
  struct sockaddr_in saGNI;
  char servInfo[NI_MAXSERV];

  strcpy((char *) lParams->Hostname, "unknown");
  saGNI.sin_family = AF_INET;
  saGNI.sin_addr.s_addr = inet_addr((char *) lParams->IPAddressStr);
  saGNI.sin_port = htons(23456); 

  getnameinfo((struct sockaddr *) &saGNI, sizeof (struct sockaddr), (char *) lParams->Hostname,
               NI_MAXHOST, servInfo, NI_MAXSERV, NI_NUMERICSERV);

  return(0);
}