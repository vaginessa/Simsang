#include <stdio.h>
#include <string.h>
#include <sys/stat.h>

#include "httppil.h"
#include "httpapi.h"
#include "microweb.h"


#pragma comment(lib, "Ws2_32")



UrlHandler urlHandlerList[]=
{
	{NULL},
	{NULL},
	{NULL},
};

HttpParam *httpParam;
int nInst = 0;


#define TOTAL_COUNTERS 8
static unsigned long counter[TOTAL_COUNTERS];



/*
 * Thread entry point.
 *
 */
//DWORD WINAPI MicroWebThread(LPVOID lpParam) 
int main(int argc, char *argv[])
{
  DWORD lRetVal = 0;
  int lFuncRetVal = 0;
  int lCount = 0;
  struct stat st;
  FILE *lFH = NULL;
  
  LogMsg(DBG_INFO, "MicroWebThread() : Starting MicroWeb");



  /*
   * create 404.html if it doesn't exist
   */
  if (stat("404.html", &st) < 0)
  {
    if ((lFH = fopen("404.html", "w")) != NULL)
    {
      fprintf(lFH, "<html><body><b>OOPS!</b><br />File not found!</body></html>");
      fclose(lFH);
    }
  }


  /*
   *
   */
/*
  if ((lCancelEvent = OpenEvent(EVENT_ALL_ACCESS , FALSE, "Cancellation")) == INVALID_HANDLE_VALUE)
  {
    LogMsg(DBG_ERROR, "MicroWebThread() : OpenEvent failed (%d)", GetLastError());
    lRetVal = 1;
    goto END;
  }
*/


  /*
   * Start the  server.
   */
//  SetConsoleCtrlHandler((PHANDLER_ROUTINE) MiniWebQuit, TRUE);

  

 	if (!nInst) 
	  	nInst=1;


 	//initialize HTTP parameter structure
	 {
		  int iParamBytes = nInst * sizeof(HttpParam);
		
  		if (!(httpParam = malloc(iParamBytes))) 
		  {
   			LogMsg(DBG_ERROR, "MicroWebThread() : Out of memory");
			   return -1;
		  }
  		ZeroMemory(httpParam, iParamBytes);
	 }

 	//fill in default settings
	 {
  		int i;
		  for (i = 0; i < nInst; i++) 
		  {
   			(httpParam+i)->maxClients = 32;
			   (httpParam+i)->maxReqPerConn = 99;
   			(httpParam+i)->pchWebPath = ".\\";
			   (httpParam+i)->pxUrlHandler = urlHandlerList;
   			(httpParam+i)->flags = FLAG_DIR_LISTING;
		  }
	 }

 	//parsing command line arguments
 	{
  		int inst=0;
    (httpParam+inst)->httpPort = HTTP_PORT;
    (httpParam+inst)->pchWebPath = "/";		
	 }

 	//adjust port setting
	 {
		  int i;
	  	short int port = HTTP_PORT;
	  	for (i = 0; i < nInst; i++) 
		  {
   			if ((httpParam+i)->httpPort)
			    	port = (httpParam+i)->httpPort + 1;
   			else
			    	(httpParam+i)->httpPort = port++;
		  }
	 }


	 InitSocket();

	 if (nInst>1) 
    LogMsg(DBG_INFO, "MicroWeb() : Number of instances: %d", nInst);

	 {
		  int i;
		  int error = 0;

		  for (i = 0; i < nInst; i++) 
		  {
   			int n;

	 		  for (n=0; urlHandlerList[n].pchUrlPrefix; n++);

      LogMsg(DBG_INFO, "MicroWebThread() : Listening port: %d, web root %s, Max clients: %d, No. handlers: %d", (httpParam+i)->httpPort, (httpParam+i)->pchWebPath, (httpParam+i)->maxClients, n);


  			//register page variable substitution callback
			  //(httpParam+i)->pfnSubst=DefaultWebSubstCallback;

		 	  //start server
	   		if ((lFuncRetVal = mwServerStart(&httpParam[i])) != 0) 
   			{
  				  LogMsg(DBG_ERROR, "MicroWebThread() : Error \"%d\" starting instance #%d", lFuncRetVal, i);
			    	error++;
		   	}
  		}
	
		  if (error < nInst) 
  		{
      LogMsg(DBG_INFO, "MicroWebThread() : Waiting for micro web thread return");

      /*
       * Waiting for micro web thread returning
       */
      while (TRUE)
      {
        if (WaitForSingleObject(httpParam[0].tidHttpThread, 100) != WAIT_TIMEOUT)
        {
          LogMsg(DBG_INFO, "MicroWebThread() : Thread stopped running.");
          break;
        }

/*
        if (WaitForSingleObject(lCancelEvent, 100) != WAIT_TIMEOUT)
        {
          LogMsg(DBG_INFO, "MicroWebThread() : Cancellation event set.");
          break;
        } // if (WaitForSingleObject(...
*/
      } // while (TRUE)...
	  	} 
    else 
    {
		  	 LogMsg(DBG_ERROR, "MicroWebThread() : ailed to launch miniweb");
		  } // if (error < nInst) ...
	 }

	 //shutdown server
 	{
  		if (nInst > 1) 
		  {
   			int i;
			   for (i = 0; i < nInst; i++) 
			   {
				    LogMsg(DBG_INFO, "MicroWebThread() : Shutting down instance %d", i);
    				mwServerShutdown(&httpParam[i]);
		   	}
  		}
		  else
    {
      LogMsg(DBG_INFO, "MicroWebThread() : Shutting down single threaded server");
		   	mwServerShutdown(&httpParam[0]);		
    }
	 }

END:
	 UninitSocket();
  MiniWebQuit(1);

  if (httpParam != NULL)
    free(httpParam);

  LogMsg(DBG_INFO, "MicroWebThread() : Stopping MicroWeb");


 	return(lRetVal);
}



