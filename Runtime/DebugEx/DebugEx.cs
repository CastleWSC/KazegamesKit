using System;
using System.Diagnostics;

namespace KazegamesKit
{
    public static class DebugEx
    {
        private static ILogger _logger;
        public static ILogger Logger { get { return _logger; } }

        static DebugEx()
        {
            _logger = new Logger(new LogHandler());
        }

        [Conditional("KAZE_DEBUG")]
        public static void Log(ELogType logType, object log)
        {
            _logger.Log(logType, log);
        }
    }
}
