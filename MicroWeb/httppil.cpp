#include <stdio.h>
#include <time.h>
#include <string.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <ctype.h>
#include <fcntl.h>
#include <io.h>
#include "httppil.h"



/*
 *
 */
int InitSocket()
{
 	WSADATA wsaData;
 	if (WSAStartup(MAKEWORD(2, 2), &wsaData))
		  return 0;
	 
 	return(1);   
}



/*
 *
 */
void UninitSocket()
{
  WSACleanup( );
}



/*
 *
 */
char *GetTimeString()
{
	 static char buf[16];
	 time_t tm=time(NULL);
	 CopyMemory(buf, ctime(&tm)+4, 15);
	 buf[15]=0;
	 return buf;
}



/*
 *
 */
int ThreadCreate(pthread_t *pth, void* (*start_routine)(void*), void* arg)
{
 	DWORD dwid;	    
	 *pth = CreateThread(0, 0, (LPTHREAD_START_ROUTINE)start_routine, arg, 0, &dwid);
	 return *pth!=NULL?0:1;
}




/*
 *
 */
int ThreadKill(pthread_t pth)
{
 	return TerminateThread(pth,0)?0:1;
}



/*
 *
 */
int ThreadWait(pthread_t pth, void** ret)
{
  if (WaitForSingleObject(pth,INFINITE) != WAIT_OBJECT_0)
    return GetLastError();

  if (ret) 
    GetExitCodeThread(pth,(LPDWORD)ret);

  return 0;
}


