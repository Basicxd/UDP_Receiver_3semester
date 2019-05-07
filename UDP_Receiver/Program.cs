using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace UDP_Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            // Skaber en UDPClient for det data der skal læses
            UdpClient udpServer = new UdpClient(7000);

            //Laver en IPEndPoint til at læse IP Address og Port nummer for afsenderen
            IPAddress ip = IPAddress.Any;
            IPEndPoint remoteEndPoint = new IPEndPoint(ip, 7000);

            try
            {
                Console.WriteLine("Receiver er startet");
                while (true)
                {
                    Byte[] receivebBytes = udpServer.Receive(ref remoteEndPoint);

                    string receivedData = Encoding.ASCII.GetString(receivebBytes);

                    string[] data = receivedData.Split('\n');

                    

                    Console.WriteLine(receivedData);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
