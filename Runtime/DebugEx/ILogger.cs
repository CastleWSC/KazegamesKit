
namespace KazegamesKit
{
    public interface ILogger
    {
        bool LogEnabled { get; set; }
        ELogType LogFilter { get; set; }
        ILogHandler LogHandler { get; set; }
        void Log(ELogType logType, object log);
    }

    class Logger : ILogger
    {
        public bool LogEnabled { get; set; }
        public ELogType LogFilter { get; set; }
        public ILogHandler LogHandler { get; set; }

        public Logger(ILogHandler handler)
        {
            LogEnabled = true;
            LogFilter = ELogType.System;
            LogHandler = handler;
        }

        public void Log(ELogType logType, object log)
        {
            if (IsLogTypeAllowed(logType))
            {
                LogHandler.Log(logType, log.ToString());
            }
        }

        public bool IsLogTypeAllowed(ELogType logType)
        {
            return (LogEnabled) && (logType <= LogFilter);
        }
    }
}
