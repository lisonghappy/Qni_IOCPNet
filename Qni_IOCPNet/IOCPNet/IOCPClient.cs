/***********************************

* Author    : lisonghappy
* Email     : lisonghappy@gmail.com
* Date      : 2025-04-11
* Desc      : IOCP Net client

************************************/

using System.Net;
using System.Net.Sockets;

namespace IOCPNet {
    public class IOCPClient<T_Session> : IOCPNet<T_Session> where T_Session : IOCPSession, new() {

        //session
        private T_Session session;


        public void Start (string ip, int port) {
            this.ip = ip;
            this.port = port;

            if (string.IsNullOrEmpty(ip) || port == -1) {
                IOCPUtils.Logger.LogError("client connect to server ip or prot error!");
                return;
            }


            IPEndPoint pt = new IPEndPoint(IPAddress.Parse(ip), port);
            socket = new Socket(pt.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            eventArgs.RemoteEndPoint = pt;

            IOCPUtils.Logger.LogWithColor(IOCPUtils.Logger.ELogColor.Green, "Client Start...");

            StartConnect();
        }

        private void StartConnect () {
            bool suspend = socket.ConnectAsync(eventArgs);
            if (!suspend) {
                ProcessConnect();
            }
        }

        protected override void ProcessConnect () {
            session = new T_Session();
            session.Init(socket);
        }

        public bool Send (byte[] bytes) {
            if (session == null) {
                IOCPUtils.Logger.LogWarning("Client session is error! can't send message.");
                return false;
            }

            var _data = IOCPUtils.NetMessage.PackingNetMessage(bytes);
            return session.SendMsg(_data);
        }

        public override void Close () {
            if (session != null) {
                session.Close();
                session = null;
            }
            if (socket != null) {
                if (socket.Connected) {
                    socket.Shutdown(SocketShutdown.Both);
                }
                socket = null;
            }
        }

    }
}

