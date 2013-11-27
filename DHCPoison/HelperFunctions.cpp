#define HAVE_REMOTE

#include <pcap.h>
#include <stdio.h>
#include <stdlib.h>
//#include <iphlpapi.h>
#include <Shlwapi.h>



#include "DHCPoison.h"
#include "HelperFunctions.h"


/*
 * Print usage
 *
 */
void printUsage(char *pAppName)
{
  if (pAppName == NULL)
    pAppName = "PROG_NAME";

  system("cls");
  printf("\nDHCPoison Version %s\n", DHCPOISON_VERSION);
 	printf("---------------------\n\n");
  printf("Web\t http://www.buglist.io/\n");
  printf("Mail\t buglist.io@gmail.com\n\n\n");
 	printf("List all interfaces             : %s -l\n", pAppName);
  printf("Start DHCP poisoning/forwarding : %s -x Ifc-Name StartIP StopIP\n", pAppName);
  printf("\n\n\n\nExamples\n--------\n\n");
  printf("Example : %s {7BC69807-9515-46E4-A4B7-HBAF13E820BA} 192.168.1.100 192.168.110\n\n\n\n\n", pAppName);
  printf("WinPcap version\n---------------\n\n");
  printf("%s\n\n", pcap_lib_version());
}




/*
 *
 *
 */
void LogMsg(int pPriority, char *pMsg, ...)
{
  HANDLE lFH = INVALID_HANDLE_VALUE;
  OVERLAPPED lOverl = { 0 };
  char lDateStamp[MAX_BUF_SIZE + 1];
  char lTimeStamp[MAX_BUF_SIZE + 1];
  char lTime[MAX_BUF_SIZE + 1];
  char lTemp[MAX_BUF_SIZE + 1];
  char lLogMsg[MAX_BUF_SIZE + 1];
  DWORD lBytedWritten = 0;
  va_list lArgs;



  if (pPriority >= DEBUG_LEVEL && DEBUG_LEVEL != DBG_OFF)
  { 
    if ((lFH = CreateFile(DBG_LOGFILE, GENERIC_READ|GENERIC_WRITE, 0, 0, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0)) != INVALID_HANDLE_VALUE)
    {
      ZeroMemory(&lOverl, sizeof(lOverl));

      if (LockFileEx(lFH, LOCKFILE_EXCLUSIVE_LOCK, 0, 0, 0, &lOverl) == TRUE)
      {
        ZeroMemory(lTime, sizeof(lTime));
        ZeroMemory(lTimeStamp, sizeof(lTimeStamp));
        ZeroMemory(lDateStamp, sizeof(lDateStamp));


        /*
         * Create timestamp
         */
        _strtime(lTimeStamp);
        _strdate(lDateStamp);
        _snprintf(lTime, sizeof(lTime) - 1, "%s %s", lDateStamp, lTimeStamp);

        /*
         * Create log message
         */
        ZeroMemory(lTemp, sizeof(lTemp));
        ZeroMemory(lLogMsg, sizeof(lLogMsg));
        va_start (lArgs, pMsg);
        vsprintf(lTemp, pMsg, lArgs);
        va_end(lArgs);
      		_snprintf(lLogMsg, sizeof(lLogMsg) - 1, "%s : %s\n", lTime, lTemp);
printf(lLogMsg);
        /*
         * Write message to the logfile.
         */
        SetFilePointer(lFH, 0, NULL, FILE_END);
        WriteFile(lFH, lLogMsg, strnlen(lLogMsg, sizeof(lLogMsg) - 1), &lBytedWritten, NULL);
        UnlockFileEx(lFH, 0, 0, 0, &lOverl);
	     } // if (LockFileEx(lF...
      CloseHandle(lFH);
    } // if ((lFH = CreateF...
  } // if (pPriori...
}







/*
 *
 *
 */
int UserIsAdmin()
{
  BOOL lRetVal = FALSE;
  SID_IDENTIFIER_AUTHORITY lNtAuthority = SECURITY_NT_AUTHORITY;
  PSID lAdmGroup = NULL;


  if(AllocateAndInitializeSid(&lNtAuthority, 2, SECURITY_BUILTIN_DOMAIN_RID,
    DOMAIN_ALIAS_RID_ADMINS, 0, 0, 0, 0, 0, 0, &lAdmGroup)) 
  {
    if (! CheckTokenMembership(NULL, lAdmGroup, &lRetVal))
      lRetVal = FALSE;
    FreeSid(lAdmGroup); 
  }

  return(lRetVal);
}







