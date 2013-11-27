#ifndef __HELPERFUNCTIONS__
#define __HELPERFUNCTIONS__


/*
 * Function forward declarations
 *
 */
void printUsage(char *pProgName);
void LogMsg(int pPriority, char *pMsg, ...);
void adminCheck(char *pProgName);
int UserIsAdmin();
void ExecCommand(char *pCmd);
void MAC2String(unsigned char pMAC[BIN_MAC_LEN], unsigned char *pOutput, int pOutputLen);
void IP2String(unsigned char pIP[BIN_IP_LEN], unsigned char *pOutput, int pOutputLen);
int String2IP(unsigned char pIP[BIN_IP_LEN], unsigned char *pInput, int pInputLen);
void String2MAC(unsigned char pMAC[BIN_MAC_LEN], unsigned char *pInput, int pInputLen);



#endif