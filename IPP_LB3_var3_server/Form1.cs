using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPP_LB3_var3_server
{
    public partial class Form1 : Form
    {
        public static bool doChange = false; 
        public static string rgb = "";
        
        public Task server = new Task(async () =>
        {
            IPAddress ipAddress = new IPAddress(new byte[] { 192, 168, 0, 157 });
            IPEndPoint ipEndPoint = new(ipAddress, 443);
            using Socket listener = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(ipEndPoint);
            listener.Listen(100);
            using (var handler = await listener.AcceptAsync())
            {
                var buffer = new byte[1_024];
                var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
                rgb = Encoding.UTF8.GetString(buffer, 0, received);
                doChange = true;
            }
            listener.Close();
        });

        public Form1()
        {
            InitializeComponent();
            server.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Interval = 200; 
            timer1.Start();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (doChange)
            {
                int r = 0, g = 0, b = 0;
                r = Int32.Parse(rgb.Substring(0, rgb.IndexOf(",")));
                rgb = rgb.Remove(0, rgb.IndexOf(",") + 1);
                g = Int32.Parse(rgb.Substring(0, rgb.IndexOf(",")));
                rgb = rgb.Remove(0, rgb.IndexOf(",") + 1);
                b = Int32.Parse(rgb.Substring(0));
                BackColor = Color.FromArgb(255, r, g, b);
                doChange = false;
                timer1.Start();
            }
        }
    }
}
