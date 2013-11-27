#ifndef __ACCOUNTINGDESTINATION__
#define __ACCOUNTINGDESTINATION__

#include "NetPackets.h"

/*
 * Function forward declarations
 *
 */
void ProcessDestinationIPTraffic(PSCANPARAMS lScanParams, char *pProtocol, IPADDRESS pSrcIP, IPADDRESS pDstIP, unsigned int pSrcPort, unsigned int pDstPort, int pDataLen);
DWORD WINAPI DestinationVisualisationThread(LPVOID lpParam);
void DestinationStartAccounting(PSCANPARAMS pScanParams);
void DestinationAccountingCallback(unsigned char *pScanParams, struct pcap_pkthdr *pPcapHdr, unsigned char *pPktData);
//int IPAddrBelongsToLocalNet(unsigned long pLocalIP, unsigned long pNetmask, unsigned long pTestAddr);
//DWORD WINAPI IPLookup(void *pParams);

#endif