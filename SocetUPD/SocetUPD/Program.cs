using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocetUPD
{
    class Chat
    {
        private static IPAddress remoteIPAddress;
        private static int remotePort;
        private static int localPort;
        private  static string name;
        private static bool flag = true;
        
        [STAThread]
        static void Main(string[] args)
        {
           
            try
            {
                // Получаем данные, необходимые для соединения
                Console.WriteLine("Укажите локальный порт");
                localPort = Convert.ToInt16(Console.ReadLine());

                Console.WriteLine("Укажите удаленный порт");
                remotePort = Convert.ToInt16(Console.ReadLine());

                Console.WriteLine("Представтесь :");
                 name = Console.ReadLine();
                //Console.WriteLine("Укажите удаленный IP-адрес");
                //remoteIPAddress = IPAddress.Parse(Console.ReadLine());
                remoteIPAddress = IPAddress.Parse("127.0.0.1");

                // Создаем поток для прослушивания
                Thread tRec = new Thread(new ThreadStart(Receiver));
                tRec.Start();
                
                while (flag)
                {
                    Send(Console.ReadLine());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
        }

        private static void Send(string datagram)
        {
            // Создаем UdpClient
            UdpClient sender = new UdpClient();

            // Создаем endPoint по информации об удаленном хосте
            IPEndPoint endPoint = new IPEndPoint(remoteIPAddress, remotePort);

            try
            {
                if (datagram == "exit")
                {
                    flag = false;
                }
                // Преобразуем данные в массив байтов
                byte[] bytes = Encoding.UTF8.GetBytes(name+"->"+datagram);

                // Отправляем данные
                sender.Send(bytes, bytes.Length, endPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
            finally
            {
                // Закрыть соединение
                sender.Close();
            }
        }

        public static void Receiver()
        {
            // Создаем UdpClient для чтения входящих данных
            UdpClient receivingUdpClient = new UdpClient(localPort);

            IPEndPoint RemoteIpEndPoint = null;

            try
            {
                Console.WriteLine(
                   "\n-----------*******Общий чат*******-----------");

                while (flag)
                {
                    // Ожидание дейтаграммы
                    byte[] receiveBytes = receivingUdpClient.Receive(
                       ref RemoteIpEndPoint);

                    // Преобразуем и отображаем данные
                    string returnData = Encoding.UTF8.GetString(receiveBytes);
                    Console.WriteLine(" --> " + returnData.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
            
        }
    }
}
