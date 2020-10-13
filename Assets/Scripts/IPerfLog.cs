using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts
{
    public interface IPerfLog
    {
        void Init();
        void LogRaw(string rawLine);
        //void LogSummary(string SummaryMetrics);
        void LogScan(string scanLine);
        void Term();
    }

    public class DebugLogger : IPerfLog
    {
        public void LogRaw(string rawLine)
        {

        }

        public void LogScan(string scanLine)
        {

        }

        //public void LogSummary(string SummaryMetrics)
        //{

        //}

        public void Init()
        {

        }

        public void Term()
        {

        }
    }

    public class GridLogger : IPerfLog
    {
        public void LogRaw(string rawLine)
        {

        }

        public void LogScan(string scanLine)
        {

        }

        //public void LogSummary(string SummaryMetrics)
        //{

        //}

        public void Init()
        {

        }

        public void Term()
        {

        }
    }

    public class FileSystemLogger : IPerfLog
    {
        private StringBuilder _sbSummary;
        private StringBuilder _sbRaw;
        private StringBuilder _sbScan;
        private string _folderPath;

        public FileSystemLogger() //constructor
        {
            _sbRaw = new StringBuilder();
            _sbScan = new StringBuilder();
        }

        //public void LogSummary(string SummaryMetrics)
        //{
        //    string _subId = TaskSettingsManager.TaskSettings.SubjectId;
        //    string _eventId = TaskSettingsManager.TaskSettings.EventId;

        //    DateTime d = DateTime.Now;
        //    string dateTime = d.ToString("yyyyMMdd_HHmmss");

        //    string fileName = Path.Combine(_folderPath, dateTime + "_" + _subId + "_" + _eventId + "_" + "BaselineSummaryLog" + ".txt");
        //    File.WriteAllText(fileName, SummaryMetrics);
        //}

        public void LogRaw(string rawLine)
        {
            _sbRaw.AppendLine(rawLine);
        }

        public void LogScan(string scanLine)
        {
            _sbScan.AppendLine(scanLine);
        }

        public void Init()
        {
            _folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\DS-CPT_Data\";

            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
        }

        public void Term()
        {
            string _subId = TaskSettingsManager.TaskSettings.SubjectID;
            string _eventId = TaskSettingsManager.TaskSettings.EventID;

            DateTime d = DateTime.Now;
            string dateTime = d.ToString("yyyyMMdd_HHmmss");

            string fileName = Path.Combine(_folderPath, dateTime + "_" + _subId + "_" + _eventId + "_" + "RawLog" + ".txt");
            File.WriteAllText(fileName, "BlockType, StimCode, StimTime, ResponseTime, ResponseEval, Hits, FalseAlarms, Misses, CorrectRejections"+ Environment.NewLine + _sbRaw.ToString());

            string scanFileName = Path.Combine(_folderPath, dateTime + "_" + _subId + "_" + _eventId + "_" + "ScanLog" + ".txt");
            File.WriteAllText(scanFileName, "scanCount , scanTime" + Environment.NewLine + _sbScan.ToString());
        }
    }
}





