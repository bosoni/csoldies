using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace TCPClient
{
    public class ClientProgram
    {

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("TCP Client v1.0");
            try
            {
                new Client(args[0]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

    public class Client
    {
        public Client(string ip)
        {
            TcpClient client = new TcpClient();
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), 3000);
            client.Connect(serverEndPoint);
            NetworkStream clientStream = client.GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();

            //create a thread to handle communication with connected client
            Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
            clientThread.Start(client);


            string text;
            while (true)
            {
                text = Console.ReadLine();
                if (text == "exit") break;

                byte[] buffer = encoder.GetBytes(text);
                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();
            }

            clientStream.Close();
            client.Close();
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
            }
        }

    }


}