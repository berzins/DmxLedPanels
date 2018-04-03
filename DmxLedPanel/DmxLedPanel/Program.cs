using ArtNet.ArtPacket;
using System;
using System.Collections.Generic;
using System.Linq;
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
            IPixelPatch pixelPatch = new PixelPatchSnakeColumnWiseTopLeft(9, 9, 0, 3);
            var patch = pixelPatch.GetPixelPatch();
            for (int row = 0; row < 9; row++) {
                for (int col = 0; col < 9; col++) {
                    Pixel pix = patch[col, row];
                    Console.Write( (pix.Index < 10 ? "0" : "") + pix.Index + " ");                  
                }
                Console.WriteLine();
            }


            IMode mode = new ModeLineWiseTopLeft(3, 3);
            List<Field> fields = mode.GetFields(patch);

            Console.WriteLine();

            foreach (Field f in fields) {
                foreach (Pixel pix in f.Pixels) {
                    Console.Write(f.Index + "/" + (pix.Index < 10 ? "0" : "") + pix.Index + " ");
                }
                Console.WriteLine();
            }


            Fixture fixture = new Fixture(mode, pixelPatch);
            fixture.Address = new Address() { DmxAddress = 0, Port = new Port(0,0,0)};


            int[] dmxData = Enumerable.Repeat(7, 81).ToArray();
             
            for (int i = 3; i < dmxData.Length; i++) {
                dmxData[i] = 0;
            }
            
            ArtDmxPacket packet = new ArtDmxPacket();
            packet.DmxData = dmxData.Select(x => (byte)x).ToArray();
            ((IDmxPacketHandler)fixture).HandlePacket(packet);


            patch = fixture.PixelPatch.GetPixelPatch();
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
