using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectUtilities
{
    using System;
    using System.IO;
    using System.Threading;

    public static class Logger
    {
        private static readonly object _lock = new object();
        private static string _logFilePath;
        private static StreamWriter _writer;

        public static void Initialise(string filePath)
        {
            lock (_lock)
            {
                _logFilePath = filePath;

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                _writer = new StreamWriter(_logFilePath, append: true)
                {
                    AutoFlush = true
                };
            }
        }

        public static void Log(string message, [System.Runtime.CompilerServices.CallerFilePath] string file = "", [System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
        {
            if (_writer == null)
                throw new InvalidOperationException("Logger.Initialize must be called first.");

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            lock (_lock)
            {
                _writer.WriteLine($"{timestamp}  {Path.GetFileName(file)}:{line}  {message}");
            }
        }


        public static void Shutdown()
        {
            lock (_lock)
            {
                _writer?.Dispose();
                _writer = null;
            }
        }
    }

}
