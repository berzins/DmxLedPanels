using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talker;

namespace DmxLedPanel.RestApi.Log
{
    public class MessageLogger
    {

        protected static readonly int IS_NOT_PART_OF_STATE = 1;
        protected static readonly int IS_PART_OF_STATE = 2;

        public ActionMessage Message { get; private set; } = new ActionMessage();
        private int State { get; set; }

        public void SetInfoMessage(string msg, int state, string source) {
            SetMessage(msg, source, Talker.LogLevel.INFO, state);

        }

        public void SetWarningMessage(string msg, int state, string source) {
            SetMessage(msg, source, Talker.LogLevel.WARNING, state);
        }

        public void SetErrorMessage(string msg, int state, string source)
        {
            SetMessage(msg, source, Talker.LogLevel.ERROR, state);
        }

        public void SetMessage(string msg, int level, int state, string source) {
            SetMessage(msg, source, level, state);
        }

        private void SetMessage(string msg, string source, int level, int state) {
            Message.Message = msg;
            Message.Source = source;
            Message.Level = level;
            State = state;
        }        
        public void Log() {
                Talker.Talk.Log(Message);
        }

        public void LogException(string msg, string source) {
            SetMessage(msg, source, LogLevel.ERROR, IS_NOT_PART_OF_STATE);
            Log();
        }
    }
}
