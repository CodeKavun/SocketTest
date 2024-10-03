using Newtonsoft.Json;
using SomeJunk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Product> products = new List<Product>
            {
                new Product("Roshen Waffers", 99999.99, "ROSHEN"),
                new Product("Crazy Bee", 199.99, "ROSHEN"),
                new Product("Some cakes", 99999.99, "Biedronka"),
                new Product("Candies from ATB", 99999.99, "ATB")
            };

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(ip, 8088);

            socket.Bind(endPoint);
            socket.Listen(10);

            try
            {
                while (true)
                {
                    Socket newSocket = socket.Accept();
                    Console.WriteLine(newSocket.RemoteEndPoint.ToString());

                    byte[] buffer = new byte[1024];
                    string someStr = string.Empty;
                    int length = 0;

                    do
                    {
                        length = newSocket.Receive(buffer);
                        someStr += Encoding.Unicode.GetString(buffer, 0, length);
                    }
                    while (length > 1024);

                    Console.WriteLine($"Received message: {someStr}");

                    newSocket.Send(Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(products)));

                    newSocket.Shutdown(SocketShutdown.Both);
                    newSocket.Close();
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
