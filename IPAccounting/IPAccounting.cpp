#define HAVE_REMOTE


#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <pcap.h>
#include <Shlwapi.h>
#include <stdarg.h>
#include <locale.h>

#include "IPAccounting.h"
#include "AccountingProtocol.h"
#include "AccountingDestination.h"
#include "AccountingSource.h"
#include "LinkedListHosts.h"
#include "GeneralFunctions.h"


#pragma comment(lib, "wpcap.lib")
#pragma comment(lib, "Ws2_32.lib")
#pragma comment(lib, "Shlwapi.lib")
#pragma comment(lib, "IPHLPAPI.lib")



/*
 * Global variables
 */
int gDEBUGLEVEL = DEBUG_LEVEL;
SCANPARAMS gScanParams;
DWORD gVisualisationThreadID = 0;
HANDLE gVisualisationThreadHandle = INVALID_HANDLE_VALUE;
SERVICEDATA gServices[MAX_SERVICES];
PHOSTNODE gHosts; 
int gNumServices;
char **gARGV = NULL;
int gXML = 0;
int gFILTER = 0;


/*
 *
 * Main:  Program entry point
 *
 */
int main(int argc, char **argv)
{ 
  int lRetVal = 0;
  int lCounter = 0;
  char *lTempPtr = NULL;
  DWORD (WINAPI *ThreadFunctionPtr)(LPVOID) = NULL;
  void (*VisualisationFunctionPtr)(PSCANPARAMS);


  /*
   * Initialisation
   */
  LogMsg(DBG_LOW, "main() : Starting %s", argv[0]);
  ZeroMemory(&gScanParams, sizeof(gScanParams));
  gARGV = argv;
  gHosts = InitHostsList();

//  setlocale(LC_NUMERIC, "en_US");



  /*
   * Basic parameter check. Sub optimal but ja ...
   */
  for (lCounter = 0; lCounter < argc; lCounter++)
  {
    // Let's generate XML
    if (StrCmpI(argv[lCounter], "-x") == 0)
    {
      gXML = 1;

    // Let's filter local requests out (MITM only)
    }
    else if (StrCmpI(argv[lCounter], "-f") == 0)
    {
      gFILTER = 1;
    // Accounting is based on destination addresses
    }
    else if (StrCmpI(argv[lCounter], "-d") == 0)
    {
      ThreadFunctionPtr = &DestinationVisualisationThread;
      VisualisationFunctionPtr = &DestinationStartAccounting;

    // Accounting is based on source addresses
    }
    else if (StrCmpI(argv[lCounter], "-s") == 0)
    {
      ThreadFunctionPtr = &SourceVisualisationThread;
      VisualisationFunctionPtr = &SourceStartAccounting;

    // Accounting is based on layer 3 protocols (services)
    }
    else if (StrCmpI(argv[lCounter], "-p") == 0)
    {
      ThreadFunctionPtr = &ProtocolVisualisationThread;
      VisualisationFunctionPtr = &StartAccounting;
    }

    
  }
  



  /*
   * List all interfaces
   */
  if (argc == 2 && ! strcmp(argv[1], "-l")) 
  {
    ListIFCDetails();
    goto END;


  /*
   * General sniffer mode
   * -g IFC-Name "PCAP_Pattern"
   */
  }
  else if (argc >= 3 && ! strcmp(argv[1], "-i")) 
  {
 	  adminCheck(argv[0]);

    strncpy((char *) gScanParams.IFCName, argv[2], sizeof(gScanParams.IFCName));
    GetIFCName(argv[2], (char *) gScanParams.IFCName, sizeof(gScanParams.IFCName) - 1);
    GetIFCDetails(argv[2], &gScanParams);

    MAC2String(gScanParams.LocalMAC, gScanParams.LocalMACStr, MAX_MAC_LEN);
    IP2String(gScanParams.LocalIP, gScanParams.LocalIPStr, MAX_IP_LEN);


    /*
     * Create output records per protocol (p)
     *
     * Call: IPAccounting.exe -i        {IFC-String}   (-p|-s|-d)   [-x]       [-f]
     *       argv[0]          argv[1]   argv[2]        argv[3]      argv[4]    argv[5]
     */
    if (ThreadFunctionPtr != NULL && VisualisationFunctionPtr != NULL)
    {
      if ((gVisualisationThreadHandle = CreateThread(NULL, 0, ThreadFunctionPtr, 0, 0, &gVisualisationThreadID)) == NULL)
  	   {
        LogMsg(DBG_ERROR, "main() : Can't start ARP Replies thread : %d", GetLastError());
        goto END;
   	  } // if ((lPOISO...
      Sleep(1000);
      VisualisationFunctionPtr(&gScanParams);
    } // if (ThreadF...
  }
  else
  {
    system("cls");
    printf("\nIPAccounting Version %s\n", IPACCOUNTING_VERSION);
    printf("------------------------\n\n");
    printf("Web\t http://www.buglist.io/\n");
    printf("Mail\t ruben.unteregger@gmail.com\n\n\n");
	   printf("List all interfaces               :  %s -l\n", argv[0]);
				printf("Start accounting                  :  %s -i IFC-Name (-p|-s|-d)\n", argv[0]); 
    printf("\n");
    printf("                                                      -p   by protocol/service\n");
    printf("                                                      -d   by destination IP\n");
    printf("                                                      -s   by source IP\n");
				printf("\n\n\n\n");
				printf("Example : %s -i 0F716AAF-D4A7-ACBA-1234-EA45A939F624 -d \n", argv[0]);
				printf("\n\n\n\n");
    printf("WinPcap version\n---------------\n\n");
   	printf("%s\n\n", pcap_lib_version());
  } // if (argc == 2 &...




END:

  LogMsg(DBG_LOW, "main() : Stopping %s", argv[0]);

  return(lRetVal);
}



void MAC2String(unsigned char pMAC[BIN_MAC_LEN], unsigned char *pOutput, int pOutputLen)
{
  if (pOutput && pOutputLen > 0)
    _snprintf((char *) pOutput, pOutputLen, "%02X-%02X-%02X-%02X-%02X-%02X", pMAC[0], pMAC[1], pMAC[2], pMAC[3], pMAC[4], pMAC[5]);  
}

void IP2String(unsigned char pIP[BIN_IP_LEN], unsigned char *pOutput, int pOutputLen)
{
  if (pOutput && pOutputLen > 0)
    _snprintf((char *) pOutput, pOutputLen, "%d.%d.%d.%d", pIP[0], pIP[1], pIP[2], pIP[3]);
}

