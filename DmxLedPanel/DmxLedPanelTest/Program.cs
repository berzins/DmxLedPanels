using ArtNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanelTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.run();

        }

        public void run() {

            ArtNetWritter artOut = new ArtNetWritter(IPAddress.Parse("127.0.0.1"));




        }
    }



}
