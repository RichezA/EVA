using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Windows.Forms;

namespace monitotest
{
    class RemoteServer
    {
        //public TcpClient client;
        //private TcpListener tcpListener;
        //private Thread listenThread;
        //public Boolean startup = false;

        //public RemoteServer()
        //{
        //    Console.WriteLine("Starting...");
        //    this.tcpListener = new TcpListener(IPAddress.Any, 3000);
        //    this.listenThread = new Thread(new ThreadStart(ListenForClients));
        //    this.listenThread.Start();
        //}

        //private void ListenForClients()
        //{
        //    this.tcpListener.Start();
        //    Console.WriteLine("I'm listening..");
        //    while (true)
        //    {
        //        //blocks until a client has connected to the server
        //        TcpClient client = this.tcpListener.AcceptTcpClient();

        //        //create a thread to handle communication 
        //        //with connected client
        //        Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
        //        clientThread.Start(client);
        //    }
        //}

        //private void HandleClientComm(object client)
        //{
        //    TcpClient tcpClient = (TcpClient)client;
        //    this.client = tcpClient;
        //    NetworkStream clientStream = tcpClient.GetStream();

        //    ASCIIEncoding encoder = new ASCIIEncoding();
        //    byte[] buffer = encoder.GetBytes("Hello Client!");
        //    clientStream.Write(buffer, 0, buffer.Length);
        //    clientStream.Flush();

        //    byte[] message = new byte[4096];
        //    int bytesRead;

        //    while (true)
        //    {
        //        bytesRead = 0;

        //        try
        //        {
        //            //blocks until a client sends a message
        //            bytesRead = clientStream.Read(message, 0, 4096);
        //        }
        //        catch
        //        {
        //            //a socket error has occured
        //            break;
        //        }

        //        if (bytesRead == 0)
        //        {
        //            //the client has disconnected from the server
        //            break;
        //        }
        //        Console.WriteLine(encoder.GetString(message, 0, bytesRead));
        //    }

            //tcpClient.Close();
        //}
    }
}
