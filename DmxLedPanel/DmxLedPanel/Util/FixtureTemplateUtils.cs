using DmxLedPanel.Modes;
using DmxLedPanel.PixelPatching;
using DmxLedPanel.Template;
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
            List<FixtureTemplate> templateList = new List<FixtureTemplate>(templates);
            if (templateList.Count == 0) {
                templateList = GenerateDefaultTemplates();

                Talker.Talk.Log(new Talker.ActionMessage()
                {
                    Message = "No fixture templates were found. Defaults generated successfully.",
                    Source = Talker.Talk.GetSource(),
                    Level = Talker.LogLevel.WARNING
                });
            }
            return templateList;
        }

        public static string FormatNameForWritting(string name) {
            var i = name.LastIndexOf(" ");
            if (i < 0) return name;
            return name.Substring(0, i).Replace(" ", "_");
        }

        // currently this generae only one default 1pix template.... 
        public static List<FixtureTemplate> GenerateDefaultTemplates() {
            List<FixtureTemplate> templates = new List<FixtureTemplate>();
            IMode mode = new ModeGridTopLeft(1, 1);
            List<IMode> modes = new List<IMode>();
            modes.Add(mode);
            IPixelPatch patch = new PixelPatchSnakeRowWiseTopLeft(1, 1, 0, 3);
            Fixture f = new Fixture(modes, patch) {
                Name = "Generic 1pix"
            };
            FixtureTemplate template = FixtureTemplateFactory.createFixtureTemplate(f);
            templates.Add(template);
            FixtureTemplateUtils.StoreTemplate(template);
            return templates;
        }

    }
}
