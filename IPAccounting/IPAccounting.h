#ifndef __IPACCOUNTING__
#define __IPACCOUNTING__

#define XML 1

#define IPACCOUNTING_VERSION "0.3"

#define DEBUG_LEVEL 0

#define DBG_OFF    0
#define DBG_INFO   1
#define DBG_LOW    2
#define DBG_MEDIUM 3
#define DBG_HIGH   4
#define DBG_ALERT  5
#define DBG_ERROR  5

#define DBG_LOGFILE "debug.log"

#define MAX_BUF_SIZE 1024
#define snprintf _snprintf
#define PCAP_READTIMEOUT 1

#define BIN_IP_LEN 4
#define BIN_MAC_LEN 6
#define MAX_IP_LEN 16
#define MAX_MAC_LEN 18

#define ICMP_PSEUDO_SERVICE "ICMP"
#define ICMP_PSEUDO_PORT -2

#define OK 0
#define NOK 1

typedef struct SCANPARAMS
{
  unsigned char IFCName[MAX_BUF_SIZE + 1];
  unsigned char IFCAlias[MAX_BUF_SIZE + 1];
  unsigned char IFCDescr[MAX_BUF_SIZE + 1];
  char IFCString[MAX_BUF_SIZE + 1];
  int Index;
  unsigned long Netmask;
  unsigned char StartIP[BIN_IP_LEN];
  unsigned long StartIPNum;
  unsigned char StopIP[BIN_IP_LEN];
  unsigned long StopIPNum;

  unsigned char GWIP[BIN_IP_LEN];
  unsigned char GWIPStr[MAX_IP_LEN];
  unsigned char GWMAC[BIN_MAC_LEN];
  unsigned char GWMACStr[MAX_MAC_LEN];

  unsigned char LocalIP[BIN_IP_LEN];
  unsigned char LocalIPStr[MAX_IP_LEN];
  unsigned char LocalMAC[BIN_MAC_LEN];
  unsigned char LocalMACStr[MAX_MAC_LEN];

  unsigned char VictimIP[BIN_IP_LEN];
  unsigned char VictimIPStr[MAX_IP_LEN];
  unsigned char VictimMAC[BIN_MAC_LEN];
  unsigned char VictimMACStr[MAX_MAC_LEN];
  
  void *IfcReadHandle;
  void *IfcWriteHandle;
} SCANPARAMS, *PSCANPARAMS;

typedef struct IPREVERSELOOKUP
{
  unsigned char *Hostname;
  unsigned long IPAddress;
  unsigned char *IPAddressStr;
} IPREVERSELOOKUP, *PIPREVERSELOOKUP;




/*
 * Function forward declarations
 *
 */
void MAC2String(unsigned char pMAC[BIN_MAC_LEN], unsigned char *pOutput, int pOutputLen);
void IP2String(unsigned char pIP[BIN_IP_LEN], unsigned char *pOutput, int pOutputLen);

#endif