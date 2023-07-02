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
using System.IO;
namespace FTP_Client
{
    public partial class Form1 : Form
    {
        public static Form1 hold;
        public NetworkStream onlineData;
        public TcpClient Connection;
        public TcpClient Connection2;
        public StreamReader reader;
        public StreamWriter writer;
        public string rsp;
        public Form1()
        {
            InitializeComponent();
            hold = this;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           //Connection = new TcpClient("127.0.0.1", 1234);
            Connection = new TcpClient(ip.Text, 21);
            onlineData = Connection.GetStream();

           
        // reader = new StreamReader(onlineData);
        // writer = new StreamWriter(onlineData);

             Form2 frm2 = new Form2();


            frm2.Show();
            this.Hide();


            // rsp = reader.ReadLine();

            //textBox3.Text = rsp;
            // Form2 frm2 = new Form2(rsp);


            //frm2.Show();%%%%

            //   this.Hide();
            /*
            richTextBox1.Clear();
            writer.WriteLine("dir-lst");
            writer.Flush();
            string res;
            while (true)
            {
                res = reader.ReadLine();
                if (res == "")
                {
                    break;
                }
                else
                {
                    richTextBox1.AppendText(res);
                    richTextBox1.AppendText("\n");


                }


            }*/



        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ip_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
