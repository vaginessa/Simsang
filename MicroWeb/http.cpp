#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>
#include <fcntl.h>
#include <sys/stat.h>
#include <Shlwapi.h>

#include "httppil.h"
#include "httpapi.h"
#include "httpint.h"
#include "MicroWeb.h"
//#include "BamBam.h"
//#include "HelperFunctions.h"



char* contentTypeTable[]=
{
  HTTPTYPE_OCTET,
  HTTPTYPE_HTML,
  HTTPTYPE_XML,
  HTTPTYPE_TEXT,
  HTTPTYPE_CSS,
  HTTPTYPE_PNG,
  HTTPTYPE_JPEG,
  HTTPTYPE_GIF,
  HTTPTYPE_SWF,
  HTTPTYPE_MPA,
  HTTPTYPE_MPEG,
  HTTPTYPE_AVI,
  HTTPTYPE_QUICKTIME,
  HTTPTYPE_QUICKTIME,
  HTTPTYPE_JS,
  HTTPTYPE_OCTET,
  HTTPTYPE_STREAM
};

char* defaultPages[] = {"index.htm", "index.html", "default.htm", NULL};


////////////////////////////////////////////////////////////////////////////
// API callsc
////////////////////////////////////////////////////////////////////////////

const char *dayNames="Sun\0Mon\0Tue\0Wed\0Thu\0Fri\0Sat";
const char *monthNames="Jan\0Feb\0Mar\0Apr\0May\0Jun\0Jul\0Aug\0Sep\0Oct\0Nov\0Dec";
const char *httpDateTimeFormat="%s, %02d %s %02d %02d:%02d:%02d GMT";




/*
 *
 */
int mwGetHttpDateTime(time_t timer, char *buf)
{
 	struct tm *btm;
 	btm = gmtime(&timer);
 	return sprintf(buf, httpDateTimeFormat, dayNames+(btm->tm_wday<<2), btm->tm_mday, monthNames+(btm->tm_mon<<2), 1900+btm->tm_year, btm->tm_hour, btm->tm_min, btm->tm_sec);
}



/*
 * Start the webserver
 */
int mwServerStart(HttpParam* hp)
{
	 int i;

 	if (hp->bWebserverRunning) 
	 {
  		LogMsg(DBG_ERROR, "mwServerStart() : Webserver thread already running");
		  return(-1);
	 }
 

	 if (!InitSocket()) 
  {
		  LogMsg(DBG_ERROR, "mwServerStart() : Error initializing Winsock");
		  return(-2);
	 }
	
 	for (i = 0; (hp->pxUrlHandler+i)->pchUrlPrefix; i++)
	  	if ((hp->pxUrlHandler+i)->pfnEventHandler && (hp->pxUrlHandler+i)->pfnEventHandler(MW_INIT, hp))
			   (hp->pxUrlHandler+i)->pfnUrlHandler = NULL;
	

 	if (!(hp->listenSocket = _mwStartListening(hp))) 
	  	return(-3);

 	hp->stats.startTime = time(NULL);
	 hp->bKillWebserver = FALSE;
	 hp->bWebserverRunning = TRUE;

 	if (ThreadCreate(&hp->tidHttpThread, _mwHttpThread, (void*)hp)) 
 	{ 
	  	LogMsg(DBG_ERROR, "mwServerStart() :  reating server thread");
		  return(-4);
	 }

	 return(0);
}

////////////////////////////////////////////////////////////////////////////
// mwServerShutdown
// Shutdown the webserver
////////////////////////////////////////////////////////////////////////////
int mwServerShutdown(HttpParam* hp)
{
 	int i;

 	LogMsg(DBG_ERROR, "mwServerShutdown() : Shutting down web server thread");
	 // signal webserver thread to quit
	 hp->bKillWebserver = TRUE;
  
 	// and wait for thread to exit
	 for (i = 0; hp->bWebserverRunning && i < 10; i++) 
		  Sleep(100);

 	for (i = 0; (hp->pxUrlHandler+i)->pchUrlPrefix; i++)
	  	if ((hp->pxUrlHandler+i)->pfnUrlHandler && (hp->pxUrlHandler+i)->pfnEventHandler)
			   (hp->pxUrlHandler+i)->pfnEventHandler(MW_UNINIT, hp);


  /*
   * Bringing the thread painfully in its knees. 
   */
  TerminateThread(hp->tidHttpThread, 0);
  hp->tidHttpThread = INVALID_HANDLE_VALUE;
  closesocket(hp->listenSocket);


 	UninitSocket();
 	LogMsg(DBG_INFO, "mwServerShutdown() : Webserver shutdown complete");
 	return(0);
}






/*
 *
 */
int mwGetLocalFileName(HttpFilePath* hfp)
{
 	char ch;
	 char *p = hfp->cFilePath;
  char *s = hfp->pchHttpPath;
  char *upLevel = NULL;

 	hfp->pchExt = NULL;
 	hfp->fTailSlash = 0;

 	if (hfp->pchRootPath) 
	 {
  		p += _mwStrCopy(hfp->cFilePath, hfp->pchRootPath);
		  if (*(p-1)!=SLASH)
		  {
   			*p=SLASH;
			   *(++p)=0;
		  }
	 }

	while ((ch=*s) && ch != '?' && (int) p - (int) hfp->cFilePath < sizeof(hfp->cFilePath)-1) 
	{
		if (ch == '%') 
  {
			*(p++) = _mwDecodeCharacter(++s);
			s += 2;
		} 
  else if (ch=='/') 
  {
			*p=SLASH;
			upLevel=(++p);
			while (*(++s)=='/');
			continue;
		} 
  else if (ch=='+') 
  {
			*(p++) = ' ';
			s++;
		}
  else if (ch=='.') 
  {
			if (upLevel && GETWORD(s+1) == DEFWORD('.','/')) 
   {
				s += 2;
				p=upLevel;
			} 
   else 
   {
				*(p++) = '.';
				hfp->pchExt = p;
				while (*(++s) == '.');	//avoid '..' appearing in filename for security issue
			}
		}
  else 
			*(p++) = *(s++);
		
	}

	if (*(p-1)==SLASH) 
 {
		p--;
		hfp->fTailSlash=1;
	}

	*p = 0;

	return (int) p-(int)hfp->cFilePath;
}




////////////////////////////////////////////////////////////////////////////
// Internal (private) helper functions
////////////////////////////////////////////////////////////////////////////

