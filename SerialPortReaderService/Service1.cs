using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortReaderService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //Call the SerialReader
            FileWriter.WriteLogFile("Serial port reader service started at " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            SerialReader serialReader = new SerialReader();           
        }

        protected override void OnStop()
        {
            FileWriter.WriteLogFile("Serial port reader service stopped at " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
        }
       
    }
}
