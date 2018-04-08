using ArtNet;
using ArtNet.ArtPacket;
using DmxLedPanel.Modes;
using DmxLedPanel.RestApi;
using DmxLedPanel.Util;
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

            //// crate fixtures

            //int fixtureCount = 4;
            //List<Fixture> fixtures = new List<Fixture>();
            //int startAddress = 0;
            //for (int i = 0; i < fixtureCount; i++)
            //{
            //    Fixture fixture = new Fixture(
            //        new ModeGridTopLeft(1, 9),
            //        new PixelPatchSnakeColumnWiseTopLeft(9, 9, 0, 3)
            //        );
            //    fixture.Address = new Address() { Port = new Port(0, 0, 2), DmxAddress = startAddress };
            //    startAddress += fixture.InputAddressCount;
            //    fixtures.Add(fixture);
            //}

            //// init output
            //Output output = new Output();
            //output.Ports.Add(new Port(0, 0, 0));
            //output.Ports.Add(new Port(0, 0, 1));
            //foreach (Port p in output.Ports)
            //{
            //    Console.WriteLine("Output port set (" + p.Net + "." + p.SubNet + "." + p.Universe + ")");
            //}

            //foreach (Fixture f in fixtures)
            //{
            //    output.PatchFixture(f);
            //}
            //Console.WriteLine("Output fixtures has been set.");

            //// init input

            //ArtnetIn artin = ArtnetIn.Instance;

            //artin.AddDmxPacketListeners(
            //    fixtures.Select(x => (IDmxPacketHandler)x).ToList());

            //artin.Start();

            //Console.WriteLine("Artnet input enabled");

            //ArtDmxPacket packet = new ArtDmxPacket();
            //for (int i = 0; i < 3; i++)
            //{
            //    packet.DmxData[i] = 7;
            //}


            //// Set Up State

            //State state = new State();
            //state.Outputs.Add(output);
            //state.FixturePool = fixtures;

            ArtNetReader reader = new ArtNetReader();
            reader.GetAvailableBindAddresses();



            //var settings = SettingManager.Instance.Settings.RestApiPort;


            RestApiServer restApi = new RestApiServer();
            restApi.Start();





            //Console.WriteLine(settings.UIHomePath);


            string path = @"D:\ProgrammingProjects\Asound\DmxLedPanels\DmxLedPanel\DmxLedPanel\ConfigFiles\template.json";

            //FileIO.WriteFile(path, ((ISerializable)state).Serialize());

            string serializedState = FileIO.ReadFile(path, false);
            State deserializedState = new State().Deserialize<State>(serializedState);
            ArtnetIn artin = ArtnetIn.Instance;
            List<Fixture> patchedFixtures = deserializedState.GetPatchedFixtures();

            artin.AddDmxPacketListeners(
                patchedFixtures
                .Select(x => (IDmxPacketHandler)x).ToList());
            artin.Start();


            // Manual Pixel check
            int pixel = 0;

            while (true)
            {
                Console.ReadKey();
                //Console.WriteLine("Line read is " + pixel);
                byte[] dmx = Enumerable.Repeat((byte)5, 108).ToArray();
                int start = pixel * 3;
                for (int i = start; i < start + 3; i++)
                {
                    dmx[i] = 200;
                }
                var p = new ArtDmxPacket() { Universe = 2 };
                p.DmxData = dmx;

                ArtnetOut.Instance.Writer.Write(p);

                //foreach (Fixture f in fixtures) {
                //    ((IDmxPacketHandler)f).HandlePacket(p);
                //}
                pixel++;
                if (pixel > 35) pixel = 0;
            }
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
