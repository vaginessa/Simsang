#define HAVE_REMOTE

#include <stdio.h>
#include <stdlib.h>
#include <pcap.h>

#include "DHCPoison.h"
#include "HelperFunctions.h"
#include "Network.h"


#pragma comment(lib, "wpcap.lib")
#pragma comment(lib, "IPHLPAPI.lib")
#pragma comment(lib, "Shlwapi.lib")
#pragma comment(lib, "Ws2_32.lib")


/*
 * Global variables definition
 *
 */
int gDEBUGLEVEL = DBG_ERROR;



/*
 * Program entry point
 *
 * - Only poison systems that ask for a particular IP in their DHCP request
 */
int main(int argc, char *argv[])
{
  int lRetVal = 0;
  SCANPARAMS gScanParams;


  ZeroMemory(&gScanParams, sizeof(gScanParams));

  /*
   * List all interfaces
   */
  if (argc == 2 && ! strcmp(argv[1], "-l")) 
  {
    ListIFCDetails();
    goto END;
  }
  else if (argc == 5 && !strcmp(argv[1], "-x"))
  {
    adminCheck(argv[0]);

    RemoveMAC((char *) gScanParams.IFCName, "*");
    Sleep(500);
    RemoveMAC((char *) gScanParams.IFCName, "*");
    LogMsg(2, "main() : %s\n", gScanParams.IFCName);


    /*
     * Initialisation. Parse parameters (Ifc, start IP, stop IP) and
     * pack them in the scan configuration struct.
     */
    strncpy((char *) gScanParams.IFCName, argv[3], sizeof(gScanParams.IFCName));
    GetIFCName(argv[2], (char *) gScanParams.IFCName, sizeof(gScanParams.IFCName) - 1);
    GetIFCDetails(argv[2], &gScanParams);

    MAC2String(gScanParams.LocalMAC, gScanParams.LocalMACStr, MAX_MAC_LEN);
    IP2String(gScanParams.LocalIP, gScanParams.LocalIPStr, MAX_IP_LEN);

    MAC2String(gScanParams.GWMAC, gScanParams.GWMACStr, MAX_MAC_LEN);
    IP2String(gScanParams.GWIP, gScanParams.GWIPStr, MAX_IP_LEN);


   	// Set GW IP static.
   	SetMACStatic((char *) gScanParams.IFCAlias, (char *) gScanParams.GWIPStr, (char *) gScanParams.GWMACStr);

    if (gDEBUGLEVEL > DBG_INFO)
      PrintConfig(gScanParams);

    exit(0);
  }
  else
    printUsage(argv[0]);


END:

  return(lRetVal);
}





/*
 *
 *
 */
void PrintConfig(SCANPARAMS pScanParams)
{
  printf("Local IP :\t%d.%d.%d.%d\n", pScanParams.LocalIP[0], pScanParams.LocalIP[1], pScanParams.LocalIP[2], pScanParams.LocalIP[3]);
  printf("Local MAC :\t%02X-%02X-%02X-%02X-%02X-%02X\n", pScanParams.LocalMAC[0], pScanParams.LocalMAC[1], pScanParams.LocalMAC[2],
                                             pScanParams.LocalMAC[3], pScanParams.LocalMAC[4], pScanParams.LocalMAC[5]); 
  printf("GW MAC :\t%02X-%02X-%02X-%02X-%02X-%02X\n", pScanParams.GWMAC[0], pScanParams.GWMAC[1], pScanParams.GWMAC[2],
                                          pScanParams.GWMAC[3], pScanParams.GWMAC[4], pScanParams.GWMAC[5]);
  printf("GW IP :\t\t%d.%d.%d.%d\n", pScanParams.GWIP[0], pScanParams.GWIP[1], pScanParams.GWIP[2], pScanParams.GWIP[3]);
  printf("Start IP :\t%d.%d.%d.%d\n", pScanParams.StartIP[0], pScanParams.StartIP[1], pScanParams.StartIP[2], pScanParams.StartIP[3]);
  printf("Stop IP :\t%d.%d.%d.%d\n", pScanParams.StopIP[0], pScanParams.StopIP[1], pScanParams.StopIP[2], pScanParams.StopIP[3]);
}


