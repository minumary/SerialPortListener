using System;
using System.IO;
using System.Threading;

namespace SerialPortReaderService
{
    public class FileWriter
    {
      public FileWriter()
        {

        }
        public static void WriteToFile(string message,bool isRFID)
        {
            string path = "C:\\TempDriverIdLogs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath;
            if(isRFID)
            {
                filepath = path + "\\RFIDCardLog" + ".txt";
            }
            else
            {
                filepath = path + "\\BarcodeCardLog" + ".txt";
            }
            

            //1. IF FILE NOT EXISTS CREATE THE FILE
            //2. IF FILE EXISTS WAIT TILL IT GOT READED BY THE WEB SERVICE.
            //3. WEB SERVICE DELETE THE FILE ONCE IT IS READ
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(message);
                }
            }
            else// IF FILE EXISTS WAIT TILL IT GOT READED BY THE WEB SERVICE.
            {
                TimeSpan retryInterval = TimeSpan.FromSeconds(10);

                //SLEEP THE THREAD FOR 10 SECONDS TO DELETE THE FILE AS IT IS SUPPOSED TO GET DELETED FROM THE WEB SERVICE
                Thread.Sleep(retryInterval);

                File.Delete(filepath);
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(message);
                }

            }
        }
        public static void WriteLogFile(string Message)
        {
            string path = "C:\\ServiceLogs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = path + "\\ReportLog" + ".txt";

            //    IF FILE NOT EXISTS CREATE THE FILE
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else// IF FILE EXISTS WRITE TO FILE
            {

                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }

            }
        }
    }
}
