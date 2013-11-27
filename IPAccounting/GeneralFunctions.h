#ifndef __GENERALFUNCTIONS__
#define __GENERALFUNCTIONS__


/*
 * Function forward declarations
 *
 */
void LogMsg(int pPriority, char *pMsg, ...);
int UserIsAdmin();
void adminCheck(char *pProgName);
int GetIFCName(char *pIFCName, char *pRealIFCName, int pBufLen);
int GetIFCDetails(char *pIFCName, PSCANPARAMS pScanParams);
int GetAliasByIfcIndex(int pIfcIndex, char *pAliasBuf, int pBufLen);
int ListIFCDetails();
int IPAddrBelongsToLocalNet(unsigned long pLocalIP, unsigned long pNetmask, unsigned long pTestAddr);
DWORD WINAPI IPLookup(void *pParams);

#endif