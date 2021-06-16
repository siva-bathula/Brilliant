using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDefs.DotNet
{
    public class Logger
    {
        public static void Log(string strLogText)
        {
            // Create a writer and open the file:
            StreamWriter log;
            string fileName = string.Format(@"{0}.txt", Guid.NewGuid());

            if (!File.Exists(fileName)) { log = new StreamWriter(fileName); }
            else { log = File.AppendText(fileName); }

            // Write to the file:
            log.WriteLine(DateTime.Now);
            log.WriteLine(strLogText);
            log.WriteLine();

            // Close the stream:
            log.Close();
            log.Dispose();
        }

        public static void Log(LoggerContext lc) {
            if (lc == null) throw new Exception("LoggerContext is null.");

            // Create a writer and open the file:
            StreamWriter log;

            if (lc.ExtendingPreviousLog) {
                bool append = lc.Mode == FileMode.Append ? true : false;
                log = new StreamWriter(lc.FileName, append);
            }
            else if (!File.Exists(lc.FileName)) { log = new StreamWriter(lc.FileName); }
            else { log = File.AppendText(lc.FileName); }

            // Write to the file:
            log.WriteLine(DateTime.Now);
            log.WriteLine(lc.Log);
            log.WriteLine();

            // Close the stream:
            log.Close();
            log.Dispose();
        }
    }

    public class LoggerContext {
        public string Context;
        public FileMode Mode;
        public string FileName
        {
            get
            {
                return string.Format(@"{0}.txt", Context);
            }
        }

        internal bool ExtendingPreviousLog { get; set; }

        public LoggerContext(string existingFile) {
            Context = existingFile;
            ExtendingPreviousLog = true;
            Mode = FileMode.Append;
        }

        public LoggerContext()
        {
            Context = Guid.NewGuid().ToString();
            Mode = FileMode.OpenOrCreate;
        }

        private object _syncLog = new object();
        public string Log { get; internal set; }
        public void AppendCurrentLog(string log) {
            lock (_syncLog) {
                Log += log;
            }
        }

        public void AppendNewLine() {
            lock (_syncLog)
            {
                Log += Environment.NewLine;
            }
        }
    }
}