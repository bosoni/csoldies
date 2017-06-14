using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections.Generic;

namespace TCPServer
{
    public class ServerProgram
    {

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("TCP Server v1.0");
            new Server();
        }
    }

    public class Server
    {
        private TcpListener tcpListener;
        private Thread listenThread;
        List<TcpClient> clients = new List<TcpClient>();

        public Server()
        {
            this.tcpListener = new TcpListener(IPAddress.Any, 3000);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
            Console.WriteLine("Started. Waiting for connections...");
        }

        private void ListenForClients()
        {
            this.tcpListener.Start();

            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client = this.tcpListener.AcceptTcpClient();
                clients.Add(client);

                Console.WriteLine(">" + client.GetHashCode() + " connected.");

                //create a thread to handle communication with connected client
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }

        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }

                //message has successfully been received
                ASCIIEncoding encoder = new ASCIIEncoding();
                Console.WriteLine(encoder.GetString(message, 0, bytesRead));

                for (int q = 0; q < clients.Count; q++)
                {
                    TcpClient cl = (TcpClient)clients[q];
                    if (cl.GetHashCode() == tcpClient.GetHashCode()) continue;

                    NetworkStream stream = cl.GetStream();
                    stream.Write(message, 0, bytesRead);
                    stream.Flush();
                }
                
            }

            Console.WriteLine(">" + tcpClient.GetHashCode() + " leaves.");
            clients.Remove(tcpClient);
            tcpClient.Close();

        }
    }
}
