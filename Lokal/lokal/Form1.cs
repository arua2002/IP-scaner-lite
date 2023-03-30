using System;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;
using System.Threading;
namespace lokal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        }
        int t = 1, t1 = 0;
        Ping ping = new Ping();
        Thread th;
        string[,] mass = new string[254, 3];
        PingReply r;
        int c1 = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            progressBar1.Maximum = 254;
            progressBar1.Visible = false;
          
        }
        private void pinger()
        {
            int t2 = 0;
            int c = 0;
            while (true)
            {
                string ip = $"192.168.0.{t2}";
                r = ping.Send(ip, 500);
                if (r.Status == IPStatus.Success)
                {
                    try
                    {
                        IPHostEntry host = Dns.GetHostEntry(ip);
                        //label2.Invoke(new Action(() => { label2.Text = $"Найдено : {t1}"; }));
                        mass[c, 0] = host.HostName;
                        mass[c, 1] = r.Address.ToString();
                        mass[c, 2] = r.RoundtripTime.ToString();
                        if (c == 254) break;
                        else
                        {
                            c++;
                            c1 = c;
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
                t2++;
                t1 = t2;
            }    
        }    
        private void timer1_Tick(object sender, EventArgs e)
        {          
            if (t == 254)
            {
                timer1.Enabled = false;
                label1.Text = $"Готово";
            }
            for (int i = 0; i < 254; i++)
            {
                    dataGridView1.Rows[i].Cells[0].Value = mass[i, 0];
                    dataGridView1.Rows[i].Cells[1].Value = mass[i, 1];
                    dataGridView1.Rows[i].Cells[2].Value = mass[i, 2];
            }    
            label2.Text = $"Найдено : {c1}";
            label1.Text =  $"пройдено {t1} из 254";
            progressBar1.Value = t;          
            t++;
        }
        private void пускстопToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            th = new Thread(pinger);
            th.Start();
            dataGridView1.RowCount = 254;
            progressBar1.Visible = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (th != null)  th.Abort(); 
        }

        private void перезапуститьПроцессToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            t = 1;
            t1 = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++) for (int j = 0; j < 3; j++)  dataGridView1.Rows[i].Cells[j].Value = "";
        }
    }
}