SOCKET _mwStartListening(HttpParam* hp)
{
 	SOCKET listenSocket;
 	int iRc;

  // create a new socket
  if ((listenSocket = socket(AF_INET,SOCK_STREAM,0)) < 0) 
	  	return(0);

#if 1
    // allow reuse of port number
    {
      int iOptVal = 1;      
      if ((iRc = setsockopt(listenSocket, SOL_SOCKET, SO_REUSEADDR, (char*) &iOptVal, sizeof(iOptVal))) < 0)
        return 0;
    }
#endif

  // bind it to the http port
  {
    struct sockaddr_in sinAddress;
    ZeroMemory(&sinAddress, sizeof(struct sockaddr_in));
    sinAddress.sin_family=AF_INET;
	   sinAddress.sin_addr.s_addr=htonl((hp->flags & FLAG_LOCAL_BIND) ? 0x7f000001 : INADDR_ANY);
    sinAddress.sin_port=htons(hp->httpPort); // http port
    

 	  if ((iRc = bind(listenSocket, (struct sockaddr*)&sinAddress, sizeof(struct sockaddr_in))) < 0) 
    {
    		LogMsg(DBG_ERROR, "_mwStartListening() : Error binding on port %d", hp->httpPort);
	    	return 0;
	   }
  }



    // listen on the socket for incoming calls
 	if (listen(listenSocket, hp->maxClients-1)) 
	 {
  		LogMsg(DBG_ERROR, "_mwStartListening() : Unable to listen on socket %d", listenSocket);
		  return 0;
	 }

  LogMsg(DBG_INFO, "_mwStartListening() : Http listen socket %d opened", listenSocket);
	 return(listenSocket);
}


/*
 *
 */
void _mwInitSocketData(HttpSocket *phsSocket)
{
 	ZeroMemory(&phsSocket->response, sizeof(HttpResponse));
 	ZeroMemory(&phsSocket->request, sizeof(HttpRequest));

 	phsSocket->fd = 0;
 	phsSocket->flags = FLAG_RECEIVING;
 	phsSocket->pucData = phsSocket->buffer;
 	phsSocket->iDataLength = 0;
 	phsSocket->response.iBufferSize = HTTP_BUFFER_SIZE;
 	phsSocket->ptr = NULL;
}




////////////////////////////////////////////////////////////////////////////
// _mwHttpThread
// Webserver independant processing thread. Handles all connections
////////////////////////////////////////////////////////////////////////////
void* _mwHttpThread(HttpParam *hp)
{  
 	HttpSocket *phsSocketCur;
 	SOCKET socket;
 	struct sockaddr_in sinaddr;
  int iRc;

  // main processing loop
 	while (!hp->bKillWebserver) 
	 {
		  time_t tmCurrentTime;
  		SOCKET iSelectMaxFds;
  		fd_set fdsSelectRead;
  		fd_set fdsSelectWrite;

  		// clear descriptor sets
	  	FD_ZERO(&fdsSelectRead);
	 	 FD_ZERO(&fdsSelectWrite);
 		 FD_SET(hp->listenSocket,&fdsSelectRead);
	  	iSelectMaxFds=hp->listenSocket;

  		// get current time
		  tmCurrentTime=time(NULL);  
  		// build descriptor sets and close timed out sockets
		  for (phsSocketCur=hp->phsSocketHead; phsSocketCur; phsSocketCur=phsSocketCur->next) 
		  {
   			int iError=0;
			   int iOptSize=sizeof(int);
  			// get socket fd
		  	socket=phsSocketCur->socket;
			  if (!socket) 
       continue;

  			if (getsockopt(socket,SOL_SOCKET,SO_ERROR,(char*)&iError,&iOptSize)) 
     {
   				// if a socket contains a error, close it
			   	LogMsg(DBG_INFO, "_mwHttpThread() : [%d] Socket no longer vaild.",socket);

  			 	phsSocketCur->flags=FLAG_CONN_CLOSE;
		  	 	_mwCloseSocket(hp, phsSocketCur);
				   continue;
			  }
  			// check expiration timer (for non-listening, in-use sockets)
	  		if (tmCurrentTime > phsSocketCur->tmExpirationTime) 
     {
   				LogMsg(DBG_INFO, "_mwHttpThread() : [%d] Http socket expired", phsSocketCur->socket);
		  		hp->stats.timeOutCount++;
				  // close connection
  				phsSocketCur->flags=FLAG_CONN_CLOSE;
		  		_mwCloseSocket(hp, phsSocketCur);
			  } 
     else 
     {
       // add to read descriptor set
			  	 if (ISFLAGSET(phsSocketCur,FLAG_RECEIVING))
					    FD_SET(socket,&fdsSelectRead);
				
       // add to write descriptor set
	 	  		if (ISFLAGSET(phsSocketCur,FLAG_SENDING)) 					
		 		   	FD_SET(socket, &fdsSelectWrite);
				
  	 			// check if new max socket
		   		if (socket>iSelectMaxFds)
				     iSelectMaxFds=socket;				
			  }
		 }

		 {
			  struct timeval tvSelectWait;
			  // initialize select delay
			  tvSelectWait.tv_sec = 1;
			  tvSelectWait.tv_usec = 0; // note: using timeval here -> usec not nsec

			  // and check sockets (may take a while!)
			  iRc = select(iSelectMaxFds+1, &fdsSelectRead, &fdsSelectWrite, NULL, &tvSelectWait);
		 }
		if (iRc < 0) 
  {
			if (hp->bKillWebserver) 
     break;

			msleep(1000);
			continue;
		}

		if (iRc > 0) 
  {
			HttpSocket *phsSocketNext;
			// check which sockets are read/write able
			phsSocketCur=hp->phsSocketHead;
			while (phsSocketCur) 
   {
				BOOL bRead;
				BOOL bWrite;

				phsSocketNext = phsSocketCur->next;
				// get socket fd
				socket = phsSocketCur->socket;
		          
				// get read/write status for socket
				bRead = FD_ISSET(socket, &fdsSelectRead);
				bWrite = FD_ISSET(socket, &fdsSelectWrite);

				if ((bRead|bWrite) != 0)
    {
					//DBG("socket %d bWrite=%d, bRead=%d\n",phsSocketCur->socket,bWrite,bRead);
					// if readable or writeable then process
					if (bWrite && ISFLAGSET(phsSocketCur,FLAG_SENDING))      
						iRc = _mwProcessWriteSocket(hp, phsSocketCur);					
     else if (bRead && ISFLAGSET(phsSocketCur,FLAG_RECEIVING))      
						iRc = _mwProcessReadSocket(hp,phsSocketCur);
     else 
     {
						iRc =- 1;
						LogMsg(DBG_ERROR, "_mwHttpThread() : Invalid socket state (flag: %08x)", phsSocketCur->flags);
						SETFLAG(phsSocketCur,FLAG_CONN_CLOSE);
					}

     // and reset expiration timer
					if (!iRc)      					 
						 phsSocketCur->tmExpirationTime = time(NULL) + HTTP_EXPIRATION_TIME;
					else    
					 	_mwCloseSocket(hp, phsSocketCur);
					
				}
				phsSocketCur = phsSocketNext;
			}

			// check if any socket to accept and accept the socket
			if (FD_ISSET(hp->listenSocket, &fdsSelectRead) && hp->stats.clientCount<hp->maxClients && 	(socket=_mwAcceptSocket(hp,&sinaddr))) 
   {
 				// create a new socket structure and insert it in the linked list	 			
		 		if (!(phsSocketCur = (HttpSocket*) malloc(sizeof(HttpSocket)))) 
     {
			  		LogMsg(DBG_ERROR, "_mwHttpThread() :Out of memory");
					  break;
				 }

 				phsSocketCur->next = hp->phsSocketHead;
 				hp->phsSocketHead = phsSocketCur;	//set new header of the list
		 		//fill structure with data
	 			_mwInitSocketData(phsSocketCur);
			 	phsSocketCur->tmAcceptTime = time(NULL);
 				phsSocketCur->socket = socket;
	 			phsSocketCur->tmExpirationTime = time(NULL) + HTTP_EXPIRATION_TIME;
		 		phsSocketCur->iRequestCount = 0;
			 	phsSocketCur->request.ipAddr.laddr = ntohl(sinaddr.sin_addr.s_addr);
				 hp->stats.clientCount++;
 				//update max client count
	 			if (hp->stats.clientCount>hp->stats.clientCountMax)
		  			hp->stats.clientCountMax = hp->stats.clientCount;

				 {
					  IP ip = phsSocketCur->request.ipAddr;
				  	LogMsg(DBG_INFO, "_mwHttpThread() : [%d] IP: %d.%d.%d.%d", phsSocketCur->socket,ip.caddr[3],ip.caddr[2],ip.caddr[1],ip.caddr[0]);
				 }

 			 LogMsg(DBG_INFO, "_mwHttpThread() : Connected clients: %d", hp->stats.clientCount);
			}
		} 
  else 
  {
			HttpSocket *phsSocketPrev=NULL;
			// select timeout
			// clean up the link list
			phsSocketCur=hp->phsSocketHead;
			while (phsSocketCur) 
   {
				if (!phsSocketCur->socket) 
    {
					LogMsg(DBG_INFO, "_mwHttpThread() : Free up socket structure 0x%08x", phsSocketCur);

					if (phsSocketPrev) 
     {
						phsSocketPrev->next=phsSocketCur->next;
						free(phsSocketCur);
						phsSocketCur=phsSocketPrev->next;
					}
     else 
     {
						hp->phsSocketHead=phsSocketCur->next;
						free(phsSocketCur);
						phsSocketCur=hp->phsSocketHead;
					}
				}
    else 
    {
					phsSocketPrev=phsSocketCur;
					phsSocketCur=phsSocketCur->next;
				}
			}
		}
	}

	 {
		  phsSocketCur=hp->phsSocketHead;
		  while (phsSocketCur) 
    {
			   HttpSocket *phsSocketNext;
			   phsSocketCur->flags = FLAG_CONN_CLOSE;
			   _mwCloseSocket(hp, phsSocketCur);
			   phsSocketNext = phsSocketCur->next;
			   free(phsSocketCur);
			   phsSocketCur = phsSocketNext;
	   }
 	}

	// clear state vars
	hp->bKillWebserver = FALSE;
	hp->bWebserverRunning = FALSE;
  
	return NULL;
} // end of _mwHttpThread