/*
 *
 *
 */
void adminCheck(char *pProgName)
{
  /*
   * The user needs adminstrator privileges to 
   * run APE successfully.
   */
  if(! UserIsAdmin())
  {
    system("cls");
    printf("\nDHCPoison Version %s\n", DHCPOISON_VERSION);
 	  printf("---------------------\n\n");
    printf("Web\t http://www.buglist.io/\n");
    printf("Mail\t buglist.io@gmail.com\n\n\n");
    printf("You need Administrator permissions to run %s successfully!\n\n", pProgName);
   	exit(1);
  }
}









/*
 *
 *
 */
void ExecCommand(char *pCmd)
{
  STARTUPINFO lSI;   
  PROCESS_INFORMATION lPI;
  char lTemp[MAX_BUF_SIZE + 1];
  char *lComSpec = getenv("COMSPEC");


  /*
   * Build command string + execute it.
   *
   */
  if (pCmd != NULL)
  {
    ZeroMemory(&lSI, sizeof(lSI));
    ZeroMemory(&lPI, sizeof(lPI));
    ZeroMemory(lTemp, sizeof(lTemp));

   	lComSpec = lComSpec!=NULL?lComSpec:"cmd.exe";

    lSI.cb = sizeof(STARTUPINFO);
    lSI.dwFlags = STARTF_USESHOWWINDOW;
    lSI.wShowWindow = SW_HIDE;

   	_snprintf(lTemp, sizeof(lTemp) - 1, "%s /c %s", lComSpec, pCmd);
	   LogMsg(DBG_INFO, "ExecCommand() : %s", lTemp);

   	CreateProcess(NULL, lTemp, NULL, NULL, FALSE, CREATE_NEW_CONSOLE, NULL, NULL, &lSI, &lPI);
  } // if (pCmd != N ...
}


/*
 *
 *
 */
void MAC2String(unsigned char pMAC[BIN_MAC_LEN], unsigned char *pOutput, int pOutputLen)
{
  if (pOutput && pOutputLen > 0 && pMAC != NULL && pOutputLen >= MAX_MAC_LEN)
    _snprintf((char *) pOutput, pOutputLen-1, "%02X-%02X-%02X-%02X-%02X-%02X", pMAC[0], pMAC[1], pMAC[2], pMAC[3], pMAC[4], pMAC[5]);
}

void IP2String(unsigned char pIP[BIN_IP_LEN], unsigned char *pOutput, int pOutputLen)
{
  if (pOutput && pOutputLen > 0)
    _snprintf((char *) pOutput, pOutputLen, "%d.%d.%d.%d", pIP[0], pIP[1], pIP[2], pIP[3]);
}



void String2MAC(unsigned char pMAC[BIN_MAC_LEN], unsigned char *pInput, int pInputLen)
{
  if (pInput != NULL && pInputLen > 0)
    if (sscanf((char *) pInput, "%02X:%02X:%02X:%02X:%02X:%02X", &pMAC[0], &pMAC[1], &pMAC[2], &pMAC[3], &pMAC[4], &pMAC[5]) != 6)
      sscanf((char *) pInput, "%02X-%02X-%02X-%02X-%02X-%02X", &pMAC[0], &pMAC[1], &pMAC[2], &pMAC[3], &pMAC[4], &pMAC[5]);
}

int String2IP(unsigned char pIP[BIN_IP_LEN], unsigned char *pInput, int pInputLen)
{
  int lRetVal = 1;
  unsigned char lIP[BIN_IP_LEN];

  ZeroMemory(lIP, sizeof(BIN_IP_LEN));
  ZeroMemory(pIP, sizeof(BIN_IP_LEN));

  if (pInput != NULL && pInputLen > 0)
    if (sscanf((char *) pInput, "%d.%d.%d.%d", &pIP[0], &pIP[1], &pIP[2], &pIP[3]) == 4)
      if ((lIP[0] | lIP[1] | lIP[2] | lIP[3]) < 255)
        if (strspn((char *) pInput, "0123456789.") == strlen((char *) pInput))
          lRetVal = 0;

  return(lRetVal);
}