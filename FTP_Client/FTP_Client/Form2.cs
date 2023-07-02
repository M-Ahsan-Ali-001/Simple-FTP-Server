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
using System.Diagnostics;

namespace FTP_Client
{
    public partial class Form2 : Form
    {
       // public static Form2 holdit;
        public static String respo;
        StreamReader reader = new StreamReader(Form1.hold.onlineData);
        StreamWriter writer = new StreamWriter(Form1.hold.onlineData);
        // Please change the path according to your PC
        String filePath = @"C:\Users\Anonymous Guy\source\repos\FTP_Client\FTP_Client\bin\Debug\" ;
        public Form2()
        {
            InitializeComponent();
          //  holdit=this;
           // richTextBox1.Text = Form1.hold.reader.ReadLine();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {


             Console.WriteLine("++++++++");

          //  StreamReader reader = new StreamReader(Form1.hold.onlineData);
         //    StreamWriter writer = new StreamWriter(Form1.hold.onlineData);

             // Send the appropriate FTP command to request a file download
             writer.WriteLine("download "+dn.Text);
             writer.Flush();




             using (NetworkStream networkStream = (Form1.hold.onlineData))
             {

                 // Receive the file
                 string command = reader.ReadLine();
                 if (command == "FileExists")
                 {
                     string filePath = dn.Text; // Specify the path where you want to save the file

                     using (FileStream fileStream = File.Create(filePath))
                     {
                         string base64Data;
                         while ((base64Data = reader.ReadLine()) != "END")
                         {
                             try
                             {
                                 byte[] buffer = Convert.FromBase64String(base64Data);
                                 fileStream.Write(buffer, 0, buffer.Length);
                             }
                             catch (FormatException err)
                             {
                                 Console.WriteLine("Invalid base64-encoded data: " + err.Message);
                                 // Handle the error accordingly (e.g., terminate the process, log the error, etc.)
                             }
                         }
                     }

                     Console.WriteLine("File received: " + filePath);

                     // Respond to the sender
                     writer.WriteLine("FileReceived");
                     writer.Flush();
                 }
                 else
                 {
                     // Handle the case where the sender indicates the file does not exist or there was an error
                     Console.WriteLine("File does not exist or there was an error.");
                 }

                 // Close the reader and writer
                //// reader.Close();
               /// // writer.Close();
        }
            


            // Close the BinaryReader and BinaryWriter
            // reader.Close();
            //writer.Close();


            /*Console.WriteLine("++++++++");
            StreamReader reader = new StreamReader(Form1.hold.onlineData);
            StreamWriter writer = new StreamWriter(Form1.hold.onlineData);
            //string x = reader.ReadLine();
            writer.WriteLine("download abc.txt");
            writer.Flush();
            int i = 0;


            using (NetworkStream networkStream = Form1.hold.onlineData)
            {
                byte[] buffer = new byte[4096];

                // Create a FileStream to save the received file
                using (FileStream fileStream = File.Create(@"abc.txt"))
                {
                    // Read the data from the network stream and write it to the file stream
                    int bytesRead;
                    while ((bytesRead = networkStream.Read(buffer, 0, buffer.Length)) > 0)
                    {

                        fileStream.Write(buffer, 0, bytesRead);

                        Console.WriteLine(">>>>>>>>>>>>>>>" + buffer);


                    }
                }
            }*/


        }

        private void button2_Click(object sender, EventArgs e)

             {

            //TcpClient Connection = new TcpClient("127.0.0.1", 21);
            //NetworkStream onlineData = Connection.GetStream();

            //StreamReader reader = new StreamReader(Form1.hold.onlineData);
            //StreamWriter  writer = new StreamWriter(Form1.hold.onlineData);


           richTextBox1.Clear();
           writer.WriteLine("list");
           writer.Flush();
            string res;
            byte[] buffer = new byte[1024];
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


                    Console.WriteLine("+++++++++++++++++++++");
                  
                }
            }
           // reader.Close();
          //  writer.Close();
                

        }

        private void button3_Click(object sender, EventArgs e)
        {
       
            Form1.hold.Dispose();
          
           
            System.Environment.Exit(0);


        }

        private void button4_Click(object sender, EventArgs e)
        {


           // StreamReader reader = new StreamReader(Form1.hold.onlineData);
           // StreamWriter writer = new StreamWriter(Form1.hold.onlineData);


            writer.WriteLine("upload "+up.Text);
            writer.Flush();
            filePath = filePath+up.Text;
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string base64Data = Convert.ToBase64String(buffer, 0, bytesRead);
                    writer.WriteLine(base64Data);
                }
            }

            writer.WriteLine("END");
            writer.Flush();
        }

        private void dn_TextChanged(object sender, EventArgs e)
        {
             
        }
    }
}
