using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace monitotest.Services
{
    class Network
    {
        public static async void SendPacket(String Message, String Ip)
        {
            TcpClient client = new TcpClient();
            //IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("10.11.1.94"), 3000);
            await client.ConnectAsync(IPAddress.Parse(Ip), 3000);

            NetworkStream clientStream = client.GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(Message);
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }
    
    }
}
