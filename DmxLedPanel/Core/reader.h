#pragma once

#include "artnet/artnet.h" 

namespace core_artnet {

	class Reader {
	private:

		artnet_node base_node;
		void initOutputPorts(artnet_node node, int portCount);

	public:
		static int dmx_handler(artnet_node node, int packet, void * d);
		Reader(const char * ip, int verbose);
		void start();
		void stop();
		void add_dmx_packet_handler();
		void check_start_result(int start_result);
		
	};

	
}
