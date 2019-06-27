using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace UdpClientServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            Graphics graphics = Graphics.FromImage(printscreen as Image);

            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);

            printscreen.Save("printscreen.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);



            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345));

            using (var reader = new StreamReader("printscreen.jpg"))
            {
                string content = reader.ReadToEnd();
                byte[] buffer = Encoding.Default.GetBytes(content);
                while (true)
                {
                    EndPoint clientEndPoint = new IPEndPoint(0, 0);
                    byte[] receiveBuffer = new byte[1024 * 64];
                    int receiveSize = serverSocket.ReceiveFrom(receiveBuffer, ref clientEndPoint);
                    if (receiveSize == 0)
                    {
                        continue;
                    }
                    Console.WriteLine(Encoding.UTF8.GetString(receiveBuffer));
                    serverSocket.SendTo(buffer, clientEndPoint);
                }
            }
        }
    }
}
