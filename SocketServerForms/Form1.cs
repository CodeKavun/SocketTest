using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketServerForms
{
    public partial class Form1 : Form
    {
        private TcpListener listener;

        public Form1()
        {
            InitializeComponent();
        }

        private void startServerBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Server started!");

            try
            {
                listener = new TcpListener(IPAddress.Parse(ipBox.Text), int.Parse(portBox.Text));
                listener.Start();

                Thread thread = new Thread(new ThreadStart(ThreadCrazyStuff)) { IsBackground = true };
                thread.Start();
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

        private void ThreadCrazyStuff()
        {
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();

                Action action = () =>
                {
                    screenshotViewer.Image = Bitmap.FromStream(client.GetStream());
                    screenshotViewer.BackgroundImageLayout = ImageLayout.Zoom;
                };
                Invoke(action);

                client.Close();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            listener?.Stop();
        }
    }
}
