// Ŭnicode please
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Assets.Kanau.Utils {
    public enum ReportLevel {
        Error, All
    }

    public interface ICustomLogger {
        void LogErrorFormat(string format, params object[] args);
        void LogFormat(string format, params object[] args);

        void SaveReportFile(string path);
    }

    public class ConsoleLogger : ICustomLogger {
        public void LogErrorFormat(string format, params object[] args) {
            Debug.LogErrorFormat(format, args);
        }
        public void LogFormat(string format, params object[] args) {
            Debug.LogFormat(format, args);
        }
        public void SaveReportFile(string path) { return; }
    }

    public class TextLogger : ICustomLogger {
        StringBuilder sb = new StringBuilder();

        public void LogErrorFormat(string format, params object[] args) {
            var msg = "ERROR: " + string.Format(format, args);
            sb.AppendLine(msg);
        }
        public void LogFormat(string format, params object[] args) {
            var msg = "LOG:   " + string.Format(format, args);
            sb.AppendLine(msg);
        }

        public void SaveReportFile(string path) {
            FileHelper.SaveContentsAsFile(sb.ToString(), path);
        }
    }

    public class Report {
        public static readonly Report Instance;
        static Report() {
            Instance = Get("Kanau");
        }

        readonly static Dictionary<string, Report> reports = new Dictionary<string, Report>();
        public static Report Get(string tag) {
            Report report;
            if (reports.TryGetValue(tag, out report)) {
                return report;
            } else {
                reports[tag] = new Report(tag);
                return reports[tag];
            }
        }

        public ReportLevel Level { get; set; }
        public string Tag { get; private set; }

        ICustomLogger[] loggers;
        public bool UseConsole
        {
            set
            {
                if(value == true) {
                    loggers = new ICustomLogger[] { new ConsoleLogger(), };
                } else {
                    loggers = new ICustomLogger[] { new ConsoleLogger(), new TextLogger() };
                }
            }
        }

        public Report(string tag) {
            Level = ReportLevel.All;
            this.Tag = tag;
            this.UseConsole = false;
        }

        public void Error(string message, params object[] args) {
            if (Level < ReportLevel.Error) {
                return;
            }
            foreach(var logger in loggers) {
                logger.LogErrorFormat(message, args);
            }
        }

        public void Log(string message, params object[] args) {
            if (Level < ReportLevel.All) {
                return;
            }
            foreach(var logger in loggers) {
                logger.LogFormat(message, args);
            }
        }

        public void SaveReport(string filename) {
            var rootpath = ExportSettings.Instance.destination.rootPath;
            var path = Path.Combine(rootpath, filename);
            foreach (var logger in loggers) {
                logger.SaveReportFile(path);
            }
        }
    }
}