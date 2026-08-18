using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Keplercraft_V_Orbital_Station_Console
{
    public class StationLogger
    {
        private readonly string logFile = "station_log.txt";

        public void LogModuleStatus(string message)
        {
            string logEntry =
                $"{DateTime.Now} - MODULE STATUS - {message}";

            File.AppendAllText(
                logFile,
                logEntry + Environment.NewLine
            );
        }

        public void LogCriticalCondition(string message)
        {
            string logEntry =
                $"{DateTime.Now} - CRITICAL CONDITION - {message}";

            File.AppendAllText(
                logFile,
                logEntry + Environment.NewLine
            );
        }
    }
}
