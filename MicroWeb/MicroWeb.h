#ifndef __MICROWEB__
#define __MICROWEB__

#define HTTP_PORT 80

#define OK 0
#define NOK 1


#define MAX_BUF_SIZE 1024
#define snprintf _snprintf


#define DBG_LOGFILE "debug.log"
#define DEBUG_LEVEL 0
#define DBG_OFF    5
#define DBG_INFO   1
#define DBG_LOW    2
#define DBG_MEDIUM 3
#define DBG_HIGH   4
#define DBG_ALERT  5
#define DBG_ERROR  5



/*
 * Function forward declarations.
 * 
 */
DWORD WINAPI MicroWebThread(LPVOID lpParam);
int MiniWebQuit(int arg) ;
void LogMsg(int pPriority, char *pMsg, ...);

#endif