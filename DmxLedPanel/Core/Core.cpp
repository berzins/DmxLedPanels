// Core.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "pch.h"
#include <iostream>
#include <artnet/artnet.h>
#include "reader.h"

using namespace std;
using namespace core_artnet;

typedef unsigned char dmx_t;

int main()
{
	const char * ip_addr_wifi = "192.168.0.101";
	//const char * ip_addr_loopback = "192.168.0.50";
	const char * ip_addr_loopback = "169.254.233.187";


	Reader * reader = new Reader(ip_addr_wifi, 1);
	reader->start();



	//dmx_t * dmx = new dmx_t[512];
	//

	//artnet_node node_slave;
	//artnet_node node_controller;

	



	//// ---- init controller node
	//if ((node_controller = artnet_new(ip_addr_loopback, 1)) == NULL) {
	//	printf("new failed %s\n", artnet_strerror());
	//}
	//else {
	//	cout << "magic has happened" << endl;
	//}

	//artnet_set_short_name(node_controller, "artnet-discovery");
	//artnet_set_long_name(node_controller, "ArtNet Discovery Node");
	//artnet_set_node_type(node_controller, ARTNET_RAW);

	//// start
	//if (artnet_start(node_controller) != ARTNET_EOK) {
	//	printf("Failed to start: %s\n", artnet_strerror());
	//}



	//// ---- init slave node
	//if ((node_slave = artnet_new(ip_addr_wifi, 1)) == NULL) {
	//	printf("new failed %s\n", artnet_strerror());
	//}
	//else {
	//	cout << "magic has happened" << endl;
	//}

	//artnet_set_short_name(node_slave, "artnet-reveiver");
	//artnet_set_long_name(node_slave, "ArtNet Node");
	//artnet_set_node_type(node_slave, ARTNET_NODE);

	//// start
	//if (artnet_start(node_slave) != ARTNET_EOK) {
	//	printf("Failed to start: %s\n", artnet_strerror());
	//}

	//artnet_raw_send_dmx(node_controller, (uint8_t)57, 512, dmx);






	//// ---------------

	//if (artnet_send_poll(node_controller, NULL, ARTNET_TTM_DEFAULT) != ARTNET_EOK) {
	//	printf("send poll failed\n");
	//}
	//else {
	//	printf("art poll sent successfully");
	//}


}
