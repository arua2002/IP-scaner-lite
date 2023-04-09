using System;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;
using System.Collections.Generic;
using System.Threading;
using System.Net.Sockets;

namespace lokal
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public void GetDeviceInfo(string ipAddress)
        {
            // Проверяем доступность устройства
            Ping ping = new Ping();
            PingReply reply = ping.Send(ipAddress);
            
            if (reply.Status == IPStatus.Success)
            {
                IPHostEntry host = Dns.GetHostEntry(reply.Address);            
                listBox1.Items.Add($"Device name: {host.HostName}");
                listBox1.Items.Add($"IP address: {reply.Address}");
            }
            else
            {
                label1.Text = "Не найдено";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();           
            GetDeviceInfo(textBox5.Text);
        }
    }
}