////////////////////////////////////////////////////////////////////////////
// _mwAcceptSocket
// Accept an incoming connection
////////////////////////////////////////////////////////////////////////////
SOCKET _mwAcceptSocket(HttpParam* hp,struct sockaddr_in *sinaddr)
{
  SOCKET socket;
 	int namelen=sizeof(struct sockaddr);

	 socket=accept(hp->listenSocket, (struct sockaddr*)sinaddr,&namelen);

  LogMsg(DBG_INFO, "_mwHttpThread() : [%d] connection accepted @ %s",socket,GetTimeString());

 	if ((int)socket<=0) 
  {
		  LogMsg(DBG_MEDIUM, "_mwHttpThread() : Error accepting socket");
		  return 0;
  } 

 	if (hp->socketRcvBufSize) 
  {
		  int iSocketBufSize=hp->socketRcvBufSize<<10;
		  setsockopt(socket, SOL_SOCKET, SO_RCVBUF, (const char*)&iSocketBufSize, sizeof(int));
	  }

 	return socket;
} // end of _mwAcceptSocket




int _mwBuildHttpHeader(HttpParam* hp, HttpSocket *phsSocket, time_t contentDateTime, unsigned char* buffer)
{
	char *p = buffer;
	p += sprintf(p,HTTP200_HEADER,	(phsSocket->request.iStartByte==0)?"200 OK":"206 Partial content",
		     HTTP_KEEPALIVE_TIME,hp->maxReqPerConn,
    		ISFLAGSET(phsSocket,FLAG_CONN_CLOSE)?"close":"Keep-Alive");
	p += mwGetHttpDateTime(contentDateTime, p);
	SETWORD(p,DEFWORD('\r','\n'));
	p += 2;
	p += sprintf(p,"Content-Type: %s\r\n",contentTypeTable[phsSocket->response.fileType]);
	if (phsSocket->response.iContentLength >= 0)
		p += sprintf(p,"Content-Length: %d\r\n", phsSocket->response.iContentLength);
	
	SETDWORD(p,DEFDWORD('\r','\n',0,0));
	return (int)p-(int)buffer+2;
}



/*
 *
 *
 */
int mwParseQueryString(UrlHandlerParam* up)
{
	if (up->iVarCount==-1) {
		//parsing variables from query string
		char *p,*s = NULL;
		if (ISFLAGSET(up->hs,FLAG_REQUEST_GET)) {
			// get start of query string
			s = strchr(up->pucRequest, '?');
			if (s) {
				*(s++) = 0;
			}
#ifdef HTTPPOST
		} else {
			s = up->hs->request.pucPayload;
#endif
		}
		if (s && *s) {
			int i;
			int n = 1;
			//get number of variables
			for (p = s; *p ; ) if (*(p++)=='&') n++;
			up->pxVars = calloc(n + 1, sizeof(HttpVariables));
			up->iVarCount = n;
			//store variable name and value
			for (i = 0, p = s; i < n; p++) {
				switch (*p) {
				case '=':
					if (!(up->pxVars + i)->name) {
						*p = 0;
						(up->pxVars + i)->name = s;
						s=p+1;
					}
					break;
				case 0:
				case '&':
					*p = 0;
					if ((up->pxVars + i)->name) {
						(up->pxVars + i)->value = s;
						_mwDecodeString(s);
					} else {
						(up->pxVars + i)->name = s;
						(up->pxVars + i)->value = p;
					}
					s = p + 1;
					i++;
					break;
				}
			}
			(up->pxVars + n)->name = NULL;
		}
	}
	return up->iVarCount;
}



