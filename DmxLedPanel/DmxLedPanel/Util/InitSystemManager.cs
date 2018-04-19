using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Util
{
    public class InitSystemManager
    {
        private static InitSystemManager instance;

        private List<ISystemInitializer> initializers;

        private InitSystemManager() {
            initializers = new List<ISystemInitializer>();
        }

        public static InitSystemManager Instance {
            get {
                if (instance == null) {
                    instance = new InitSystemManager();
                }
                return instance;
            }
        }

        public void InitSystem() {
            foreach (ISystemInitializer i in initializers) {
                i.InitSystem();
            }
        }

        public void AddInitializer(ISystemInitializer i) {
            initializers.Add(i);
        }

        public void RemoveInitializer(ISystemInitializer i) {
            initializers.Remove(i);
        }

        public void ClearInitializers() {
            initializers.Clear();
        }
    }
}
