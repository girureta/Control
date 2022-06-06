using Swan.Logging;
using UnityEngine;

namespace Control.WebDriver
{
    public class UnityLogger : Swan.Logging.ILogger
    {
        public LogLevel LogLevel => logLevel;
        protected LogLevel logLevel;


        public void Log(LogMessageReceivedEventArgs logEvent)
        {
            Debug.Log(logEvent.Message);
        }

        public void Dispose() { }
    }
}