/*
 *
 */
int itoc (int num, char *pbuf, int type)
{
  static const char *chNum[]={"零","一","二","三","四","五","六","七","八","九"};
  static const char *chUnit[]={"亿","万","千","百","十","",NULL};
  char *p=pbuf;
  int c=1000000000,unit=4,d,last=0;

  if (num == 0)
    return sprintf(pbuf,chNum[0]);

  if (num<0)
  {
    p+=sprintf(pbuf,"负");
    num=-num;
  }

	d=num;
	for (;;) 
	{
		do 
		{
			int tmp=d/c;
			if (tmp>0) 
			{
				p+=sprintf(p,"%s%s",(unit==2 && tmp==1)?"":chNum[tmp],chUnit[unit]);
				d%=c;
			}
			else if (last!=0 && c>=10 && d>0) 
			{
				p+=sprintf(p,chNum[0]);
			}
			last=tmp;
			c/=10;
		} while(chUnit[++unit]);

		if (c==0)
			break;
		if (c==1000 && num>=10000)
			p+=sprintf(p,chUnit[1]);
		else if (c==10000000 && num>=100000000)
			p+=sprintf(p,chUnit[0]);

		unit=2;
	}
	return (int)(p-pbuf);
}



//////////////////////////////////////////////////////////////////////////
// callback from the web server whenever it needs to substitute variables
//////////////////////////////////////////////////////////////////////////
int DefaultWebSubstCallback(SubstParam* sp)
{
	// the maximum length of variable value should never exceed the number
	// given by sp->iMaxValueBytes
	if (!strcmp(sp->pchParamName,"mykeyword"))
		return sprintf(sp->pchParamValue, "%d", time(NULL));

	return -1;
}



int MiniWebQuit(int arg) 
{
  int i;
  if (arg)
	  LogMsg(DBG_INFO, "MicroWebThread() : Caught signal (%d). MiniWeb shutting down...", arg);

  for (i=0;i<nInst;i++)
    (httpParam+i)->bKillWebserver = 1;

  return 0;
}





