using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Media;
using System.Net.Sockets;

namespace TBC_connection
{
    public partial class TBCconnection : Form
    {
        static int counter;
        static bool repeat;
        static int timeout;
        static string address;
        static int port;
        static bool[] sound;
        public TBCconnection()
        {
            InitializeComponent();
            tbAddress.Text = "185.8.177.151";
            tbPort.Text = "3724";
            counter = 0;
            repeat = true;
            btStart.Click += new EventHandler(btStart_Click);
            sound = new bool[4] { false, true, false, false };
            comboBox1.SelectedIndex = 1;
        }


        private void btStart_Click(object sender, EventArgs e)
        {
            UpdateProgressBar(true);
            repeat = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate (object unused)
            {
                bool LoginAccess;
                bool Amazon;
                bool Nile;
                bool Yangtze;
                try
                {
                    address = tbAddress.Text;
                    port = Convert.ToInt16(tbPort.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    repeat = false;
                }
                while (repeat)
                {
                    LoginAccess = PingAddress(address, port);
                    Amazon = PingAddress("185.8.177.151", 8085);
                    Nile = PingAddress("185.8.177.151", 8086);
                    Yangtze = PingAddress("185.8.177.151", 8088);
                    UpdateWindow(LoginAccess);
                    UpdateAmazon(Amazon);
                    UpdateNile(Nile);
                    UpdateYangtze(Yangtze);
                    counter++;
                    Thread.Sleep(15000);
                }
                UpdateProgressBar(repeat);
            }));
        }
        private void UpdateProgressBar(bool start)
        {
            if (InvokeRequired)
                Invoke(new Action<bool>(UpdateProgressBar), start);
            else
            {
                if (start)
                    pBar.Style = ProgressBarStyle.Marquee;
                else
                    pBar.Style = ProgressBarStyle.Continuous;
            }
        }
        void NotificShow(int secondClose, string text)
        {
            Notification notific = new Notification(secondClose, text);
            notific.Visible = true;
        }
        private void UpdateAmazon(bool access)
        {
            if (InvokeRequired)
                Invoke(new Action<bool>(UpdateAmazon), access);
            else
            {
                outputWindow.Text += "\n\nPing Amazon..  ";
                outputWindow.Text += "\nAttempt #" + counter;
                if (access)
                {
                    outputWindow.Text += ("\nServer status: Available");
                    lbAmazonLine.Text = "Online";
                    lbAmazonLine.ForeColor = Color.Green;
                    lbAmazonStatus.Text = "Available";
                    lbAmazonStatus.BackColor = Color.GreenYellow;
                    if (sound[1])
                    {
                        NotificShow(5, "Amazon is Available");
                        System.Console.Beep(2200, 300);
                        System.Console.Beep(1100, 300);
                        System.Console.Beep(2200, 300);
                    }
                }
                else
                {
                    outputWindow.Text += ("\nServer status: Unavailable");
                    lbAmazonLine.Text = "Offline";
                    lbAmazonLine.ForeColor = Color.Red;
                    lbAmazonStatus.Text = "Unavailable";
                    lbAmazonStatus.BackColor = Color.IndianRed;
                }
            }
        }
        private void UpdateNile(bool access)
        {
            if (InvokeRequired)
                Invoke(new Action<bool>(UpdateNile), access);
            else
            {
                outputWindow.Text += "\n\nPing Nile..  ";
                outputWindow.Text += "\nAttempt #" + counter;
                if (access)
                {
                    outputWindow.Text += ("\nServer status: Available");
                    lbNileLine.Text = "Online";
                    lbNileLine.ForeColor = Color.Green;
                    lbNileStatus.Text = "Available";
                    lbNileStatus.BackColor = Color.GreenYellow;
                    if (sound[2])
                    {
                        NotificShow(5, "Nile is Available");
                        System.Console.Beep(2200, 300);
                        System.Console.Beep(1100, 300);
                        System.Console.Beep(2200, 300);
                    }
                }
                else
                {
                    outputWindow.Text += ("\nServer status: Unavailable");
                    lbNileLine.Text = "Offline";
                    lbNileLine.ForeColor = Color.Red;
                    lbNileStatus.Text = "Unavailable";
                    lbNileStatus.BackColor = Color.IndianRed;
                }
            }
        }
        private void UpdateYangtze(bool access)
        {
            if (InvokeRequired)
                Invoke(new Action<bool>(UpdateYangtze), access);
            else
            {
                outputWindow.Text += "\n\nPing Yangtze..  ";
                outputWindow.Text += "\nAttempt #" + counter;
                if (access)
                {
                    outputWindow.Text += ("\nServer status: Available");
                    lbYangtzeLine.Text = "Online";
                    lbYangtzeLine.ForeColor = Color.Green;
                    lbYangtzeStatus.Text = "Available";
                    lbYangtzeStatus.BackColor = Color.GreenYellow;
                    if (sound[3])
                    {
                        NotificShow(5, "Yangtze is Available");
                        System.Console.Beep(2200, 300);
                        System.Console.Beep(1100, 300);
                        System.Console.Beep(2200, 300);
                    }
                }
                else
                {
                    outputWindow.Text += ("\nServer status: Unavailable");
                    lbYangtzeLine.Text = "Offline";
                    lbYangtzeLine.ForeColor = Color.Red;
                    lbYangtzeStatus.Text = "Unavailable";
                    lbYangtzeStatus.BackColor = Color.IndianRed;
                }
            }
        }
        private bool PingAddress(string address, int port)
        {
            TcpClient client = new TcpClient();
            try
            {
                timeout = Convert.ToInt16(timeoutBar.Value) * 10000;

                client.SendTimeout = timeout;
                client.ReceiveTimeout = timeout;
                client.Connect(address, port);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void btStop_Click(object sender, EventArgs e)
        {
            repeat = false;
            pBar.Style = ProgressBarStyle.Continuous;
        }
        public void UpdateWindow(bool reply)
        {
            if (InvokeRequired)
                Invoke(new Action<bool>(UpdateWindow), reply);
            else
            {
                outputWindow.Text += "\n\nPinging..  ";
                outputWindow.Text += "\nIP address: " + tbAddress.Text  + " Port: " + port;
                outputWindow.Text += "\nAttempt #" + counter;
                if (reply)
                {
                    outputWindow.Text += ("\nServer status: Available");
                    labelResult.Text = "Available";
                    labelResult.BackColor = Color.GreenYellow;
                    if (sound[0])
                    {
                        NotificShow(5, "Login Server is Available");
                        System.Console.Beep(2200, 300);
                        System.Console.Beep(1100, 300);
                        System.Console.Beep(2200, 300);
                    }
                }
                else
                {
                    labelResult.Text = "Unavailable";
                    labelResult.BackColor = Color.IndianRed;
                    outputWindow.Text += ("\nServer status: Unavailable");
                }
            }
        }
        public void ShowErrorLog(string message)
        {
            if (InvokeRequired)
                Invoke(new Action<string>(ShowErrorLog), message);
            else
            {
                pBar.Style = ProgressBarStyle.Continuous;
                repeat = false;

                outputWindow.Text += "\n\n*An error occurred*";
                outputWindow.Text += "\nIP address: " + tbAddress.Text + " Port: " + port;
                outputWindow.Text += "\n" + message;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TurnOffSound(comboBox1.SelectedIndex);
        }
        private void TurnOffSound(int pos)
        {
            for (int i = 0; i < sound.Length; i++)
            {
                if (i == pos)
                    sound[i] = true;
                else
                    sound[i] = false;
            }
        }
    }
}