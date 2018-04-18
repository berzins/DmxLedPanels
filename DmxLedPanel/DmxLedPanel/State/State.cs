using DmxLedPanel.Template;
using DmxLedPanel.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.State
{
    public class State : ISerializable
    {
        public List<Output> Outputs { get; set; }
        public List<Fixture> FixturePool { get; set; }

        public int GetLastFixtureId() {
            int last = 0;
            foreach (Fixture f in GetAllFixtures())
            {
                last = f.ID > last ? f.ID : last;
            }
            return last;
        }

        public int GetLastOutputId() {
            int last = 0;
            foreach (Output o in Outputs) {
                last = o.ID > last ? o.ID : last;
            }
            return last;
        }

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
                    output.TryPatchFixture(f);
                }
                output.IP = o.IPAddress;
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
                ot.ID = o.ID;
                ot.Ports = o.Ports;
                ot.Name = o.Name;
                ot.IPAddress = o.IP;
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

        public Output GetOutput(int id)
        {
            foreach (Output o in Outputs) {
                if (o.ID == id) {
                    return o;
                }
            }
            return null;
        }

        public List<Output> GetOutputs(int[] ids) {
            var outs = new List<Output>();
            foreach (var id in ids) {
                var o = GetOutput(id);
                if (o != null) {
                    outs.Add(o);
                }
            }
            return outs;
        }

        public Output GetOutputByFixture(int id) {
            foreach (Output o in Outputs) {
                foreach (Fixture f in o.GetFixtures()) {
                    if (f.ID == id) return o;
                }
            }
            return null;
        }

        public Fixture GetFixtureFromFixturePool(int id) {
            foreach (Fixture f in FixturePool) {
                if (f.ID == id) return f;
            }
            foreach (var o in Outputs) {
                var f = o.GetFixture(id);
                if (f != null) return f;
            }
            return null;
        }

        public List<Fixture> GetFixturesFromFixturePool(int[] ids) {
            List<Fixture> fl = new List<Fixture>();
            foreach (int id in ids) {
                fl.Add(GetFixtureFromFixturePool(id));
            }
            return fl;
        }

        public List<Fixture> GetFixture(int id) {
            var fix = new List<Fixture>();
            foreach (var f in GetAllFixtures()) {
                if (f.ID == id) fix.Add(f);
            }
            return fix;
        }

        public List<Fixture> GetFixtures(int [] ids) {
            var fix = new List<Fixture>();
            foreach (var f in GetAllFixtures()) {
                foreach (var id in ids) {
                    if (f.ID == id) fix.Add(f);
                }
            }
            return fix;
        }

        

        /// <summary>
        /// Removes output with all its content from State Outputs
        /// </summary>
        /// <returns>Removed Output if found.. null if outputs not found</returns>
        public Output RemoveOutput(int id) {
            var o = GetOutput(id);
            if (o != null) {
                Outputs.Remove(o);
                return o;
            }
            return null;
        }

        /// <summary>
        /// Remove fixture from fixture pool
        /// To remove fixtures form outputs firt remove them from outputs.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Fixture RemoveFixtureFromFixturePool(int id) {
            var f = GetFixtureFromFixturePool(id);
            if (f != null) {
                FixturePool.Remove(f);
                return f;
            }
            return null;
        }

        public List<Fixture> RemoveFixturesFromFixturePool(int [] ids) {
            List<Fixture> fixtures = new List<Fixture>();
            foreach (int id in ids) {
                fixtures.Add(RemoveFixtureFromFixturePool(id));
            }
            return fixtures;
        }

        public List<Fixture> RemoveFixturesFromFixturePool(List<Fixture> fixtures) {
            foreach (Fixture f in fixtures) {
                FixturePool.Remove(f);
            }
            return fixtures;
        }


        public bool ContainOutput(int id) {
            return GetOutput(id) != null ? true : false;
        }

        public bool ContainFixture(int id) {
            return GetFixtureFromFixturePool(id) != null ? true : false;
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

        public List<Fixture> GetAllFixtures() {
            List<Fixture> fl = new List<Fixture>();
            fl.AddRange(FixturePool);
            fl.AddRange(GetPatchedFixtures());
            return fl;
        }

        
    }
}
