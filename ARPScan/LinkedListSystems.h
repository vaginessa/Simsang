
#ifndef __LINKEDLIST__
#define __LINKEDLIST__

#include "ARPScan.h"

#define  MAX_NODE_COUNT 1024

/*
 * Type declarations.
 *
 */

typedef struct SYSTEMDATA 
{
  unsigned char SystemIP[BIN_IP_LEN];
  unsigned char SystemMAC[BIN_MAC_LEN];
} SYSTEMDATA;


typedef struct SYSTEMNODE 
{
  SYSTEMDATA sData;

  int first;
  struct SYSTEMNODE *prev;
  struct SYSTEMNODE *next;
} SYSTEMNODE, *PSYSTEMNODE, **PPSYSTEMNODE;




/*
 * Function forward declarations.
 */
PSYSTEMNODE InitSystemList();
void AddToList(PPSYSTEMNODE pHostNodes, unsigned char pSystemIP[BIN_IP_LEN], unsigned char pSystemMAC[BIN_MAC_LEN]);
PSYSTEMNODE GetNodeByMAC(PSYSTEMNODE pSysNodes, unsigned char pIPBin[BIN_IP_LEN]);
void EnumListNodes(PSYSTEMNODE pSysNodes);

#endif