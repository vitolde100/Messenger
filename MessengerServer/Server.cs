using System;
using System.Net;
using System.Net.Sockets;

namespace MessengerServer
{
    internal class Server
    {
        TcpListener m_listener;
        ClientRegistry m_registry = ClientRegistry.instance;
        Logger m_logger = Logger.instance;
        public bool m_running = true;

        public Server(IPAddress ip, int port)
        {
            m_listener = new TcpListener(ip, port);
        }

        public void Run()
        {
            m_listener.Start();
            m_logger.log("Server Started\n", this.GetType().Name);
            while (m_running)
            {
                try
                {
                    TcpClient client = m_listener.AcceptTcpClient();
                    ClientHandler handler = new ClientHandler(client);
                    Thread HandlerThread = new Thread(handler.Read);
                    HandlerThread.Start();
                }
                catch (Exception ex)
                {
                    m_logger.log(ex.Message, this.GetType().Name);
                }
            }
        }

        public void Stop()
        {
            try
            {
                m_listener.Stop();
            }
            catch (Exception ex)
            {
                m_logger.log(ex.Message, this.GetType().Name);
            }
            m_registry.DisconnectAll();
            m_logger.log("Server Closed\n", this.GetType().Name);
        }
    }
}
