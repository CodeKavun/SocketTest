using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketClientForms
{
    public partial class Form1 : Form
    {
        private TcpClient client;

        public Form1()
        {
            InitializeComponent();
        }

        private void startClientBtn_Click(object sender, EventArgs e)
        {
            try
            {
                client = new TcpClient();
                client.Connect(IPAddress.Parse(ipBox.Text), int.Parse(portBox.Text));

                NetworkStream stream = client.GetStream();
                byte[] buffer = MakeScreensot();
                stream.Write(buffer, 0, buffer.Length);

                client.Close();
            }
            catch (SocketException socketEx)
            {
                MessageBox.Show("Something wrong with socket occured: " + socketEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Some error from statan: " + ex.Message);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            client?.Close();
        }

        private byte[] MakeScreensot()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);

            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    return stream.ToArray();
                }
            }
        }
    }
}
