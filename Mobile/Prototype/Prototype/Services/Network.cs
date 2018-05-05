using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace Prototype.Services
{
    public static class Network
    {
        public static async void SendPacket(String Message, String ip)
        {
            TcpClient client = new TcpClient();
            //IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("10.11.1.94"), 3000);
            await client.ConnectAsync(IPAddress.Parse(ip), 3000);

            NetworkStream clientStream = client.GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(Message);
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }
    }
}
