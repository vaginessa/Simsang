#ifndef __NETWORKDEFINITIONS__
#define __NETWORKDEFINITIONS__


/*
 * Function forward declarations
 *
 */
int ListIFCDetails();
int GetAliasByIfcIndex(int pIfcIndex, char *pAliasBuf, int pBufLen);
void RemoveMAC(char *pIfcAlias, char *pIPAddr);
void SetMACStatic(char *pIfcAlias, char *pIP, char *pMAC);
int GetIFCName(char *pIFCName, char *pRealIFCName, int pBufLen);
int GetIFCDetails(char *pIFCName, PSCANPARAMS pScanParams);
#endif 