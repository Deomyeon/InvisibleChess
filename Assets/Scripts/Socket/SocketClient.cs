using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace InvisibleChess
{
    class SocketClient : SocketEntity
    {

        public SocketClient()
        {
        }
        ~SocketClient()
        {
            Close();
        }

        private bool Connect(string address, int port)
        {
            try
            {
                clientSocket.Connect(System.Net.IPAddress.Parse(address), port);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool Close()
        {
            try
            {
                clientSocket.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public override bool Begin(int bufferSize, int port, string address)
        {
            SetBufferSize(bufferSize);
            clientSocket = new TcpClient();
            return Connect(address, port);
        }
        public override bool Play()
        {
            return true;
        }

        public override bool End()
        {
            return Close();
        }
    }
}
