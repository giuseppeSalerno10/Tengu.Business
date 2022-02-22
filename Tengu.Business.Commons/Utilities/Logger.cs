using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Business.Commons
{
    public class Logger : ILogger
    {
        private readonly string _directoryPath = $"{Environment.CurrentDirectory}\\log";

        public void WriteInfo(string message, object? obj = null, bool writeToFile = true)
        {
            var currentTime = DateTime.Now;

            var strToWrite = $"#####################################\n" +
                $"[ DATETIME: {currentTime} ] - Information\n" +
                $"MESSAGE: {message}\n";

            if(obj != null)
            {
                strToWrite += $" -- OBJECT -- \n";
                Type type = obj.GetType();

                foreach (var property in type.GetProperties())
                {
                    strToWrite += $"{property.Name} : {property.GetValue(obj)}\n";
                }
            }

            strToWrite += "\n\n";

            if (writeToFile)
            {
                var bytes = Encoding.ASCII.GetBytes(strToWrite);

                var file = OpenOrReadFile();

                using (file)
                {
                    file.Write(bytes, 0, bytes.Length);
                }
            }

            Debug.Write(strToWrite);

        }

        public void WriteError(string message, Exception? exception = null, bool writeToFile = true)
        {
            var currentTime = DateTime.Now;

            var strToWrite = $"#####################################\n" +
                $"[ DATETIME: {currentTime} ] - Error\n" +
                $"MESSAGE: {message}\n" +
                $" -- ERROR -- \n";

            Exception? tempException = exception;

            while (tempException != null)
            {
                strToWrite += $"MESSAGE: {tempException.Message}\n" +
                    $"STACKTRACE: {tempException.StackTrace}\n";

                tempException = exception?.InnerException;

                if (tempException != null)
                {
                    strToWrite += " - INNER EXCEPTION - \n";
                }
            }

            strToWrite += "\n\n";

            if (writeToFile)
            {
                var file = OpenOrReadFile();

                using (file)
                {

                    var bytes = Encoding.ASCII.GetBytes(strToWrite);

                    file.Write(bytes, 0, bytes.Length);
                }
            }

            Debug.Write(strToWrite);

        }

        private FileStream OpenOrReadFile()
        {
            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }

            var filePath = $"{_directoryPath}\\log_{DateTime.Now.Date.ToString("yyyy-MM-dd")}.txt";

            var file = File.Exists(filePath) ? File.OpenWrite(filePath) : File.Create(filePath);

            return file;
        }
    }
}
