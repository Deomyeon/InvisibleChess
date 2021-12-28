using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace InvisibleChess
{
    class SocketManager<T> where T : SocketEntity, new()
    {
        private static SocketManager<T> instance;
        public static SocketManager<T> Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SocketManager<T>();
                }
                return instance;
            }
        }

        SocketEntity socketEntity;
        string address;
        int port;
        int bufferSize;
        int timeoutTime;

        private SocketManager()
        {
            socketEntity = new T();
        }

        public void SetAll(int bufferSize, int port, string address)
        {
            if (socketEntity is SocketServer) throw new Exception("This is not Server");
            this.address = address;
            this.port = port;
            this.bufferSize = bufferSize;
        }

        public void SetAll(int bufferSize, int port, int timeoutTime)
        {
            if (socketEntity is SocketClient) throw new Exception("This is not Client");
            this.timeoutTime = timeoutTime;
            this.port = port;
            this.bufferSize = bufferSize;
        }
        public void Start(string sendData, Action<string> receiveCallback)
        {
            if (socketEntity is SocketServer) throw new Exception("This is not Server");
            socketEntity.OnReceived = receiveCallback;
            while (!socketEntity.Begin(bufferSize, port, address)) ;
            socketEntity.Send(sendData);
            while (!socketEntity.isAvailableRecieveData()) ;
            socketEntity.Recieve();
        }
        public void Start(Func<TcpClient, string, string> sendCallback)
        {
            if (socketEntity is SocketClient) throw new Exception("This is not Client");
            socketEntity.Begin(bufferSize, port, timeoutTime);
            while (!socketEntity.Play()) ;
            while (socketEntity.isConnected() && !socketEntity.isAvailableRecieveData()) ;
            socketEntity.Send(sendCallback(socketEntity.Client, socketEntity.Recieve()));
        }

        public void End()
        {
            socketEntity.End();
        }

        public TcpClient GetClient()
        {
            return socketEntity.Client;
        }
    }
}
