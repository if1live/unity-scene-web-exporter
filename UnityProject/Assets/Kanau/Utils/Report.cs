// Ŭnicode please
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Kanau.Utils
{
    public enum ReportLevel
    {
        Error, Warning, All
    }

    public class Report
    {
        private static ILogger logger = Debug.logger;
        public static string rootPath = "";

        private static Dictionary<string, Report> reports = new Dictionary<string, Report>();

        public static Report Instance()
        {
            return Get("Kanau");
        }

        public static Report Get(string tag)
        {
            if (!reports.ContainsKey(tag))
            {
                reports[tag] = new Report(tag);
            }
            return reports[tag];
        }

        #region Members
        private string buffer;

        private bool useConsole;
        public bool UseConsole
        {
            get { return useConsole; }
            set { useConsole = value; }
        }

        private ReportLevel level;
        public ReportLevel Level
        {
            get { return level; }
            set { level = value; }
        }

        private string tag;
        public string Tag { get { return tag; } }

        public Report(string tag)
        {
            UseConsole = true;
            Level = ReportLevel.All;
            buffer = "";
            this.tag = tag;
        }

        public void Warning(object message)
        {
            if (Level < ReportLevel.Warning)
            {
                return;
            }
            if(UseConsole)
            {
                logger.LogWarning(tag, message);
            }
            buffer += string.Format("Warning: {0}\n", message);
        }

        public void Error(object message)
        {
            if (Level < ReportLevel.Error)
            {
                return;
            }
            if (UseConsole)
            {
                logger.LogError(tag, message);
            }
            buffer += string.Format("Error: {0}\n", message);
        }

        public void Log(object message)
        {
            if (Level < ReportLevel.All)
            {
                return;
            }
            if(UseConsole)
            {
                logger.Log(tag, message);
            }
            buffer += string.Format("Log: {0}\n", message);
        }

        public void Info(object message)
        {
            if (Level < ReportLevel.All)
            {
                return;
            }
            if (UseConsole)
            {
                logger.Log(tag, message);
            }
            buffer += string.Format("{0}\n", message);
        }

        public void SaveReport(string path)
        {
            FileHelper.SaveContentsAsFile(buffer, path);
        }
        public void SaveReportFile(string filename)
        {
            FileHelper.SaveContentsAsFile(buffer, Path.Combine(Report.rootPath, filename));
        }
        #endregion
    }
}