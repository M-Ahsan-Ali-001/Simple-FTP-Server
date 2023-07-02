using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FTP_server_new
{
    class Program
    {
        public class FTP_Server
        {
            private TcpListener listener;
            private bool running;
            private string ipAddress;
            public FTP_Server(string ip, int port)
            {
                IPAddress ipAddress = IPAddress.Parse(ip);
                listener = new TcpListener(ipAddress, port);
            }

            public void Start()
            {
                running = true;
                listener.Start();
                Console.WriteLine("FTP server started");

                while (running)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Thread thread = new Thread(new ParameterizedThreadStart(HandleClient));
                    thread.Start(client);
                }
            }

            public void Stop()
            {
                running = false;
                listener.Stop();
                Console.WriteLine("FTP server stopped");
            }

            private void HandleClient(object obj)
            {
                TcpClient client = (TcpClient)obj;
               
                NetworkStream stream = client.GetStream();

                StreamReader reader = new StreamReader(stream);
                StreamWriter writer = new StreamWriter(stream);

                writer.WriteLine("220 FTP server ready");
                writer.Flush();
               // writer.WriteLine("");
                //writer.Flush();


                while (client.Connected)
                {
                    string command;
                    try
                    {
                        command = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error reading command: " + e.Message);
                        break;
                    }

                    Console.WriteLine("Received command: " + command);

                    // Handle commands here
                    if (command == null)
                    {
                        writer.WriteLine("500 Unknown command");
                        writer.Flush();
                        


                    }
                    else if (command == "USER anonymous")
                    {
                        writer.WriteLine("connected");
                        writer.Flush();

                        writer.WriteLine("");
                        writer.Flush();

                    }
                    else if (command.StartsWith("list") && command != null)
                    {
                        ListFiles(writer);
                         ipAddress = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

                        Console.WriteLine(ipAddress);


                    }
                    else if (command.StartsWith("upload") && command != null)
                    {
                        string fileName = command.Substring(7);
                        ReceiveFile(reader, fileName);
                    }
                    else if (command.StartsWith("download") && command != null)
                    {
                        string fileName = command.Substring(9);
                        SendFile(writer, fileName);
                    }
                    else if (command == "quit" && command != null)
                    {
                        break;
                    }
                    else if (command == null)
                    {

                        writer.WriteLine("500 Unknown command");
                        writer.Flush();
                         writer.WriteLine("");
                        writer.Flush();

                    }
                    else
                    {
                        writer.WriteLine("500 Unknown command");
                        writer.Flush();
                    }
                }

                client.Close();
            }

            private void ListFiles(StreamWriter writer)
            {
                // change the path according to tour pc
                string path = @"C:\Users\Anonymous Guy\source\repos\FTP_server_new\FTP_server_new\root_dir";
                string[] files = Directory.GetFiles(path);

                writer.WriteLine("File List:");
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    writer.WriteLine(fileName);
                }

                writer.Flush();


                writer.WriteLine("");
                writer.Flush();
            }

            private void ReceiveFile(StreamReader reader, string fileName)
            {

                // change the path according to tour pc
                string path = @"C:\Users\Anonymous Guy\source\repos\FTP_server_new\FTP_server_new\root_dir";
                string filePath = Path.Combine(path, fileName);

                try
                {
                    using (FileStream fileStream = File.Create(filePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line == "END")
                                break;

                            byte[] buffer = Convert.FromBase64String(line);
                            fileStream.Write(buffer, 0, buffer.Length);
                        }
                    }

                    Console.WriteLine("File uploaded: " + filePath);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error uploading file: " + e.Message);
                }
            }

            private void SendFile(StreamWriter writer, string fileName)
            {
                // change the path according to tour pc
                string path = @"C:\Users\Anonymous Guy\source\repos\FTP_server_new\FTP_server_new\root_dir";
                //string rootDirName = "root_dir";
               // string path = Path.Combine(Directory.GetCurrentDirectory(), rootDirName);

                Console.WriteLine(path);
                string filePath = Path.Combine(path, fileName);

                if (File.Exists(filePath))
                {

                  


                    

                    writer.WriteLine("FileExists");
                    writer.Flush();

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

                    Console.WriteLine("File sent: " + filePath);
                }
                else
                {
                    writer.WriteLine("FileNotFound");
                    writer.Flush();
                }
            }
        }

        static void Main(string[] args)
        {
            FTP_Server server = new FTP_Server("127.0.0.1", 21);
          


            server.Start();
        }
    }
}
