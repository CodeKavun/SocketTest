using Newtonsoft.Json;
using SomeJunk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientSide
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Product> products = new List<Product>();

            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(ip, 8088);

            Console.WriteLine($"{ip}");
            Console.WriteLine($"{endPoint}");

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            try
            {
                socket.Connect(endPoint);

                if (socket.Connected)
                {
                    socket.Send(Encoding.Unicode.GetBytes("Hello, Server!"));

                    byte[] buffer = new byte[1024];
                    string someStr = "";
                    int length = 0;

                    do
                    {
                        length = socket.Receive(buffer);
                        someStr += Encoding.Unicode.GetString(buffer, 0, length);
                    } while (length > 1024);

                    products = JsonConvert.DeserializeObject<List<Product>>(someStr);
                    Console.WriteLine($"Products:");
                    foreach (Product product in products) Console.WriteLine(product);
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                socket.Shutdown(SocketShutdown.Both);
            }
        }
    }
}
