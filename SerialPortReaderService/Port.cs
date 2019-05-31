using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortReaderService
{
    public class Port
    {
        public string name { get; set; }
        public int baud { get; set; }
        public Parity parity { get; set; }
        public int dataBits { get; set; }
        public StopBits stopBits { get; set; }
        public Boolean isRFID { get; set; }
    }
}
