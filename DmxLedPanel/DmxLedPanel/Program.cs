using ArtNet;
using ArtNet.ArtPacket;
using DmxLedPanel.Modes;
using DmxLedPanel.RestApi;
using DmxLedPanel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using DmxLedPanel.State;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
            Console.ReadLine();
        }

        private void Run() {


            ArtNetReader reader = new ArtNetReader();
            reader.GetAvailableBindAddresses();

            RestApiServer restApi = new RestApiServer();
            restApi.Start();

            StateManager.Instance.LoadState(StateManager.DEFAULT_STATE_FILE);
            ArtnetIn.Instance.Start();

        }

        void printFixturePatch(Fixture fixture) {
            var patch = fixture.PixelPatch.GetPixelPatch();
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Pixel pix = patch[col, row];
                    Console.Write((pix.Index < 10 ? "0" : "") + pix.Index + "/" + pix.DmxValues[0] + "." + pix.DmxValues[1] + "." + pix.DmxValues[2] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
