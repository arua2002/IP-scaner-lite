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
            Width = 500;
        }
        int t = 1, t1 = 0;
        Ping ping = new Ping();
        Thread th;
        string[,] mass;
        PingReply r;
        int c1 = 0;
        int c = 0;
        int x = 0, y = 0,x1= 0;
        int t2 = 0;
        bool a = false;
        private void Form1_Load(object sender, EventArgs e)
        {          
            progressBar1.Visible = false;
            settins();
        }
        private void pinger()
        {         
            while (a)
            {
                string ip = $"192.168.{x}.{t2}";
                r = ping.Send(ip, 500);
                if (r.Status == IPStatus.Success)
                {
                    try
                    {
                        IPHostEntry host = Dns.GetHostEntry(ip);
                        mass[c, 0] = host.HostName;
                        mass[c, 1] = r.Address.ToString();
                        mass[c, 2] = r.RoundtripTime.ToString();
                        if (c == x1)
                        {
                            настройкаToolStripMenuItem.Enabled = true;
                            break;
                        }
                        else
                        {
                            c++;
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
                for (int i = 0; i < x1; i++)
                {
                    if (mass[i, 0] != "")
                    {
                        dataGridView1.Rows[i].Cells[0].Value = mass[i, 0];
                        dataGridView1.Rows[i].Cells[1].Value = mass[i, 1];
                        dataGridView1.Rows[i].Cells[2].Value = mass[i, 2];
                    }
                }
                label2.Text = $"Найдено : {c}";
                label1.Text = $"пройдено {t1} из {x1}";
                progressBar1.Value = t1;
            if (t1 == x1)
            {
                timer1.Enabled = false;
                label1.Text = $"Готово";
            }

        }
        bool tr = false;
        private void пускстопToolStripMenuItem_Click(object sender, EventArgs e)
        {
            a = !a;
            timer1.Enabled = a;
            progressBar1.Visible = a;
            if (a)
            {
                настройкаToolStripMenuItem.Enabled = false;
                Width = 500;
            }
            else настройкаToolStripMenuItem.Enabled = true;
            if (!tr || !th.IsAlive) 
            {
                th = new Thread(pinger);
                th.Start();
            }
            tr = true;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            a = false;
        }
        void settins()
        {
            x = Convert.ToInt32(textBox1.Text);//область
            y = Convert.ToInt32(textBox2.Text);//min
            x1 = Convert.ToInt32(textBox3.Text);//max
            progressBar1.Maximum = x1;
            progressBar1.Minimum = y;
            mass = new string[x1, 3];
            dataGridView1.RowCount = x1;
            progressBar1.Value = y;
            t2 = y;
        }
        void restart()
        {
            for (int i = 0; i < dataGridView1.RowCount; i++) for (int j = 0; j < 3; j++) dataGridView1.Rows[i].Cells[j].Value = "";
            t1 = y;
            progressBar1.Value = y;
            timer1.Enabled = true;
            c = 0;
            t2 = y-1;
        }
        private void button1_Click(object sender, EventArgs e)//принять новые настройки
        {
            settins();
            Width = 500;
        }
        bool w = false;
        private void настройкаToolStripMenuItem_Click(object sender, EventArgs e)//открыть и закрыть меню настроек
        {
            w = !w;
            if (w) Width = 750;
            else Width = 500;
        }
        private void перезапуститьПроцессToolStripMenuItem_Click(object sender, EventArgs e)//перезапуск
        {
            restart();
        }
    }
}
