using System;
using System.Text;

namespace KazegamesKit
{
    public interface ILogHandler 
    {
        void Log(ELogType logType, string log);
    }

    class LogHandler : ILogHandler
    {
        StringBuilder _strBuilder;
        UnityEngine.ILogger _uLogger;

        readonly string[] LOG_COLORS = new string[]
        {
            "#FF0066FF",    // Error
            "#FFFF66FF",    // Warning
            "#9900FFFF",    // Trace
            "#00FFFFFF",    // Log
            "#66FF66FF"     // System
        };

        const string FORMAT = "<b><size=14><color={0}>[{1}] {2}</color></size></b>";

        public LogHandler()
        {
            _strBuilder = new StringBuilder();
            _uLogger = UnityEngine.Debug.unityLogger;
        }

        public void Log(ELogType logType, string log)
        {
            _strBuilder.Clear();

            _strBuilder.AppendFormat(FORMAT,
                LOG_COLORS[(int)logType],
                DateTime.Now.ToString("HH:mm:ss"),
                log);

            _uLogger.Log(UnityEngine.LogType.Log, logType.ToString(), _strBuilder.ToString());
        }
    }
}
