#ifndef __DHCPOISON__
#define __DHCPOISON__

#define DHCPOISON_VERSION "0.1"
#define MAX_BUF_SIZE 1024

#define DEBUG_LEVEL 1

#define DBG_OFF    0
#define DBG_INFO   1
#define DBG_LOW    2
#define DBG_MEDIUM 3
#define DBG_HIGH   4
#define DBG_ALERT  5
#define DBG_ERROR  5

#define DBG_LOGFILE "c:\\debug.log"


#define ETHERTYPE_ARP 0x0806
#define ETHERTYPE_IP 0x0800

#define BIN_MAC_LEN 6
#define MAX_MAC_LEN 18
#define BIN_IP_LEN 4
#define MAX_IP_LEN 18

#define OK 0
#define NOK 1



/*
 * Data type definitions
 *
 */
typedef struct SCANPARAMS
{
  unsigned char IFCName[MAX_BUF_SIZE + 1];
  unsigned char IFCAlias[MAX_BUF_SIZE + 1];
  unsigned char IFCDescr[MAX_BUF_SIZE + 1];
  int Index;
  unsigned char GWIP[BIN_IP_LEN];
  unsigned char GWIPStr[MAX_IP_LEN];
  unsigned char GWMAC[BIN_MAC_LEN];
  unsigned char GWMACStr[MAX_MAC_LEN];
  unsigned char StartIP[BIN_IP_LEN];
  unsigned long StartIPNum;
  unsigned char StopIP[BIN_IP_LEN];
  unsigned long StopIPNum;
  unsigned char LocalIP[BIN_IP_LEN];
  unsigned char LocalIPStr[MAX_IP_LEN];
  unsigned char LocalMAC[BIN_MAC_LEN];
  unsigned char LocalMACStr[MAX_MAC_LEN];
  unsigned char *PCAPPattern;
  HANDLE PipeHandle;
  void *IfcReadHandle;  // HACK! because of header hell :/
  void *IfcWriteHandle; // HACK! because of header hell :/
} SCANPARAMS, *PSCANPARAMS;




/*
 * Function forward declarations
 *
 */
void PrintConfig(SCANPARAMS pScanParams);



#endif