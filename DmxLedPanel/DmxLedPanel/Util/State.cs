using DmxLedPanel.Template;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Util
{
    public class State : ISerializable
    {
        public List<Output> Outputs { get; set; }
        public List<Fixture> FixturePool { get; set; }

        public State() {
            Outputs = new List<Output>();
            FixturePool = new List<Fixture>();
        }

        public T Deserialize<T>(string str)
        {
            StateTemplate stateTemp = JsonConvert.DeserializeObject<StateTemplate>(str);
            State state = new State();

            // deserialize outputs
            foreach (OutputTemplate o in stateTemp.Outputs) {
                Output output = new Output();
                // add prots
                foreach (Port p in o.Ports) {
                    output.Ports.Add(p);
                }
                // add fixtures
                foreach (FixtureTemplate ft in o.Fixtures) {
                    Fixture f = FixtureFactory.createFixture(ft);
                    f.Name = ft.Name;
                    output.PatchFixture(f);
                }
                output.Name = o.Name;
                state.Outputs.Add(output);
            }

            // deserialize fixture pool
            foreach (FixtureTemplate ft in stateTemp.FixturePool) {
                Fixture f = FixtureFactory.createFixture(ft);
                f.Name = ft.Name;
                state.FixturePool.Add(f);
            }
            return (T) Convert.ChangeType(state, typeof(State));
        }


        public string Serialize()
        {
            StateTemplate temp = new StateTemplate();

            foreach (Output o in Outputs) {
                OutputTemplate ot = new OutputTemplate();
                ot.Ports = o.Ports;
                ot.Name = o.Name;
                foreach (Fixture f in o.GetFixtures()) {
                    ot.Fixtures.Add(FixtureTemplateFactory.createFixtureTemplate(f));
                }
                temp.Outputs.Add(ot);
            }

            foreach (Fixture f in FixturePool) {
                temp.FixturePool.Add(FixtureTemplateFactory.createFixtureTemplate(f));
            }

            return JsonConvert.SerializeObject(temp);
        }

        public List<Fixture> GetPatchedFixtures() {
            List<Fixture> fixtures = new List<Fixture>();
            foreach (Output o in Outputs) {
                foreach (Fixture f in o.GetFixtures()) {
                    fixtures.Add(f);
                }
            }
            return fixtures;
        }
    }
}
