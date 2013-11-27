
#ifndef __LINKEDLIST__
#define __LINKEDLIST__


#define SERVICE_NAME_BUFFER_LENGTH 128
#define MAX_SERVICES_COUNT 1024
#define MAX_SERVICES 512



/*
 * Data type definitions
 */
typedef struct SERVICEDATA 
{
  signed int PortNumberLow;
  signed int PortNumberUp;
  unsigned char ServiceName[SERVICE_NAME_BUFFER_LENGTH + 1];
  unsigned long PacketCounter;
  unsigned long DataVolume;
  unsigned char LastUpdate[SERVICE_NAME_BUFFER_LENGTH + 1];
} SERVICEDATA, *PSERVICEDATA;


/*
 * Function forward declaration.
 */
void AccountingCallback(unsigned char *pScanParams, struct pcap_pkthdr *pPcapHdr, unsigned char *pPktData);
void ProcessIPTraffic(char *pProtocol, unsigned int pSrcPort, unsigned int pDstPort, int pDataLen);
PSERVICEDATA GetServiceEntry(int pPortNumber);
int MapServicePort(int pSrcPort, int pDstPort);

void StartAccounting(PSCANPARAMS pScanParams);
DWORD WINAPI ProtocolVisualisationThread(LPVOID lpParam);
int ParseServicesConfigFile(char *pTargetsFile);

#endif