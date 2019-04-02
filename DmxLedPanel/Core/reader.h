#pragma once

#include <artnet/artnet.h>

namespace core_artnet {

	class Reader {
	private:

		artnet_node node;

		static int dmx_handler(artnet_node node, int prt, void * d);


	public:
		Reader(const char * ip, int verbose);

		void start();

		void stop();

		void add_dmx_packet_handler();
	};

	
}
