#ifndef __ARPSCAN__
#define __ARPSCAN__

#define MAX_BUF_SIZE 1024
#define SLEEP_BETWEEN_ARPS 5

#define OK 0
#define NOK 1

#define BIN_MAC_LEN 6
#define BIN_IP_LEN 4
#define MAX_MAC_LEN 18
#define MAX_IP_LEN 16

#define ARP_REQUEST 1   
#define ARP_REPLY 2  

#define ETHERTYPE_ARP 0x0806
#define ETHERTYPE_IP 0x0800

#define IP_PROTO_UDP 17
#define IP_PROTO_TCP 6
#define IP_PROTO_ICMP 1


/*
 * Our data types
 *
 */
typedef struct ethern_hdr
{
  unsigned char ether_dhost[BIN_MAC_LEN];  
  unsigned char ether_shost[BIN_MAC_LEN];  
  unsigned short ether_type;
} ETHDR, *PETHDR;

typedef struct arp_hdr 
{ 
  unsigned short htype;    // Hardware Type         
  unsigned short ptype;    // Protocol Type            
  unsigned char hlen;        // Hardware Address Length  
  unsigned char plen;        // Protocol Address Length  
  unsigned short oper;     // Operation Code           
  unsigned char sha[BIN_MAC_LEN];      // Sender hardware address  
  unsigned char spa[BIN_IP_LEN];      // Sender IP address        
  unsigned char tha[BIN_MAC_LEN];      // Target hardware address  
  unsigned char tpa[BIN_IP_LEN];      // Target IP address        
} ARPHDR, *PARPHDR; 


typedef struct pARPPacket
{
  int lReqType;
  unsigned char Eth_SrcMAC[BIN_MAC_LEN];
  unsigned char Eth_DstMAC[BIN_MAC_LEN];
  unsigned char ARP_LocalMAC[BIN_MAC_LEN];
  unsigned char ARP_LocalIP[BIN_IP_LEN];
  unsigned char ARP_Dst_MAC[BIN_MAC_LEN];
  unsigned char ARP_DstIP[BIN_IP_LEN];
} ARPPacket, *PARPPacket;



typedef struct SCANPARAMS
{
  unsigned char IFCName[MAX_BUF_SIZE + 1];
  unsigned char IFCAlias[MAX_BUF_SIZE + 1];
  unsigned char IFCDescr[MAX_BUF_SIZE + 1];
  char IFCString[MAX_BUF_SIZE + 1];
  int Index;
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
  
  LPVOID IfcWriteHandle;
} SCANPARAMS, *PSCANPARAMS;


/*
 * Function forward declaration
 *
 */
void LogMsg(char *pMsg, ...);
int GetIFCDetails(char *pIFCName, PSCANPARAMS pScanParams);
void IP2String(unsigned char pIP[BIN_IP_LEN], unsigned char *pOutput, int pOutputLen);
void MAC2String(unsigned char pMAC[BIN_MAC_LEN], unsigned char *pOutput, int pOutputLen);
int SendARPPacket(void *pIFCHandle, PARPPacket pARPPacket);
int SendARPWhoHas(PSCANPARAMS pScanParams, unsigned long lIPAddress);
DWORD WINAPI CaptureARPReplies(LPVOID pScanParams);


#endif