/*
 *
 *
 */
int _mwCheckUrlHandlers(HttpParam* hp, HttpSocket* phsSocket)
{
	UrlHandler* puh;
	UrlHandlerParam up;
	int ret = 0;


	up.pxVars = NULL;
	for (puh = hp->pxUrlHandler; puh->pchUrlPrefix; puh++) 
 {
		int iPrefixLen = strlen(puh->pchUrlPrefix);

		if (puh->pfnUrlHandler && !strncmp(phsSocket->request.pucPath, puh->pchUrlPrefix, iPrefixLen)) 
  {
			//URL prefix matches
			ZeroMemory(&up, sizeof(up));
			up.hp=hp;
			up.hs = phsSocket;
			up.iDataBytes = phsSocket->response.iBufferSize;
			up.pucRequest = phsSocket->request.pucPath+iPrefixLen;
			up.pucHeader = phsSocket->buffer;
			up.pucBuffer = phsSocket->pucData;
			up.pucBuffer[0] = 0;
			up.iVarCount = -1;
			phsSocket->ptr = (void*) puh->pfnUrlHandler;
			

   if(!(ret = (*(PFNURLCALLBACK)phsSocket->ptr)(&up))) 
     continue;

			if (ret & FLAG_DATA_REDIRECT) 
			{
				_mwRedirect(phsSocket, up.pucBuffer);
				LogMsg(DBG_INFO, "_mwCheckUrlHandlers() : URL handler: redirect");
			}
   else 
   {
				phsSocket->flags |= ret;
				phsSocket->response.fileType = up.fileType;
				hp->stats.urlProcessCount++;

				if (ret & FLAG_TO_FREE)
					phsSocket->ptr = up.pucBuffer;	//keep the pointer which will be used to free memory later
				
				if (ret & FLAG_DATA_RAW)
    {
					phsSocket->pucData = up.pucBuffer;
					phsSocket->iDataLength = up.iDataBytes;
					phsSocket->response.iContentLength = up.iContentBytes>0?up.iContentBytes:up.iDataBytes;
					LogMsg(DBG_INFO, "_mwCheckUrlHandlers() : URL handler: raw data)");
				}
    else if (ret & FLAG_DATA_FILE) 
    {
					phsSocket->flags |= FLAG_DATA_FILE;
					if (up.pucBuffer[0])
						phsSocket->request.pucPath = up.pucBuffer;

					LogMsg(DBG_INFO, "_mwCheckUrlHandlers() : URL handler: file");
				} 
    else if (ret & FLAG_DATA_FD) 
    {
					phsSocket->flags |= FLAG_DATA_FILE;
					LogMsg(DBG_INFO, "_mwCheckUrlHandlers() : URL handler: file descriptor");
				}
				break;
			}
		}
	}

	if (up.pxVars) 
   free(up.pxVars);

	return ret;
}




////////////////////////////////////////////////////////////////////////////
// _mwProcessReadSocket
// Process a socket (read)
////////////////////////////////////////////////////////////////////////////
int _mwProcessReadSocket(HttpParam* hp, HttpSocket* phsSocket)
{
 	char *p;
  char *lTempPtr = NULL;
  char lTemp[MAX_BUF_SIZE + 1];
#ifdef HTTPPOST
  if ((HttpMultipart*) phsSocket->ptr != NULL) 
  {
    //_mwProcessMultipartPost(phsSocket);
    return 0;
  }
#endif
 	// check if receive buffer full
	 if (phsSocket->iDataLength >= MAX_REQUEST_SIZE) 
  {
  		// close connection
		  LogMsg(DBG_INFO, "_mwProcessReadSocket() : Invalid request header size (%d bytes)", phsSocket->iDataLength);
		  SETFLAG(phsSocket, FLAG_CONN_CLOSE);
		  return -1;
	 }
	// read next chunk of data

 	{
	  	int sLength;
	  	sLength = recv(phsSocket->socket, phsSocket->pucData + phsSocket->iDataLength, phsSocket->response.iBufferSize-phsSocket->iDataLength, 0);

  		if (sLength <= 0) 
    {
   			LogMsg(DBG_INFO, "_mwProcessReadSocket() : [%d] socket closed by client",phsSocket->socket);
			   SETFLAG(phsSocket, FLAG_CONN_CLOSE);
   			return -1;
		  }
  		// add in new data received
		  phsSocket->iDataLength+=sLength;
	 }

 	//check request type
 	switch (GETDWORD(phsSocket->pucData)) 
  {
   	case HTTP_GET:
	    	SETFLAG(phsSocket,FLAG_REQUEST_GET);
	    	phsSocket->request.pucPath = phsSocket->pucData+5;
	    	break;
#ifdef HTTPPOST
   	case HTTP_POST:
		    SETFLAG(phsSocket,FLAG_REQUEST_POST);
		    phsSocket->request.pucPath = phsSocket->pucData+6;
    		break;
#endif
	 }

 	// check if end of request
 	if (phsSocket->request.siHeaderSize == 0) 
  {
	  	int i = 0;
		  while (GETDWORD(phsSocket->buffer + i) != HTTP_HEADEREND) 
   			if (++i > phsSocket->iDataLength - 3)
        return(0);
  		
		  // reach the end of the header
  		if (!ISFLAGSET(phsSocket,FLAG_REQUEST_GET|FLAG_REQUEST_POST)) 
    {
 			  LogMsg(DBG_LOW, "_mwProcessReadSocket() : [%d] Unsupported method", phsSocket->socket);		
	   		SETFLAG(phsSocket,FLAG_CONN_CLOSE);
  		 	return -1;
		  }

  		phsSocket->request.siHeaderSize = i + 4;
		  LogMsg(DBG_INFO, "_mwProcessReadSocket() : [%d] header size: %d bytes", phsSocket->socket, phsSocket->request.siHeaderSize);

  		if (_mwParseHttpHeader(phsSocket)) 
    {
			   LogMsg(DBG_LOW, "_mwProcessReadSocket() : Error parsing request");
			   SETFLAG(phsSocket, FLAG_CONN_CLOSE);
			   return -1;
#ifdef HTTPPOST
		  } 
    else if (ISFLAGSET(phsSocket,FLAG_REQUEST_POST)) 
    {
	 		  hp->stats.reqPostCount++;
	   		phsSocket->request.pucPayload=malloc(phsSocket->response.iContentLength+1);
  		 	phsSocket->request.pucPayload[phsSocket->response.iContentLength]=0;
		 	  phsSocket->iDataLength -= phsSocket->request.siHeaderSize;
		   	memcpy(phsSocket->request.pucPayload, phsSocket->buffer + phsSocket->request.siHeaderSize, phsSocket->iDataLength);
  		 	phsSocket->pucData = phsSocket->request.pucPayload;
#endif
  		}
		  // add header zero terminator
  		phsSocket->buffer[phsSocket->request.siHeaderSize] = 0;
	 }


	 if (phsSocket->iDataLength < phsSocket->response.iContentLength) 
 	 	return 0;
	
 	p = phsSocket->buffer + phsSocket->request.siHeaderSize + 4;
 	p = (unsigned char*)((unsigned long)p & (-4));	//keep 4-byte aligned
 	*p = 0;

 	//keep request path
	 {
  		char *q;
  		int iPathLen;
 	 	for (q = phsSocket->request.pucPath;*q && *q != ' '; q++);
  		iPathLen = (int) q - (int) (phsSocket->request.pucPath);
	  	if (iPathLen >= MAX_REQUEST_PATH_LEN) 
    {
  	 		LogMsg(DBG_LOW, "_mwProcessReadSocket() : Request path too long and is stripped");
		   	iPathLen = MAX_REQUEST_PATH_LEN-1;
 		 }

 		 if (iPathLen > 0)
	 	  	memcpy(p, phsSocket->request.pucPath, iPathLen);

  		*(p + iPathLen) = 0;
	  	phsSocket->request.pucPath = p;
		  p = (unsigned char*)(((unsigned long)(p+iPathLen+4+1))&(-4));	//keep 4-byte aligned
 	}

 	phsSocket->pucData = p;	//free buffer space
 	phsSocket->response.iBufferSize = (HTTP_BUFFER_SIZE-(phsSocket->pucData-phsSocket->buffer)-1)&(-4);


 	LogMsg(DBG_INFO, "_mwProcessReadSocket() : [%d] request path: /%s", phsSocket->socket, phsSocket->request.pucPath);
    /*
     *
     */
  if (phsSocket != NULL && phsSocket->iDataLength > 0 && (lTempPtr = (strchr(phsSocket->buffer, '/')+1)) != NULL && 
      strcspn(lTempPtr, "\t ") > 0 && strcspn(lTempPtr, "\t ") < (sizeof(lTemp)-1))
  {
      ZeroMemory(phsSocket->URL, sizeof(phsSocket->URL));
      strncpy(phsSocket->URL, lTempPtr, strcspn(lTempPtr, "\t "));
      
//    if (!strncmp(phsSocket->buffer, "GET", 3))
//    strncpy(lTemp, phsSocket->buffer, sizeof(lTemp)-1);      
  } // if (phsSocket...



 	hp->stats.reqCount++;
 	if (ISFLAGSET(phsSocket, FLAG_REQUEST_GET|FLAG_REQUEST_POST)) 
  {
		  if (hp->pxUrlHandler) 
   			if (!_mwCheckUrlHandlers(hp, phsSocket))
		  	  	SETFLAG(phsSocket, FLAG_DATA_FILE);
		  
  		// set state to SENDING (actual sending will occur on next select)
	  	CLRFLAG(phsSocket, FLAG_RECEIVING)
	  	SETFLAG(phsSocket, FLAG_SENDING);
	  	hp->stats.reqGetCount++;

    // send requested page
  		if (ISFLAGSET(phsSocket, FLAG_DATA_FILE))
    {
      LogMsg(DBG_INFO, "_mwProcessReadSocket(5.0) : ");
			   return _mwStartSendFile(hp, phsSocket);
    } 
    else if (ISFLAGSET(phsSocket, FLAG_DATA_RAW)) 
    {
      LogMsg(DBG_INFO, "_mwProcessReadSocket(5.1) : ");
		   	return _mwStartSendRawData(hp, phsSocket);
    }
	 }

	 LogMsg(DBG_ERROR, "_mwProcessReadSocket() : Error occurred (might be a bug)");
	 return(-1);
} // end of _mwProcessReadSocket




