using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace InvisibleChess
{
    abstract class SocketEntity
    {
        protected int bufferSize;
        protected int timeoutTime;

        public Action<string> OnReceived;

        protected TcpClient clientSocket;
        public TcpClient Client { get { return clientSocket; } }

        public virtual bool Begin(int bufferSize, int port, string address = "") { return false; }
        public virtual bool Begin(int bufferSize, int port, int timeoutTime = 0) { return false; }
        public abstract bool Play();
        public abstract bool End();

        protected void SetBufferSize(int size)
        {
            bufferSize = size;
        }

        protected void SetTimeoutTime(int microtime)
        {
            timeoutTime = microtime;
        }

        public void Send(string text)
        {
            try
            {
                NetworkStream stream = clientSocket.GetStream();

                string data = text;
                byte[] buffer = Encoding.UTF8.GetBytes(data);

                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }
            catch
            {

            }
        }
        public bool isAvailableRecieveData()
        {
            try
            {
                NetworkStream stream = clientSocket.GetStream();
                return stream.DataAvailable;
            }
            catch
            {
                return true;
            }
        }
        public bool isConnected()
        {
            return (!clientSocket.Client.Poll(timeoutTime, SelectMode.SelectRead) || !(clientSocket.Client.Available == 0));
        }
        public string Recieve()
        {
            try
            {
                NetworkStream stream = clientSocket.GetStream();

                string data = string.Empty;
                byte[] buffer = new byte[bufferSize];

                while (stream.DataAvailable)
                {
                    int numBytes = stream.Read(buffer, 0, bufferSize);
                    stream.Flush();
                    data += Encoding.UTF8.GetString(buffer, 0, numBytes);
                }

                if (OnReceived != null)
                    OnReceived(data);

                return data;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
