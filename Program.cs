using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace EmailSender
{
    class Program
    {
        static void Main(string[] args)
        {
            int requestCount = 1000000;
            int interval = 1000 / requestCount;
            int threadCount = 12;

            for (int t = 0; t < threadCount; t++)
            {
                new Thread(() =>
                {
                    // Verbindung zum E-Mail-Server herstellen
                    TcpClient client = new TcpClient("10.0.42.4", 25);
                    Stream stream = client.GetStream();
                    StreamReader reader = new StreamReader(stream);
                    StreamWriter writer = new StreamWriter(stream);
                    writer.AutoFlush = true;

                    // Handshake mit dem Server
                    string response = reader.ReadLine();
                    Console.WriteLine(response);
                    
                    for (int i = 0; i < requestCount / threadCount; i++)
                    {
                    
                        // email
                        writer.WriteLine("HELO mailClient");
                        response = reader.ReadLine();
                        Console.WriteLine(response);

                        writer.WriteLine("MAIL FROM: spam@spam.com");
                        response = reader.ReadLine();
                        Console.WriteLine(response);

                        writer.WriteLine("RCPT TO: spam@lost.com");
                        response = reader.ReadLine();
                        Console.WriteLine(response);

                        writer.WriteLine("DATA");
                        response = reader.ReadLine();
                        Console.WriteLine(response);

                        writer.WriteLine("Subject: Spam");
                        writer.WriteLine();
                        writer.WriteLine("spamspam");
                        writer.WriteLine(".");
                        response = reader.ReadLine();
                        Console.WriteLine(response);

                        writer.WriteLine("QUIT");
                        response = reader.ReadLine();
                        Console.WriteLine(response);

                        // pause 
                        Thread.Sleep(interval);
                    }
                    
                    writer.WriteLine("QUIT");
                    response = reader.ReadLine();
                    Console.WriteLine(response);

                    // Verbindung beenden
                    client.Close();

                }).Start();
            }

            Console.ReadLine();
        }
    }
}