////////////////////////////////////////////////////////////////////////////
// _mwProcessWriteSocket
// Process a socket (write)
////////////////////////////////////////////////////////////////////////////
int _mwProcessWriteSocket(HttpParam *hp, HttpSocket* phsSocket)
{
 	if (phsSocket->iDataLength <= 0) 
  {
	  	LogMsg(DBG_INFO, "_mwProcessWriteSocket() : [%d] Data sending completed (%d/%d)", phsSocket->socket, phsSocket->response.iSentBytes, phsSocket->response.iContentLength);
		  return(1);
	 }
	 LogMsg(DBG_INFO, "_mwProcessWriteSocket() : [%d] sending data", phsSocket->socket);

	 if (ISFLAGSET(phsSocket, FLAG_DATA_RAW|FLAG_DATA_STREAM))   
		  return(_mwSendRawDataChunk(hp, phsSocket));	 
  else if (ISFLAGSET(phsSocket, FLAG_DATA_FILE))   
		  return(_mwSendFileChunk(hp, phsSocket));	
  else
		  return(-1);	 
}



////////////////////////////////////////////////////////////////////////////
// _mwCloseSocket
// Close an open connection
////////////////////////////////////////////////////////////////////////////
void _mwCloseSocket(HttpParam* hp, HttpSocket* phsSocket)
{
 	if (phsSocket->fd > 0) 
  {
    LogMsg(DBG_INFO, "_mwCloseSocket() : 0");
	  	_close(phsSocket->fd);
	 }

 	phsSocket->fd = 0;
	 if (ISFLAGSET(phsSocket,FLAG_TO_FREE) && phsSocket->ptr) 
  {
    LogMsg(DBG_INFO, "_mwCloseSocket() : 1");
		  free(phsSocket->ptr);
		  phsSocket->ptr=NULL;
	 }
#ifdef HTTPPOST
 	if (phsSocket->request.pucPayload) 
  {
		  free(phsSocket->request.pucPayload);
	 }
#endif
 	if (!ISFLAGSET(phsSocket,FLAG_CONN_CLOSE) && phsSocket->iRequestCount<hp->maxReqPerConn) 
  {
    LogMsg(DBG_INFO, "_mwCloseSocket() : 2");
		  _mwInitSocketData(phsSocket);
		  //reset flag bits
	  	phsSocket->iRequestCount++;
	  	phsSocket->tmExpirationTime=time(NULL)+HTTP_KEEPALIVE_TIME;
	  	return;
	 }
  if (phsSocket->socket != 0) 
  {
    LogMsg(DBG_INFO, "_mwCloseSocket() : 3");
  		closesocket(phsSocket->socket);
	 }
  else 
  {
		  LogMsg(DBG_ERROR, "_mwCloseSocket() : [%d] bug: socket=0 (structure: 0x%x", phsSocket->socket,phsSocket);
	 }

 	hp->stats.clientCount--;
 	phsSocket->iRequestCount=0;
 	LogMsg(DBG_ERROR, "_mwCloseSocket() : [%d] socket closed after responded for %d requests",phsSocket->socket,phsSocket->iRequestCount);
 	LogMsg(DBG_INFO, "_mwCloseSocket() : Connected clients: %d", hp->stats.clientCount);
 	phsSocket->socket=0;
} // end of _mwCloseSocket



