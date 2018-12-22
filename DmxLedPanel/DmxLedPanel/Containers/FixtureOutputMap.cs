using DmxLedPanel.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Containers
{
    public class FixtureOutputMap
    {

        public FixtureOutputMap(int id, Output o, Fixture f = null)
        {
            FixtureId = id;
            Output = o;
            Fixture = f;
        }

        public int FixtureId { get; set; } // Keep this for backward compatability
        public Output Output { get; set; }
        public Fixture Fixture { get; set; }

        public static List<Fixture> GetFixtures(List<FixtureOutputMap> map) {
            var fixtures = new HashSet<Fixture>();
            foreach (FixtureOutputMap fom in map) {
                if (fom.Fixture != null)
                {
                    fixtures.Add(fom.Fixture);
                }
                else {
                    if (StateManager.Instance.State.TryGetFixture(fom.FixtureId, out Fixture f))
                    {
                        fixtures.Add(f);
                    }
                }
            }
            return fixtures.ToList();
        }

        public static List<Output> GetOutputs(List<FixtureOutputMap> map) {
            var outputs = new List<Output>();
            foreach (var fom in map) {
                outputs.Add(fom.Output);
            }
            return outputs;
        } 
    }
}
