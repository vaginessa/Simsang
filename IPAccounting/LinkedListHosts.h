#ifndef __LINKEDLISTHOSTS__
#define __LINKEDLISTHOSTS__



#define MAX_NODE_COUNT 1024
#define BIN_IP_LEN 4
#define MAX_IP_LEN 17
#define MAX_BUF_SIZE 1024



/*
 * Type declarations.
 *
 */
typedef struct HOSTDATA 
{
  unsigned long PacketCounter;
  unsigned long DataVolume;  
  unsigned char LastUpdate[128];
  unsigned long IPaddress;
  unsigned char IPaddressStr[20];
  unsigned char Hostname[1026];
} HOSTDATA, *PHOSTDATA;


typedef struct HOSTNODE 
{
  HOSTDATA sData;

  int first;
  struct HOSTNODE *prev;
  struct HOSTNODE *next;
} HOSTNODE, *PHOSTNODE, **PPHOSTNODE;



/*
 * Function forward declarations.
 *
 */
PHOSTNODE InitHostsList();
PHOSTNODE AddRecordToList(PPHOSTNODE pHostNodes, unsigned long pIPAddress);
PHOSTNODE GetNodeByIP(PHOSTNODE pSysNodes, unsigned long pIPAddress);
void EnumListNodes(PHOSTNODE pSysNodes);


#endif