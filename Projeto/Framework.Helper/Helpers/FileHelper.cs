using Framework.Helper.Extension;
using System;
using System.Collections.Generic;
using System.IO;

namespace Framework.Helper.Helpers
{
    public static class FileHelper
    {
        public static string GetBinDirectory()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.RelativeSearchPath ?? "");
        }

        public static void Debug(string msg)
        {
            LogFile("Debug", msg);
        }

        public static void Error(Exception ex)
        {
            if (ex == null)
            {
                return;
            }

            LogFile("Error", $"{ex.GetCompleteMessage()}{ Environment.NewLine }{ex.GetCompleteStackTrace()}");
        }

        public static void LogFile(string source, string msg)
        {
            string logDirectory = GetBinDirectory() + @"\Log";

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            string path = Path.Combine(logDirectory, $"Log-{DateTime.Now.Date.ToString("dd-MM-yyyy")}.txt");

            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }

            using (FileStream file = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.WriteLine($"{DateTime.Now} -({source})- {msg}");
                    sw.Close();
                }
            }
        }

        public static void LogsFile(string archiveName, IList<string> values)
        {
            string archiveDirectory = GetBinDirectory() + @"\Arquivo";

            if (!Directory.Exists(archiveDirectory))
            {
                Directory.CreateDirectory(archiveDirectory);
            }

            string path = Path.Combine(archiveDirectory, $"{archiveName}.txt");

            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }

            using (FileStream file = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(file))
                {
                    foreach (string value in values)
                    {
                        sw.WriteLine($"{DateTime.Now} -(Debug)- {value}");
                    }

                    sw.Close();
                }
            }
        }
    }
}