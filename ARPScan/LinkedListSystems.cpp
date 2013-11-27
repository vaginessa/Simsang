#include <windows.h>
#include <stdio.h>
#include <string.h>
#include <time.h>

#include "LinkedListSystems.h"


extern CRITICAL_SECTION gCSSystemsLL;

/*
*
*
*/
PSYSTEMNODE InitSystemList()
{
  PSYSTEMNODE lFirsHostNode = NULL;

  EnterCriticalSection(&gCSSystemsLL);

  if ((lFirsHostNode = (PSYSTEMNODE) HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, sizeof(SYSTEMNODE))) != NULL)
  {
    lFirsHostNode->first = 1;
    lFirsHostNode->next = NULL;
    lFirsHostNode->prev = NULL;
  } // if (tmp = ma...

  LeaveCriticalSection(&gCSSystemsLL);

  return(lFirsHostNode);
}



/*
*
*
*/
void AddToList(PPSYSTEMNODE pHostNodes, unsigned char pSystemIP[BIN_IP_LEN], unsigned char pSystemMAC[BIN_MAC_LEN])
{
  PSYSTEMNODE lTmpNode = NULL;

  EnterCriticalSection(&gCSSystemsLL);

  //printf("AddToList(0) : %d.%d.%d.%d   %02x-%02x-%02x-%02x-%02x-%02x\n", pSystemIP[0], pSystemIP[1], pSystemIP[2], pSystemIP[3]
  //  , pSystemMAC[0], pSystemMAC[1], pSystemMAC[2], pSystemMAC[3], pSystemMAC[4], pSystemMAC[5]);

  if ((lTmpNode = (PSYSTEMNODE) HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, sizeof(SYSTEMNODE))) != NULL)
  {
    CopyMemory(lTmpNode->sData.SystemIP, pSystemIP, BIN_IP_LEN);
    CopyMemory(lTmpNode->sData.SystemMAC, pSystemMAC, BIN_MAC_LEN);

    lTmpNode->prev = NULL;
    lTmpNode->first = 0;
    lTmpNode->next = *pHostNodes;
    ((PSYSTEMNODE) *pHostNodes)->prev = lTmpNode;
    *pHostNodes = lTmpNode;
  } // if (pSysMAC != NUL...

  LeaveCriticalSection(&gCSSystemsLL);
  //printf("AddToList(2) : \n");
}



/*
*
*
*/
PSYSTEMNODE GetNodeByMAC(PSYSTEMNODE pSysNodes, unsigned char *pSystemMAC)
{
  PSYSTEMNODE lRetVal = NULL;
  int lCount = 0;
  char lTemp[MAX_BUF_SIZE + 1];

  EnterCriticalSection(&gCSSystemsLL);


  if (pSysNodes != NULL && pSystemMAC != NULL)
  {
    while (pSysNodes != NULL)
    {
      if (pSysNodes->first == 0)
      {
        _snprintf(lTemp, sizeof(lTemp) - 1, "%02x-%02x-%02x-%02x-%02x-%02x",  pSysNodes->sData.SystemMAC[0], pSysNodes->sData.SystemMAC[1], pSysNodes->sData.SystemMAC[2], pSysNodes->sData.SystemMAC[3], pSysNodes->sData.SystemMAC[4], pSysNodes->sData.SystemMAC[5]);

        if (memcmp((char *)pSysNodes->sData.SystemMAC, (char *) pSystemMAC, BIN_MAC_LEN /* sizeof(lTmpSys->sData.SystemMAC)*/ ) == 0)
        {
          lRetVal = pSysNodes;
          break;
        } // if (strncmp((char...
      } // if (pSysNodes->...
      pSysNodes = pSysNodes->next;
    } // while(pSysNodes != N...
  } // if (pSysNodes ...

  LeaveCriticalSection(&gCSSystemsLL);

  return(lRetVal);
}



/*
*
*
*/
void EnumListNodes(PSYSTEMNODE pSysNodes)
{
  char lTemp[MAX_BUF_SIZE + 1];

  if (pSysNodes != NULL)
  {
    while(pSysNodes != NULL)
    {
      _snprintf(lTemp, sizeof(lTemp) - 1, "%02x-%02x-%02x-%02x-%02x-%02x",  pSysNodes->sData.SystemMAC[0], pSysNodes->sData.SystemMAC[1], pSysNodes->sData.SystemMAC[2], pSysNodes->sData.SystemMAC[3], pSysNodes->sData.SystemMAC[4], pSysNodes->sData.SystemMAC[5]);
      pSysNodes = pSysNodes->next;
    } // while(pSysNodes != N...
  } // if (pSysNodes ...
}




/*
*
*
*/
int CountNodes(PSYSTEMNODE pSysNodes)
{   
  int lRetVal = 0;

  while(pSysNodes != NULL)
  {
    pSysNodes = pSysNodes->next;
    lRetVal++;
  } // while(pSysNodes !...

  return(lRetVal);
}