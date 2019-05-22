﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UDP_Receiver
{
    class Program
    {
        public static string SensorUri = "https://xn--restndopkald20190522102204-zwc.azurewebsites.net/api/nødopkald";
        public static string AlertUri = "https://xn--restndopkald20190522102204-zwc.azurewebsites.net/api/alert";

        public static async Task<HttpResponseMessage> AddSensorAsync(Sensor sensor)
        {
            using (HttpClient client = new HttpClient())
            {
                var jsonString = JsonConvert.SerializeObject(sensor);
                Console.WriteLine("Data" + sensor);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(SensorUri, content);
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new Exception("Customer already exists. Try another id");
                }
                response.EnsureSuccessStatusCode();

                return response;
            }

        }

        public static async Task<HttpResponseMessage> AddAlertAsync(Sensor sensor)
        {
            using (HttpClient client = new HttpClient())
            {
                var jsonString = JsonConvert.SerializeObject(sensor);
                Console.WriteLine("Data" + sensor);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(AlertUri, content);
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new Exception("Customer already exists. Try another id");
                }
                response.EnsureSuccessStatusCode();

                return response;
            }

        }

        public static async Task<IList<Sensor>> GetCustomersAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync(SensorUri);
                IList<Sensor> cList = JsonConvert.DeserializeObject<IList<Sensor>>(content);
                return cList;
            }
        }

        static void Main(string[] args)
        {
            //Sensor newSensor = new Sensor("18/05/2019", "21:47:30", "Intruders Test");

            //AddCustomerAsync(new Sensor("18/05/2019", "24:24:24", "Intruders here"));

            //Thread.Sleep(3000);

            //IList<Sensor> result = GetCustomersAsync().Result;
            //Console.WriteLine(result.Count);
            //foreach (var i in result)
            //{
            //    Console.WriteLine(i.ToString());
            //}

            //Console.ReadLine();
            
            // Skaber en UDPClient for det data der skal læses

            UdpClient udpServer = new UdpClient(7000);

            //Laver en IPEndPoint til at læse IP Address og Port nummer for afsenderen
            IPAddress ip = IPAddress.Any;
            IPEndPoint remoteEndPoint = new IPEndPoint(ip, 7000);

            //try
            //{
            Console.WriteLine("Receiver er startet");
            while (true)
            {
                Byte[] receivebBytes = udpServer.Receive(ref remoteEndPoint);

                string receivedData = Encoding.ASCII.GetString(receivebBytes);

                string[] data = receivedData.Split('\n');

                string motion = data[0];
                string datoOgTid = data[1];

                string[] tidsSplit = datoOgTid.Split(' ');

                string dato = tidsSplit[1];
                string tid = tidsSplit[2];

                string[] motionSplit = motion.Split(' ', 2);

                string motion1 = motionSplit[0];
                string motion2 = motionSplit[1];

                //Console.WriteLine(motion2);
                //Console.WriteLine(dato);
                //Console.WriteLine(tid);

                Sensor sensor = new Sensor();

                HttpResponseMessage mi = AddSensorAsync(new Sensor(sensor.Dato = dato, sensor.Tid = tid, sensor.Motion = motion2)).Result;
                HttpResponseMessage mi2 = AddAlertAsync(new Sensor(sensor.Dato = dato, sensor.Tid = tid, sensor.Motion = motion2)).Result;



            }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.ToString());
            //
            //}


        }

    }
}
