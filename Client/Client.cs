using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public enum Protocol
    {
        TCP,
        UDP
    }
    public class Client
    {
        private readonly Protocol protocol;

        private string ip = "127.0.0.1";
        private int port = 1024;

        public Client(Protocol protocol)
        {
            this.protocol = protocol;
        }

        public void StartClient()
        {
            switch (protocol)
            {
                case Protocol.TCP:
                    StartTcp();
                    break;

                case Protocol.UDP:
                    StartUdp();
                    break;
            }
        }

        private void StartTcp()
        {
            IPAddress.TryParse(ip, out IPAddress ipAddress);
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);

            Socket client = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            client.Connect(endPoint);

            string command;

            do
            {
                // Сервер присылает меню
                byte[] data = new byte[2048];

                int size = client.Receive(data);

                string message = Encoding.UTF8.GetString(data, 0, size);

                Console.WriteLine(message);
                command = Console.ReadLine().Trim();

                data = Encoding.UTF8.GetBytes(command);
                client.Send(data);

                // На Exit сервер ответа не присылает
                if (IsExit(command)) break;

                data = new byte[2048];

                size = client.Receive(data);

                message = Encoding.UTF8.GetString(data, 0, size);

                Console.WriteLine(message);
                Console.WriteLine("Нажмите Enter чтобы продолжить...");
                Console.ReadLine();
                Console.Clear();

            } while (!IsExit(command));

            client.Close();
        }


        private void StartUdp()
        {
            IPAddress.TryParse(ip, out IPAddress ipAddress);

            EndPoint serverEndPoint = new IPEndPoint(ipAddress, port);

            Socket client = new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);
            string command;

            do
            {
                Console.WriteLine("1.Ping \n2.Quote \n3.Exit\nVvod: ");

                command = Console.ReadLine()?.Trim() ?? "";

                byte[] data = Encoding.UTF8.GetBytes(command);

                client.SendTo(data, serverEndPoint);

                if (IsExit(command))
                    break;

                data = new byte[2048];

                EndPoint remote =
                    new IPEndPoint(IPAddress.Any, 0);

                int size = client.ReceiveFrom(
                    data,
                    ref remote);

                string message =
                    Encoding.UTF8.GetString(data, 0, size);

                Console.WriteLine(message);
                Console.WriteLine("Нажмите Enter чтобы продолжить...");
                Console.ReadLine();
                Console.Clear();

            } while (!IsExit(command));

            client.Close();
        }


        private bool IsExit(string command)
        {
            return command == "3"|| command.Equals("exit",StringComparison.OrdinalIgnoreCase);
        }
    }
}
