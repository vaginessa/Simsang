
#include <Windows.h>
#include <stdio.h>
#include <string.h>
#include <time.h>

#include "MicroWeb.h"


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

        _strtime(lTimeStamp);
        _strdate(lDateStamp);
        snprintf(lTime, sizeof(lTime) - 1, "%s %s", lDateStamp, lTimeStamp);


        ZeroMemory(lTemp, sizeof(lTemp));
        ZeroMemory(lLogMsg, sizeof(lLogMsg));
        va_start (lArgs, pMsg);
        vsprintf(lTemp, pMsg, lArgs);
        va_end(lArgs);
      		snprintf(lLogMsg, sizeof(lLogMsg) - 1, "%s : %s\n", lTime, lTemp);


//        SetFilePointer(lFH, 0, NULL, FILE_END);
//        WriteFile(lFH, lLogMsg, strnlen(lLogMsg, sizeof(lLogMsg) - 1), &lBytedWritten, NULL);
printf(lLogMsg);
        UnlockFileEx(lFH, 0, 0, 0, &lOverl);
   	  } // if (LockFileEx(lF...
      CloseHandle(lFH);
    } // if ((lFH = CreateF...
  } // if (pPriori...
}