__inline int _mwStrCopy(char *dest, char *src)
{
	int i;
	for (i=0; src[i]; i++) {
		dest[i]=src[i];
	}
	dest[i]=0;
	return i;
}



#define OPEN_FLAG O_RDONLY|0x8000

////////////////////////////////////////////////////////////////////////////
// _mwStartSendFile
// Setup for sending of a file
////////////////////////////////////////////////////////////////////////////
int _mwStartSendFile(HttpParam* hp, HttpSocket* phsSocket)
{
	struct stat st;
	HttpFilePath hfp;

 LogMsg(DBG_INFO, "_mwStartSendFile(0) : ");

 if (stat(phsSocket->request.pucPath, &st) < 0) 
   strcpy(phsSocket->request.pucPath, "404.html");
 
   


	hfp.pchRootPath = hp->pchWebPath;
	// check type of file requested
	if (!(phsSocket->flags & FLAG_DATA_FD)) 
 {
		hfp.pchHttpPath = phsSocket->request.pucPath;
		mwGetLocalFileName(&hfp);
		// open file
//		phsSocket->fd = _open(hfp.cFilePath, OPEN_FLAG);
  phsSocket->fd = _open(hfp.pchHttpPath, OPEN_FLAG);
	} 
 else 
 {
		strcpy(hfp.cFilePath, phsSocket->request.pucPath);
		hfp.pchExt = strrchr(hfp.cFilePath, '.');

		if (hfp.pchExt) 
    hfp.pchExt++;
	}


	if (phsSocket->fd < 0) 
 {
		char *p;
		int i;

  // file/dir not found
		if (stat(hfp.cFilePath, &st) < 0 || !(st.st_mode & S_IFDIR)) 			
			return -1;
		

		for (p = hfp.cFilePath; *p; p++);
		


		//requesting for directory, first try opening default pages
		*(p++) = SLASH;
		for (i = 0; defaultPages[i]; i++) 
  {
 			strcpy(p,defaultPages[i]);
	 		phsSocket->fd = _open(hfp.cFilePath, OPEN_FLAG);
		 	if (phsSocket->fd > 0) 
      break;
		}

		if (phsSocket->fd <= 0 && (hp->flags & FLAG_DIR_LISTING)) 
  {
			SETFLAG(phsSocket,FLAG_DATA_RAW);
			if (!hfp.fTailSlash) 
			{
				p = phsSocket->request.pucPath;
				while(*p) p++;				//seek to the end of the string
				SETWORD(p,DEFWORD('/',0));	//add a tailing slash
				while(--p>(char*) phsSocket->request.pucPath) 
				{
					if (*p=='/') 
					{
						p++;
						break;
					}
				}
				_mwRedirect(phsSocket,p);
			}
			return _mwStartSendRawData(hp, phsSocket);
		}
		phsSocket->response.fileType = HTTPFILETYPE_HTML;
	} 
	else 
	{
		phsSocket->response.iContentLength = !fstat(phsSocket->fd, &st) ? st.st_size - phsSocket->request.iStartByte : 0;
		if (phsSocket->response.iContentLength <= 0)
			phsSocket->request.iStartByte = 0;
		
		if (phsSocket->request.iStartByte)
			_lseek(phsSocket->fd, phsSocket->request.iStartByte, SEEK_SET);
		
		if (!phsSocket->response.fileType && hfp.pchExt) 
			phsSocket->response.fileType = _mwGetContentType(hfp.pchExt);
		
		// mark if substitution needed
		if (hp->pfnSubst && (phsSocket->response.fileType == HTTPFILETYPE_HTML || phsSocket->response.fileType == HTTPFILETYPE_JS))
			SETFLAG(phsSocket,FLAG_SUBST);
	}


	// build http header
	phsSocket->iDataLength = _mwBuildHttpHeader(hp, phsSocket, st.st_mtime, phsSocket->pucData);

	phsSocket->response.iSentBytes =- phsSocket->iDataLength;
	hp->stats.fileSentCount++;

	return 0;
}




////////////////////////////////////////////////////////////////////////////
// _mwSendFileChunk
// Send a chunk of a file
////////////////////////////////////////////////////////////////////////////
int _mwSendFileChunk(HttpParam *hp, HttpSocket* phsSocket)
{
 	int iBytesWritten;
	 int iBytesRead;

 	// send a chunk of data
 	iBytesWritten = send(phsSocket->socket, phsSocket->pucData,phsSocket->iDataLength, 0);
 	if (iBytesWritten <= 0) 
 	{
		  // close connection
  		LogMsg(DBG_INFO, "_mwSendFileChunk() : [%d] error sending data (Sys error no : %d)", phsSocket->socket, GetLastError());
		  SETFLAG(phsSocket,FLAG_CONN_CLOSE);
  		_close(phsSocket->fd);
		  phsSocket->fd = 0;
  		return(-1);
	 }

 	phsSocket->response.iSentBytes+=iBytesWritten;
 	phsSocket->pucData+=iBytesWritten;
 	phsSocket->iDataLength-=iBytesWritten;
 	LogMsg(DBG_INFO, "_mwSendFileChunk() : [%d] sent %d bytes of %d", phsSocket->socket,phsSocket->response.iSentBytes, phsSocket->response.iContentLength);
 	// if only partial data sent just return wait the remaining data to be sent next time
 	if (phsSocket->iDataLength>0)
    return(0);

 	// used all buffered data - load next chunk of file
 	phsSocket->pucData=phsSocket->buffer;
 	if (phsSocket->fd > 0)
	  	iBytesRead = _read(phsSocket->fd, phsSocket->buffer, HTTP_BUFFER_SIZE);
	 else
		  iBytesRead = 0;

 	if (iBytesRead <= 0) 
 	{
	  	// finished with a file
	  	int iRemainBytes=phsSocket->response.iContentLength-phsSocket->response.iSentBytes;
	  	LogMsg(DBG_INFO, "_mwSendFileChunk() : [%d] EOF reached",phsSocket->socket);

  		if (iRemainBytes > 0)
	  	{
		   	if (iRemainBytes > HTTP_BUFFER_SIZE) 
			    	iRemainBytes = HTTP_BUFFER_SIZE;

  		 	LogMsg(DBG_INFO, "_mwSendFileChunk() : Sending %d padding bytes",iRemainBytes);
		   	ZeroMemory(phsSocket->buffer, iRemainBytes);
		 	  phsSocket->iDataLength = iRemainBytes;
			   return(0);
		  }
  		else
		  {
		 	  LogMsg(DBG_INFO, "_mwSendFileChunk() : Closing file (fd=%d)", phsSocket->fd);
  		 	hp->stats.fileSentBytes += phsSocket->response.iSentBytes;
		   	
      if (phsSocket->fd > 0)
        _close(phsSocket->fd);

		   	phsSocket->fd = 0;
		 	  return(1);
		  }
	 }

 	if (ISFLAGSET(phsSocket,FLAG_SUBST)) 
 	{
  		int iBytesUsed;
	  	// substituted file - read smaller chunk
		  phsSocket->iDataLength = _mwSubstVariables(hp, phsSocket->buffer, iBytesRead, &iBytesUsed);
		  if (iBytesUsed<iBytesRead)
			   _lseek(phsSocket->fd,iBytesUsed-iBytesRead, SEEK_CUR);
	 }
 	else
 	{
	  	phsSocket->iDataLength=iBytesRead;
	 }

	 return(0);
} 



