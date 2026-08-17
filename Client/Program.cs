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
            Client client = new Client(Protocol.TCP);
            //Client client = new Client(Protocol.UDP);
            client.StartClient();
            Console.WriteLine("Программа завершена");
        }
    }
}
