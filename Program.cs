using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SqrtServerApp
{
    class Program
    {
        static void Main()
        {
            int port = 13000;
            TcpListener server = null;

            try
            {
                server = new TcpListener(IPAddress.Any, port);
                server.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Клиент подключен.");

                    NetworkStream stream = client.GetStream();

                    byte[] buffer = new byte[256];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    if (double.TryParse(dataReceived, out double number))
                    {
                        double sqrtResult = Math.Sqrt(number);
                        string result = sqrtResult.ToString("F2");

                        byte[] resultData = Encoding.UTF8.GetBytes(result);
                        stream.Write(resultData, 0, resultData.Length);
                        Console.WriteLine($"Отправлен результат: {result}");
                    }
                    else
                    {
                        Console.WriteLine("Некорректный запрос от клиента.");
                    }

                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
            finally
            {
                server?.Stop();
                Console.WriteLine("Сервер остановлен.");
            }
        }
    }
}
