//#include <errno.h>
//#include <fcntl.h>
//#include <malloc.h>
//#include <signal.h>
#include <stdio.h>
//#include <stdlib.h>
//#include <string.h>
//#include <time.h>
#include <sys/timeb.h>
//#include <WinSock2.h>

#include "pch.h"
#include "reader.h"

#include <iostream>
#include <functional>
	
using namespace std;
using namespace std::placeholders;

int core_artnet::Reader::dmx_handler(artnet_node node, int prt, void * d)
{
	cout << "packet received where prt: " << prt << endl;
	artnet_read(node, 0);
	return 0;
}

core_artnet::Reader::Reader(const char * ip, int verbose)
{
	node = artnet_new(ip, verbose);
	artnet_set_short_name(node, "artnet-reveiver");
	artnet_set_long_name(node, "ArtNet Node");
	artnet_set_node_type(node, ARTNET_NODE);
	artnet_set_port_type(node, 0, ARTNET_ENABLE_OUTPUT, ARTNET_PORT_DMX);
	artnet_set_port_addr(node, 0, ARTNET_OUTPUT_PORT, 0);
	artnet_set_subnet_addr(node, 0);

	artnet_set_dmx_handler(node, dmx_handler, NULL);

	//todo:: configure somthing if necessary. 

}

void core_artnet::Reader::start()
{
	artnet_start(node);


	int c = 0;
	int optc, subnet_addr = 0, port_addr = 0;
	int artnet_sd;


	// store the sds
	artnet_sd = artnet_get_sd(node);


	/* main loop */
	c = 0;
	while (c != 'q') {
		int n, max;
		fd_set rd_fds;
		struct timeval tv;

		FD_ZERO(&rd_fds);
		FD_SET(0, &rd_fds);
		(artnet_sd, &rd_fds);

		max = artnet_sd;

		tv.tv_sec = 1;
		tv.tv_usec = 0;

		//n = select(max + 1, &rd_fds, NULL, NULL, &tv);
		//if (n > 0) {
			//if (FD_ISSET(artnet_sd, &rd_fds))
				artnet_read(node, 0);
		//}
	}
}

void core_artnet::Reader::stop() 
{
	artnet_stop(node);
}

void core_artnet::Reader::add_dmx_packet_handler()
{	

	//artnet_set_dmx_handler()
}
