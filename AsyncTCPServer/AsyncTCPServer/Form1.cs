using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace AsyncTCPServer
{
    public partial class Form1 : Form
    {
        private TcpListener server;
        TcpClient client;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add("Waiting for connection...");
            IPAddress addressIP;
            try
            {
                addressIP = IPAddress.Parse(textBox1.Text);
            }
            catch
            {
                MessageBox.Show("Wrong IP address format", "Error");
                textBox1.Text = String.Empty;
                return;
            }

            int port = System.Convert.ToInt16(numericUpDown1.Value);

            try
            {
                server = new TcpListener(addressIP, port);
                server.Start();

                server.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpClientCallback), server);
            }
            catch(Exception ex)
            {
                listBox1.Items.Add("Error: " + ex.Message);
            }
        }

        private void AcceptTcpClientCallback(IAsyncResult asyncResult)
        {
            TcpListener s = (TcpListener)asyncResult.AsyncState;
            client = s.EndAcceptTcpClient(asyncResult);
            SetListBoxText("Could not establish connection");
            client.Close();
            server.Stop();
        }

        private delegate void SetTextCallback(string text);

        private void SetListBoxText(string text)
        {
            if (listBox1.InvokeRequired)
            {
                SetTextCallback f = new SetTextCallback(SetListBoxText);
                this.Invoke(f, new object[] { text });
            }
            else
            {
                listBox1.Items.Add(text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (server != null)
                server.Stop();
        }
    }
}
