using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string stIP = "127.0.0.1";
            int iPort = 1024;

            //Создаем сам объект адреса айпи
            IPAddress? IPadress;
            IPAddress.TryParse(stIP, out IPadress);

            //создаем конечную точку айпи+порт для сокета
            IPEndPoint? iPEndPoint = new IPEndPoint(IPadress, iPort);

            //Создаем сокет
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Присоединяемся к сокету
            client.Connect(iPEndPoint);

            byte[] data = new byte[2048];
            string messege = String.Empty;
            int size = 0;


            size = client.Receive(data);
            messege = Encoding.UTF8.GetString(data, 0, size);
            Console.WriteLine(messege);

            messege = Console.ReadLine();
            data = Encoding.UTF8.GetBytes(messege);
            client.Send(data);
            data = new byte[2048];


            size = client.Receive(data);
            messege = Encoding.UTF8.GetString(data, 0, size);
            Console.WriteLine(messege);



        }
    }
}
