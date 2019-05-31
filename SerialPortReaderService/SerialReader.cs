using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml;
namespace SerialPortReaderService
{
    public class SerialReader
    {
        //EXPECTING ONLY MAX OF 8 PORTS
        SerialPort serialPort1; SerialPort serialPort2; SerialPort serialPort3; SerialPort serialPort4;
        SerialPort serialPort5; SerialPort serialPort6; SerialPort serialPort7; SerialPort serialPort8;
      
        List<Port> ports = new List<Port>();
        public SerialReader()
        {
            ReadXMLFile();
            ScanPort();
        }
        public void ReadXMLFile()
        {  
            Port portItem = new Port();
            String itemParity=""; String itemStopBits= "";

            //System.Diagnostics.Debugger.Launch();

            // Start with XmlReader object  
            //here, we try to setup Stream between the XML file nad xmlReader  
            using (XmlReader reader = XmlReader.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SerialPorts.xml")))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name.ToString() == "BarcodePort" || reader.Name.ToString() == "RFIDPort")
                        {
                            portItem = new Port();
                            portItem.isRFID = reader.Name.ToString() == "RFIDPort" ?  true : false;
                        }
                        //return only when  START tag  
                        switch (reader.Name.ToString())
                        {
                            case "Name":
                                portItem.name=reader.ReadString();
                                break;
                            case "Baud":
                                portItem.baud =Convert.ToInt32(reader.ReadString());
                                break;
                            case "Parity":
                                itemParity = reader.ReadString();
                                portItem.parity = (Parity)Enum.Parse(typeof(Parity), itemParity);
                                break;
                            case "DataBits":
                                portItem.dataBits = Convert.ToInt32(reader.ReadString());
                                break;
                            case "StopBits":
                                itemStopBits= reader.ReadString();
                                portItem.stopBits = (StopBits)Enum.Parse(typeof(StopBits), itemStopBits);
                                break;

                        }//END OF SWITCH                     
                    }//END OF IF
                    else
                    {
                        if ( (reader.Name.ToString() == "RFIDPort" || reader.Name.ToString() == "BarcodePort") && portItem.name !=null )
                        {
                            ports.Add(portItem);
                        }
                    }                   
                        
                }//END OF WHILE
            }//END OF USING
        }    
        public void ScanPort()
        {
            //POPULATE THE BARCODEPORT COLLECTION
            for (int i = 0; i <= ports.Count- 1; i++)
            {
                switch (i)
                {
                    case 0:
                        serialPort1 = new SerialPort(ports[i].name);
                        OpenPort(ports[i], serialPort1);
                        break;
                    case 1:
                        serialPort2 = new SerialPort(ports[i].name);
                        OpenPort(ports[i], serialPort2);
                        break;
                    case 2:
                        serialPort3 = new SerialPort(ports[i].name);
                        OpenPort(ports[i], serialPort3);
                        break;
                    case 3:
                        serialPort4 = new SerialPort(ports[i].name);
                        OpenPort(ports[i], serialPort4);
                        break;
                    case 4:
                        serialPort5 = new SerialPort(ports[i].name);
                        OpenPort(ports[i], serialPort5);
                        break;
                    case 5:
                        serialPort6 = new SerialPort(ports[i].name);
                        OpenPort(ports[i], serialPort6);
                        break;
                    case 6:
                        serialPort7 = new SerialPort(ports[i].name);
                        OpenPort(ports[i], serialPort7);
                        break;
                    case 7:
                        serialPort8 = new SerialPort(ports[i].name);
                        OpenPort(ports[i], serialPort8);
                        break;
                }
            }
        }
        public void OpenPort(Port itemPort, SerialPort serialPort)
        {
            try
            {
                serialPort.BaudRate = itemPort.baud;
                serialPort.DataBits = itemPort.dataBits;
                serialPort.Parity = itemPort.parity;
                serialPort.StopBits = (StopBits)itemPort.stopBits;
                //Read the ports infinitly
                serialPort.ReadTimeout = -1;

                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }                
                // ASSIGN A THREAD THAT CONSTANTLY WATCH THIS PORT
                if (itemPort.isRFID)
                {//open seperate thread for rfid readers
                 // Thread rfidThread = new Thread(() => serialPort.DataReceived += new SerialDataReceivedEventHandler(RFID_DataReceived));
                 // rfidThread.Start();
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(RFID_DataReceived);
                }
                else
                {//open seperate thread for barcode readers
                    //Thread barcodeThread = new Thread(() => serialPort.DataReceived += new SerialDataReceivedEventHandler(Barcode_DataReceived));
                    //barcodeThread.Start();
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(Barcode_DataReceived);
                }
                serialPort.Open();
            }
            catch(Exception ex)
            {
                FileWriter.WriteLogFile("Exception" + ex.StackTrace);
            }
        }
        void Barcode_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //System.Diagnostics.Debugger.Launch();
            string data = null;
            data += (sender as SerialPort).ReadExisting();
            FileWriter.WriteToFile(data,false);           
        }
        void RFID_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //System.Diagnostics.Debugger.Launch();
            string data = null;
            data += (sender as SerialPort).ReadExisting();
            FileWriter.WriteToFile(data,true);
        }
        // Call to release serial port
   
    }

}
