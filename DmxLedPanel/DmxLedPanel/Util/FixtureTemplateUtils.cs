using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Util
{
    class FixtureTemplateUtils
    {

        public static readonly String FIXTURE_TEMPLATE_PATH = "templates\\fixture\\";
        
        public static void StoreTemplate(FixtureTemplate f) {
            var path = FIXTURE_TEMPLATE_PATH + f.Name + ".json";
            FileIO.WriteFile(FIXTURE_TEMPLATE_PATH + FormatNameForWritting(f.Name) + ".json", true, StaticSerializer.Serialize(f));
        }
        
        internal static List<FixtureTemplate> GetTemplates()
        {
            var files = FileIO.GetFiles(FIXTURE_TEMPLATE_PATH, true, false, ".json");
            var templates = files.Select((f) => {
                var str = FileIO.ReadFile(FIXTURE_TEMPLATE_PATH + f, true);
                return JsonConvert.DeserializeObject<FixtureTemplate>(str);
            });
            return new List<FixtureTemplate>(templates);
        }

        public static string FormatNameForWritting(string name) {
            var i = name.LastIndexOf(" ");
            if (i < 0) return name;
            return name.Substring(0, i).Replace(" ", "_");
        }

    }
}
