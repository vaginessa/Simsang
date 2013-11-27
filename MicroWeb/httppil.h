///////////////////////////////////////////////////////////////////////
//
// httpapi.h
//
// Header file for Miniweb Platform Independent Layer
//
///////////////////////////////////////////////////////////////////////

#ifndef _HTTPPIL_H_
#define _HTTPPIL_H_

#include <windows.h>
#include <io.h>

#define ssize_t size_t
#define socklen_t int
typedef HANDLE pthread_t;
typedef HANDLE pthread_mutex_t;

typedef unsigned char OCTET;

#define SHELL_NOREDIRECT 1
#define SHELL_SHOWWINDOW 2
#define SHELL_NOWAIT 4

#define msleep(ms) (Sleep(ms))

int InitSocket();
void UninitSocket();
char *GetTimeString();
int ThreadCreate(pthread_t *pth, void* (*start_routine)(void*), void* arg);
int ThreadKill(pthread_t pth);
int ThreadWait(pthread_t pth,void** ret);
#endif
