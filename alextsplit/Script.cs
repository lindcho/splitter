using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;

namespace alextsplit
{
    public class Script
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Script()
        {
            LoadConfiguration();
        }
        public void ExecuteFile(string fileName, int numberOfLines)
        {
            var currentDirectory = fileName;
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(currentDirectory);

            var logFolder = CreateLogFolder(currentDirectory);
            var outFileName = logFolder + "\\" + fileNameWithoutExtension + "_{0}.txt";
            var outFileNumber = 1;

            var reader = File.OpenText(currentDirectory);
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                while (!reader.EndOfStream)
                {
                    var writer = File.CreateText(string.Format(outFileName, outFileNumber++));
                    for (int counter = 0; counter < numberOfLines; counter++)
                    {
                        _log.Info($"Processing {counter} into file {outFileNumber}");
                        writer.WriteLine(reader.ReadLine());
                        if (reader.EndOfStream) break;
                    }
                    writer.Close();

                }
                reader.Close();
                stopwatch.Stop();
                _log.Info($"Splitting complete after {stopwatch.Elapsed.TotalSeconds} seconds");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private static string CreateLogFolder(string filepath)
        {
            var logFolder = filepath.Replace(".", "_");

            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);

            return logFolder;

        }

        public void LoadConfiguration()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
    }
}
