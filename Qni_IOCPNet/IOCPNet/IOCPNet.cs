/***********************************

* Author    : lisonghappy
* Email     : lisonghappy@gmail.com
* Date      : 2025-04-11
* Desc      : IOCP Net base

************************************/


using System.Net.Sockets;

namespace IOCPNet {
    public abstract class IOCPNet<T_Session> where T_Session : IOCPSession, new(){

        //config data
        protected string ip = "";
        protected int port = -1;

        //client connece server or server listen client connect
        protected Socket socket;
        protected SocketAsyncEventArgs eventArgs;

        public IOCPNet () {
            eventArgs = new SocketAsyncEventArgs();
            eventArgs.Completed += OnCompleted;
        }



        protected virtual void ProcessConnect () {
        }
        protected virtual void ProcessAccept () {
        }

        public virtual void Close () {
        }

        private void OnCompleted (object sender, SocketAsyncEventArgs eventArgs) {
            switch (eventArgs.LastOperation) {
                case SocketAsyncOperation.Accept:
                    ProcessAccept();
                    break;
                case SocketAsyncOperation.Connect:
                    ProcessConnect();
                    break;
                default:
                    IOCPUtils.Logger.LogWarning("The last operation completed on the socket was not a accept or connect op.");
                    break;
            }
        }
    }
}

