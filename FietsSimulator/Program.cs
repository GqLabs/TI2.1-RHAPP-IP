using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FietsSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] ports = SerialPort.GetPortNames();
            Console.WriteLine("Availabe Comports: \n" + String.Join(" \n", ports));
            Console.WriteLine("Enter Comport:");
            string port = Console.ReadLine();
            new FietsSimulator(port);
            Console.WriteLine("Started Simulator");
            while (true)
            {
                Thread.Sleep(10);
            }
        }
    }

    
}
