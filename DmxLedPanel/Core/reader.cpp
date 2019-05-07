//#include <errno.h>
//#include <fcntl.h>
//#include <malloc.h>
//#include <signal.h>
#include <stdio.h>
//#include <stdlib.h>
//#include <string.h>
//#include <time.h>
#include <sys/timeb.h>
#include <WinSock2.h>

#include "pch.h"
#include "reader.h"

#include <iostream>
#include <functional>
#include "artnet/artnet.h"
#include "artnet/common.h"
	
using namespace std;
using namespace std::placeholders;

int core_artnet::Reader::dmx_handler(artnet_node node, int prt, void * d)
{

	artnet_node_config_t * node_conf = new artnet_node_config_t;
	auto result = artnet_get_config(node, node_conf);

	artnet_node_config_t node_confiiiiig = *node_conf;
	cout << (short)node_confiiiiig.subnet << endl;

	cout << "packet received where prt: " << prt << endl;
	delete node_conf;
	
	return 0;
}

core_artnet::Reader::Reader(const char * ip, int verbose)
{
	base_node = artnet_new(ip, verbose);
	artnet_set_short_name(base_node, "artnet-reveiver");
	artnet_set_long_name(base_node, "ArtNet Node");
	artnet_set_node_type(base_node, ARTNET_NODE);

	artnet_set_subnet_addr(base_node, 1);

	// set the first port to output dmx data

	cout << "artnet max pors = " << ARTNET_MAX_PORTS << endl;
	initOutputPorts(base_node, 16);


	if (artnet_set_dmx_handler(base_node, dmx_handler, NULL)) {
		cout << "Setting dmx handler failed.";
	}

	//todo:: configure somthing if necessary. 

}

void core_artnet::Reader::start()
{

	int SUCCESS = 0;
	int start_result = artnet_start(base_node);

	check_start_result(start_result);
	

	int length = 512;

	int c = 0;
	int optc, subnet_addr = 0, port_addr = 0;
	int artnet_sd;


	// store the sds
	artnet_sd = artnet_get_sd(base_node);

	int last_socket_error = 0;
	int artnet_read_result = 0;


	/* main loop */
	c = 0;
	while (c != 'q') {
		int n, max;
		fd_set rd_fds;
		struct timeval tv;

		FD_ZERO(&rd_fds);
		FD_SET(artnet_sd, &rd_fds);
		(artnet_sd, &rd_fds);

		max = artnet_sd;

		tv.tv_sec = 1;
		tv.tv_usec = 0;

		n = select(max + 1, &rd_fds, NULL, NULL, &tv);
		if (n > 0) {
			if (FD_ISSET(artnet_sd, &rd_fds)) {
				artnet_read_result = artnet_read(base_node, 1);
				cout << "paceket_received, read_result: " << artnet_read_result << endl;
			}
		}
		else {
			last_socket_error = WSAGetLastError();
			cout << "socket error, last_socekt_error: " << last_socket_error << endl;
		}
	}
}

void core_artnet::Reader::stop() 
{
	artnet_stop(base_node);
}

void core_artnet::Reader::add_dmx_packet_handler()
{	

	//artnet_set_dmx_handler()
}

void core_artnet::Reader::check_start_result(int start_result) {
	int SUCCESS = 0;
	if (start_result != SUCCESS) {
		cout << "artnet reader start failed with result: " << start_result << endl;
		exit(-1);
	}
	cout << "artnet reader started successfully" << endl;
}

void core_artnet::Reader::initOutputPorts(artnet_node node, int portCount) {
	int result = 0;
	for (int i = 0; i < portCount; i++) {
		result = artnet_set_port_type(node, i, ARTNET_ENABLE_OUTPUT, ARTNET_PORT_DMX);
		if (result != 0) {
			cout << artnet_strerror() << endl;
		}
		result = artnet_set_port_addr(node, i, ARTNET_OUTPUT_PORT, i);
		if (result != 0) {
			cout << artnet_strerror() << endl;
		}
	}
}

