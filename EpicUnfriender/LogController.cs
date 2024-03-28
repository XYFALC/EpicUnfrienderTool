using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EpicUnfriender
{
    internal class LogController
    {
        public event EventHandler<string> ?LogMessageAdded;

        public void Log(string logMessage)
        {
            OnLogMessageAdded(logMessage);
            
        }

        protected virtual void OnLogMessageAdded(string logMessage)
        {
            LogMessageAdded?.Invoke(this, logMessage);
        }
    }
}