////////////////////////////////////////////////////////////////////////////
// _mwStartSendRawData
// Start sending raw data blocks
////////////////////////////////////////////////////////////////////////////
int _mwStartSendRawData(HttpParam *hp, HttpSocket* phsSocket)
{
 	unsigned char header[HTTP200_HDR_EST_SIZE];
 	int offset = 0;
  int hdrsize;
  int bytes;
  LogMsg(DBG_INFO, "_mwStartSendRawData(0) : ");

 	hdrsize = _mwBuildHttpHeader(hp, phsSocket, time(NULL), header);

 	// send http header
 	do 
  {
  		bytes = send(phsSocket->socket, header + offset, hdrsize-offset, 0);
    LogMsg(DBG_INFO, "_mwStartSendRawData(1) : bytes : %d", bytes);

		  if (bytes <= 0) 
      break;

  		offset += bytes;
 	} 
  while (offset < hdrsize);

 	if (bytes <= 0) 
  {
		// failed to send header (socket may have been closed by peer)
		  LogMsg(DBG_MEDIUM, "_mwStartSendRawData() : Failed to send header");
  		return(-1);
	 }

 	return(0);
}

////////////////////////////////////////////////////////////////////////////
// _mwSendRawDataChunk
// Send a chunk of a raw data block
////////////////////////////////////////////////////////////////////////////
int _mwSendRawDataChunk(HttpParam *hp, HttpSocket* phsSocket)
{
	int  iBytesWritten;

    // send a chunk of data
	if (phsSocket->iDataLength==0) {
		if (ISFLAGSET(phsSocket,FLAG_DATA_STREAM) && phsSocket->ptr) {
			//FIXME: further implementation of FLAG_DATA_STREAM case
		}
		hp->stats.fileSentBytes+=phsSocket->response.iSentBytes;
		return 1;
	}
	iBytesWritten=(int)send(phsSocket->socket, phsSocket->pucData,phsSocket->iDataLength, 0);
    if (iBytesWritten<=0) {
		// failure - close connection
		LogMsg(DBG_INFO, "_mwSendRawDataChunk() : Connection closed");
		SETFLAG(phsSocket,FLAG_CONN_CLOSE);
		return -1;
    } else {
		LogMsg(DBG_INFO, "_mwSendRawDataChunk() : [%d] sent %d bytes of raw data", phsSocket->socket, iBytesWritten);
		phsSocket->response.iSentBytes+=iBytesWritten;
		phsSocket->pucData+=iBytesWritten;
		phsSocket->iDataLength-=iBytesWritten;
	}
	if (phsSocket->iDataLength<=0 && 
			ISFLAGSET(phsSocket,FLAG_DATA_STREAM) &&
			phsSocket->response.iSentBytes<phsSocket->response.iContentLength) {
		//load next chuck of raw data
		UrlHandlerParam uhp;
		memset(&uhp,0,sizeof(UrlHandlerParam));
		uhp.hp=hp;
		uhp.iContentBytes=phsSocket->response.iContentLength;
		uhp.iSentBytes=phsSocket->response.iSentBytes;
		uhp.pucBuffer=phsSocket->buffer;
		uhp.iDataBytes=HTTP_BUFFER_SIZE;
		if ((*(PFNURLCALLBACK)(phsSocket->ptr))(&uhp)) {
			phsSocket->pucData=uhp.pucBuffer;
			phsSocket->iDataLength=uhp.iDataBytes;
		}
	}
	return 0;
} // end of _mwSendRawDataChunk





////////////////////////////////////////////////////////////////////////////
// _mwRedirect
// Setup for redirect to another file
////////////////////////////////////////////////////////////////////////////
void _mwRedirect(HttpSocket* phsSocket, char* pchPath)
{
	char* path;
	// raw (not file) data send mode
	SETFLAG(phsSocket,FLAG_DATA_RAW);

	// messages is HTML
	phsSocket->response.fileType=HTTPFILETYPE_HTML;

	// build redirect message
	LogMsg(DBG_INFO, "_mwRedirect() : [%d] Http redirection to %s", phsSocket->socket, pchPath);
	path = (pchPath == phsSocket->pucData) ? _strdup(pchPath) : pchPath;
	phsSocket->iDataLength=sprintf(phsSocket->pucData,HTTPBODY_REDIRECT,path);
	phsSocket->response.iContentLength=phsSocket->iDataLength;
	if (path != pchPath) free(path);
} // end of _mwRedirect







////////////////////////////////////////////////////////////////////////////
// _mwSubstVariables
// Perform substitution of variables in a buffer
// returns new length
////////////////////////////////////////////////////////////////////////////
int _mwSubstVariables(HttpParam* hp, char* pchData, int iLength, int* piBytesUsed)
{
	int lastpos,pos=0,used=0;
	SubstParam sp;
	int iValueBytes;
 LogMsg(DBG_INFO, "_mwSubstVariables() : _SubstVariables input len %d", iLength);
	iLength--;
	for(;;) {
		lastpos=pos;
		for (; pos<iLength && *(WORD*)(pchData+pos)!=HTTP_SUBST_PATTERN; pos++);
		used+=(pos-lastpos);
		if (pos==iLength) {
			*piBytesUsed=used+1;
			return iLength+1;
		}
		lastpos=pos;
		for (pos=pos+2; pos<iLength && *(WORD*)(pchData+pos)!=HTTP_SUBST_PATTERN; pos++);
		if (pos==iLength) {
			*piBytesUsed=used;
			return lastpos;
		}
		pos+=2;
		used+=pos-lastpos;
		pchData[pos-2]=0;
		sp.pchParamName=pchData+lastpos+2;
		sp.iMaxValueBytes=pos-lastpos;
		sp.pchParamValue=pchData+lastpos;
		iValueBytes=hp->pfnSubst(&sp);
		if (iValueBytes>=0) {
			if (iValueBytes>sp.iMaxValueBytes) iValueBytes=sp.iMaxValueBytes;
			memmove(pchData+lastpos+iValueBytes,pchData+pos,iLength-pos);
			iLength=iLength+iValueBytes-(pos-lastpos);
			pos=lastpos+iValueBytes;
		} else {
			LogMsg(DBG_INFO, "_mwSubstVariables() : No matched variable for %s", sp.pchParamName);
			pchData[pos-2]='$';
		}
	}
} // end of _mwSubstVariables



