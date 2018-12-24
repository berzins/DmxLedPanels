using DmxLedPanel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DmxLedPanel.State
{
    public class AutoSave
    {
        private static readonly int MAX_BACKUPS = 20;

        private int fileIndex = 0;

        public void RunIfEnabled() {
            if (isAutoSaveEnabled()) {
                Run();
            }
        }

        public void Run() {
            var interval = SettingManager.Instance.Settings.AutoSave;
            var timer = new Timer(interval * 60 * 1000);
            timer.AutoReset = true;
            timer.Elapsed += new ElapsedEventHandler(Save);
            timer.Start();
        }

        private void Save(object sender, ElapsedEventArgs args) {
            try
            {
                var filename = SettingManager.Instance.Settings.CurrentProject.Replace(".json", string.Empty).ToLower();
                StateManager.Instance.SaveStateBackup(filename, getNextFileIndex());
                Talker.Talker.Log(new Talker.ActionMessage() {
                    Message = "'" + filename + "' backup saved",
                    Level = Talker.LogLevel.INFO,
                    Source = Talker.Talker.GetSource()
                });
            }
            catch (Exception e) {
                Talker.Talker.Log(new Talker.ActionMessage() {
                    Message = e.ToString(),
                    Level = Talker.LogLevel.ERROR,
                    Source = Talker.Talker.GetSource()
                });
            }
            
        }

        private int getNextFileIndex() {
            if (fileIndex >= MAX_BACKUPS)
            {
                fileIndex = 0;
            }
            return fileIndex++;
        }

        private bool isAutoSaveEnabled() {
            return SettingManager.Instance.Settings.AutoSave > 0;
        }
    }
}
