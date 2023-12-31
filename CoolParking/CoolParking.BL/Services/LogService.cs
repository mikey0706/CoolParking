﻿// TODO: implement the LogService class from the ILogService interface.
//       One explicit requirement - for the read method, if the file is not found, an InvalidOperationException should be thrown
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in LogServiceTests you can find the necessary constructor format.
using CoolParking.BL.Interfaces;
using System;
using System.Diagnostics;
using System.IO;

namespace CoolParking.BL.Services
{
    public class LogService : ILogService
    {
        private readonly string _logFileName;
        public string LogPath
        {
            get
            {
                string _directory = Directory.GetCurrentDirectory();
                return Path.Combine(_directory.Substring(0, _directory.IndexOf("bin")), _logFileName);
            }
        }

        public LogService(string logPath)
        {
            _logFileName = logPath;
        }

        public string Read()
        {
            if (!File.Exists(LogPath))
            {
                throw new InvalidOperationException();
            }

            return File.ReadAllText(LogPath);
        }

        public void Write(string logInfo)
        {
            using (FileStream fs = File.Open(LogPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter file = new StreamWriter(fs))
            {
                file.WriteLine(logInfo);
            }

        }
    }
}