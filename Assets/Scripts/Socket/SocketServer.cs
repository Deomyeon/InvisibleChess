using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace InvisibleChess
{
    class SocketServer : SocketEntity
    {
        TcpListener serverSocket;

        public SocketServer()
        {
        }
        ~SocketServer()
        {
            Stop();
        }


        private bool Start(int port)
        {
            serverSocket = new TcpListener(System.Net.IPAddress.Any, port);
            try
            {
                serverSocket.Start();
            }
            catch
            {
                return false;
            }
            return true;
        }
        private bool Stop()
        {
            try
            {
                serverSocket.Stop();
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool Accept()
        {
            try
            {
                clientSocket = serverSocket.AcceptTcpClient();
            }
            catch
            {
                return false;
            }
            if (clientSocket == null) return false;
            return true;
        }

        public override bool Begin(int bufferSize, int port, int timeoutTime)
        {
            SetBufferSize(bufferSize);
            SetTimeoutTime(timeoutTime);
            if (serverSocket == null)
            {
                bool started = Start(port);

                return started;
            }
            return true;
        }

        public override bool Play()
        {
            return Accept();
        }

        public override bool End()
        {
            bool closed = true;
            try
            {
                clientSocket.Close();
            }
            catch
            {
                closed = false;
            }
            clientSocket = null;
            return closed;
        }
    }
}
