using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Fixtures
{
    /// <summary>
    /// Describes DMX controllable property
    /// </summary>
    public abstract class FixtureDmxUtil : IDmxUtlHandler
    {

        public static readonly string UTIL_DEFAULT_NAME = "DMX_UTL_NAME_NOT_SET";

        protected int[] values;
        protected int addressCount;

        public FixtureDmxUtil(int index, int addressCount)
        {
            this.addressCount = addressCount;
            values = new int[addressCount];
            this.Index = index;
            this.Name = UTIL_DEFAULT_NAME;
        }

        // copy constructor
        public FixtureDmxUtil(int index, string name, int[] values) : this(index, values.Length) {
            //Index = index;
            //Name = name;
            Values = values;
        }

        public int Index { get; set; }
        public String Name { get; set; }
        public int [] Values {

            get {
                return this.values;
            }

            set {
                try
                {
                    if (values.Length != value.Length) {
                        throw new ArgumentException("Provided address count does not match acctual dmx util address count");
                    }
                    values = Utils.cloneArray(value);
                }
                catch (ArgumentException e) {
                    Talker.Talk.Log(new Talker.ActionMessage()
                    {
                        Message = e.Message + ". " + e.StackTrace,
                        Source = Talker.Talk.GetSource(),
                        Level = Talker.LogLevel.ERROR
                    });
                }
            }
        }

        public abstract void HandleDmx(Fixture f, int [] values);
        public abstract FixtureDmxUtil Clone();

    }
}
