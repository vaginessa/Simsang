#include <windows.h>
#include <stdio.h>
#include <string.h>
#include <time.h>

#include "LinkedListHosts.h"




/*
 *
 *
 */
PHOSTNODE InitHostsList()
{
  PHOSTNODE lFirsHostNode = NULL;

  if ((lFirsHostNode = (PHOSTNODE) HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, sizeof(HOSTNODE))) != NULL)
  {
    lFirsHostNode->first = 1;
    lFirsHostNode->next = NULL;
   	lFirsHostNode->prev = NULL;
  } // if (tmp = ma...

  return(lFirsHostNode);
}
 


/*
 *
 *
 */
PHOSTNODE AddRecordToList(PPHOSTNODE pHostNodes, unsigned long pIPAddress)
{
  PHOSTNODE lTmpNode = NULL;
  unsigned char *lTmp = (unsigned char *) &pIPAddress;

  if ((lTmpNode = (PHOSTNODE) HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, sizeof(HOSTNODE))) != NULL)
  {
//printf("AddHostToList(1) :  %lu/%d.%d.%d.%d\n", pIPAddress, lTmp[0], lTmp[1], lTmp[2], lTmp[3]);
    _snprintf((char *) lTmpNode->sData.IPaddressStr, sizeof(lTmpNode->sData.IPaddressStr)-1, "%d.%d.%d.%d", lTmp[0], lTmp[1], lTmp[2], lTmp[3]);
    lTmpNode->sData.IPaddress = pIPAddress;
    lTmpNode->prev = NULL;
    lTmpNode->first = 0;
    lTmpNode->next = *pHostNodes;
    ((PHOSTNODE) *pHostNodes)->prev = lTmpNode;
    *pHostNodes = lTmpNode;
  } // if (pSysMAC != NUL...

  return(lTmpNode);
}
 


/*
 *
 *
 */
PHOSTNODE GetNodeByIP(PHOSTNODE pSysNodes, unsigned long pIPAddress)
{
  PHOSTNODE lRetVal = NULL;
  PHOSTNODE lTmpSys;
  int lCount = 0;

  if ((lTmpSys = pSysNodes) != NULL)
  {
    /*
     * Go to the end of the list
     */
    for (lCount = 0; lCount < MAX_NODE_COUNT; lCount++)
    {
      if (lTmpSys != NULL)
      {
        if (lTmpSys->sData.IPaddress == pIPAddress)
      		{
          lRetVal = lTmpSys;
          break;
        } // if (strncmp(l..
      } // if (lTmp...

      if((lTmpSys = lTmpSys->next) == NULL)
        break;

    } // for (lCount = ...
  } // if (pMAC != ...

  return(lRetVal);
}


