#ifndef __NETPACKET__
#define __NETPACKET__


#define IP_HL(ip) (((ip)->u8_ip_vhl) & 0x0f)
#define IP_V(ip)  (((ip)->u8_ip_vhl) >> 4)

#define ETHERTYPE_ARP 0x0806
#define ETHERTYPE_IP 0x0800

#define IP_ICMP 1
#define IP_TCP 6
#define IP_UDP 17



typedef struct ethern_hdr
{
  unsigned char ether_dhost[6];  // dest Ethernet address
  unsigned char ether_shost[6];  // source Ethernet address
  unsigned short ether_type;     // protocol (16-bit)
} ETHDR, *PETHDR;



typedef struct ipaddress
{
  unsigned char byte1;
  unsigned char byte2;
  unsigned char byte3;
  unsigned char byte4;
} IPADDRESS, *PIPADDRESS;

typedef struct iphdr
{
  unsigned char  ver_ihl;        // Version (4 bits) + Internet header length (4 bits)
  unsigned char  tos;            // Type of service 
  unsigned short tlen;           // Total length 
  unsigned short identification; // Identification
  unsigned short flags_fo;       // Flags (3 bits) + Fragment offset (13 bits)
  unsigned char  ttl;            // Time to live
  unsigned char  proto;          // Protocol
  unsigned short crc;            // Header checksum
  IPADDRESS      saddr;      // Source address
  IPADDRESS      daddr;      // Destination address
  unsigned int   opt;        // Option + padding
} IPHDR, *PIPHDR;

typedef struct tcphdr 
{
  unsigned short sport;  
  unsigned short dport;
  unsigned int   seq; 
  unsigned int   ack_seq; 
  unsigned short res1:4, 
                 doff:4,
                 fin:1,
                 syn:1,  
                 rst:1,  
                 psh:1,  
                 ack:1,  
                 urg:1, 
                 res2:2; 
  unsigned short window;
  unsigned short check;  
  unsigned short urg_ptr;
} TCPHDR, *PTCPHDR; 


typedef struct udphdr 
{
  unsigned short sport;/*Source port */
  unsigned short dport;/*Destination port */
  unsigned short ulen;/*UDP length */
  unsigned short sum; /*UDP checksum */
} UDPHDR, *PUDPHDR;

#endif
