#ifndef __ACCOUNTINGSOURCE__
#define __ACCOUNTINGSOURCE__

#include "NetPackets.h"


/*
 * Function forward declarations
 *
 */
void ProcessSourceIPTraffic(PSCANPARAMS lScanParams, char *pProtocol, IPADDRESS pSrcIP, IPADDRESS pDstIP, unsigned int pSrcPort, unsigned int pDstPort, int pDataLen);
DWORD WINAPI SourceVisualisationThread(LPVOID lpParam);
void SourceStartAccounting(PSCANPARAMS pScanParams);
void SourceAccountingCallback(unsigned char *pScanParams, struct pcap_pkthdr *pPcapHdr, unsigned char *pPktData);
//int IPAddrBelongsToLocalNet(unsigned long pLocalIP, unsigned long pNetmask, unsigned long pTestAddr);
//DWORD WINAPI IPLookup(void *pParams);

#endif