////////////////////////////////////////////////////////////////////////////
// _mwDecodeCharacter
// Convert and individual character
////////////////////////////////////////////////////////////////////////////
__inline char _mwDecodeCharacter(char* s)
{
  	register unsigned char v;
	if (!*s) return 0;
	if (*s>='a' && *s<='f')
		v = *s-('a'-'A'+7);
	else if (*s>='A' && *s<='F')
		v = *s-7;
	else
		v = *s;
	if (*(++s)==0) return v;
	v <<= 4;
	if (*s>='a' && *s<='f')
		v |= (*s-('a'-'A'+7)) & 0xf;
	else if (*s>='A' && *s<='F')
		v |= (*s-7) & 0xf;
	else
		v |= *s & 0xf;
	return v;
} // end of _mwDecodeCharacter

////////////////////////////////////////////////////////////////////////////
// _mwDecodeString
// This function converts URLd characters back to ascii. For example
// %3A is '.'
////////////////////////////////////////////////////////////////////////////
void _mwDecodeString(char* pchString)
{
  int bEnd=FALSE;
  char* pchInput=pchString;
  char* pchOutput=pchString;

  do {
    switch (*pchInput) {
    case '%':
      if (*(pchInput+1)=='\0' || *(pchInput+2)=='\0') {
        // something not right - terminate the string and abort
        *pchOutput='\0';
        bEnd=TRUE;
      } else {
        *pchOutput=_mwDecodeCharacter(pchInput+1);
        pchInput+=3;
      }
      break;
/*
    case '+':
      *pchOutput=' ';
      pchInput++;
      break;
*/
    case '\0':
      bEnd=TRUE;
      // drop through
    default:
      // copy character
      *pchOutput=*pchInput;
      pchInput++;
    }
    pchOutput++;
  } while (!bEnd);
} // end of _mwDecodeString




int _mwGetContentType(char *pchExtname)
{
 	// check type of file requested
 	if (pchExtname[1]=='\0') 
  {
		  return HTTPFILETYPE_OCTET;
 	}
  else if (pchExtname[2]=='\0') 
  {
  		switch (GETDWORD(pchExtname) & 0xffdfdf) 
    {
		    case FILEEXT_JS: return HTTPFILETYPE_JS;
  		}
 	}
  else if (pchExtname[3]=='\0' || pchExtname[3]=='?') 
  {
  		//identify 3-char file extensions
		  switch (GETDWORD(pchExtname) & 0xffdfdfdf) 
    {
    		case FILEEXT_HTM:	return HTTPFILETYPE_HTML;
	    	case FILEEXT_XML:	return HTTPFILETYPE_XML;
    		case FILEEXT_TEXT:	return HTTPFILETYPE_TEXT;
    		case FILEEXT_CSS:	return HTTPFILETYPE_CSS;
    		case FILEEXT_PNG:	return HTTPFILETYPE_PNG;
    		case FILEEXT_JPG:	return HTTPFILETYPE_JPEG;
    		case FILEEXT_GIF:	return HTTPFILETYPE_GIF;
    		case FILEEXT_SWF:	return HTTPFILETYPE_SWF;
	    	case FILEEXT_MPA:	return HTTPFILETYPE_MPA;
	    	case FILEEXT_MPG:	return HTTPFILETYPE_MPEG;
	    	case FILEEXT_AVI:	return HTTPFILETYPE_AVI;
	    	case FILEEXT_MP4:	return HTTPFILETYPE_MP4;
	    	case FILEEXT_MOV:	return HTTPFILETYPE_MOV;
		  }
	 } 
  else if (pchExtname[4]=='\0' || pchExtname[4]=='?') 
  {
  		//logic-and with 0xdfdfdfdf gets the uppercase of 4 chars
		  switch (GETDWORD(pchExtname)&0xdfdfdfdf) 
    {
    		case FILEEXT_HTML:	return HTTPFILETYPE_HTML;
		    case FILEEXT_MPEG:	return HTTPFILETYPE_MPEG;
	  	}
	 }
 	return(HTTPFILETYPE_OCTET);
}

int _mwGrabToken(char *pchToken, char chDelimiter, char *pchBuffer, int iMaxTokenSize)
{
	char *p=pchToken;
	int iCharCopied=0;

	while (*p && *p!=chDelimiter && iCharCopied<iMaxTokenSize) {
		*(pchBuffer++)=*(p++);
		iCharCopied++;
	}
	pchBuffer='\0';
	return (*p==chDelimiter)?iCharCopied:0;
}

int _mwParseHttpHeader(HttpSocket* phsSocket)
{
	int iLen;
	char buf[12];
	char *p=phsSocket->buffer;		//pointer to header data
	HttpRequest *req=&phsSocket->request;

	//analyze rest of the header
	for(;;) {
		//look for a valid field name
		while (*p && *p!='\r') p++;		//travel to '\r'
		if (!*p || GETDWORD(p)==HTTP_HEADEREND) break;
		p+=2;							//skip "\r\n"
		switch (*(p++)) {
		case 'C':
			if (!memcmp(p,"onnection: ",11)) {
				p+=11;
				if (!memcmp(p,"close",5)) {
					SETFLAG(phsSocket,FLAG_CONN_CLOSE);
					p+=5;
				}
			} else if (!memcmp(p,"ontent-Length: ",15)) {
				p+=15;
				p+=_mwGrabToken(p,'\r',buf,sizeof(buf));
				phsSocket->response.iContentLength=atoi(buf);
			}
			break;
		case 'R':
			if (!memcmp(p,"eferer: ",8)) {
				p+=8;
				phsSocket->request.ofReferer=(int)p-(int)phsSocket->buffer;
			} else if (!memcmp(p,"ange: bytes=",12)) {
				p+=12;
				iLen=_mwGrabToken(p,'-',buf,sizeof(buf));
				if (iLen==0) continue;
				p+=iLen;
				phsSocket->request.iStartByte=atoi(buf);
				iLen=_mwGrabToken(p,'/',buf,sizeof(buf));
				if (iLen==0) continue;
				p+=iLen;
                phsSocket->response.iContentLength=atoi(buf)-phsSocket->request.iStartByte+1;
			}
			break;
		case 'H':
			if (!memcmp(p,"ost: ",5)) {
				p+=5;
				phsSocket->request.ofHost=(int)p-(int)phsSocket->buffer;
			}
			break;
		}
	}
	return 0;					//end of header
}
//////////////////////////// END OF FILE ///////////////////////////////////
