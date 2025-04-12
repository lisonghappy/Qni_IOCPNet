/***********************************

* Author    : lisonghappy
* Email     : lisonghappy@gmail.com
* Date      : 2025-04-11
* Desc      : IOCP Net Session

************************************/



using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace IOCPNet {

    public enum IOCPSessionState {
        None,
        Connected,
        Disconnected
    }

    public abstract class IOCPSession {

        private Socket socket;
        public int sessionID;
        private IOCPSessionState sessionState = IOCPSessionState.None;

        private Action<int> OnSessionCloseCallback;


        //send
        private bool isMessageSending = false;
        private Queue<byte[]> sendCacheQueue;
        private SocketAsyncEventArgs sendEventArgs;


        //receive
        private List<byte> receiveCacheList;
        private SocketAsyncEventArgs receiveEventArgs;


        public IOCPSession () {
            sendCacheQueue = new Queue<byte[]>();
            receiveCacheList = new List<byte>();

            receiveEventArgs = new SocketAsyncEventArgs();
            sendEventArgs = new SocketAsyncEventArgs();

            receiveEventArgs.SetBuffer(new byte[2048], 0, 2048);

            receiveEventArgs.Completed += OnSocketAsyncCompleted;
            sendEventArgs.Completed += OnSocketAsyncCompleted;

        }


        public void Init (Socket socket, Action<int> OnSessionCloseCallback = null) {
            this.socket = socket;
            this.OnSessionCloseCallback = OnSessionCloseCallback;


            sessionState = IOCPSessionState.Connected;

            OnConnected();

            StartReceive();
        }

        /// <summary>
        /// session 建立连接完成时
        /// / When the session is complete.
        /// </summary>
        protected abstract void OnConnected ();
        /// <summary>
        /// session 断开连接时
        /// / when the session disconnect.
        /// </summary>
        protected abstract void OnDisconnected ();
        /// <summary>
        /// session 接收到网络数据时
        /// / When the session receives network data.
        /// </summary>
        /// <param name="buff"></param>
        protected abstract void OnReceiveMessage (byte[] buff);


        #region  -------------- Receive
        /// <summary>
        /// 开始接收网络数据
        /// / Start receiving network data.
        /// </summary>
        private void StartReceive () {
            if (socket == null) {
                IOCPUtils.Logger.LogWarning("Socket is NULL! Can't receive net msg.");
                return;
            }

            bool suspend = socket.ReceiveAsync(receiveEventArgs);
            // socket 没有挂起，直接处理数据接收
            //Socket is not suspended, directly handle data reception.
            if (!suspend) {
                ProcessReceive();
            }
        }

        /// <summary>
        /// 处理网络接收的数据
        /// / Process the data received from the network.
        /// </summary>
        private void ProcessReceive () {
            if (socket != null && receiveEventArgs.BytesTransferred > 0 && receiveEventArgs.SocketError == SocketError.Success) {

                //get data
                byte[] bytes = new byte[receiveEventArgs.BytesTransferred];
                Buffer.BlockCopy(receiveEventArgs.Buffer, 0, bytes, 0, receiveEventArgs.BytesTransferred);

                //cache data
                receiveCacheList.AddRange(bytes);

                //process data
                ProcessReceiveCacheBytes();

                //start new receive process
                StartReceive();
            }
            else {
                IOCPUtils.Logger.LogWarningFormat("[Session process receive ] id:{0} , receive len:{1}, SocketError: {2}",
                    sessionID, receiveEventArgs.BytesTransferred, receiveEventArgs.SocketError.ToString());
                Close();
            }
        }

        /// <summary>
        /// 处理接收到的缓存网络数据
        /// / Process the cached network data received.
        /// </summary>
        private void ProcessReceiveCacheBytes () {
            byte[] _buff = IOCPUtils.NetMessage.SplitNetMessageBytes(ref receiveCacheList);
            if (_buff != null) {
                OnReceiveMessage(_buff);

                //开启新的处理过程
                //Start a new process
                ProcessReceiveCacheBytes();
            }
        }
        #endregion




        #region  -------------- Send
        /// <summary>
        /// 发送网络消息数据
        /// / Send network message data.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public bool SendMsg (byte[] bytes) {
            if (socket == null) {
                IOCPUtils.Logger.LogError("Socket Error! Cannot send net msg.");
                return false;
            }

            if (sessionState != IOCPSessionState.Connected) {
                IOCPUtils.Logger.LogWarning("Connection is break,cannot send net msg.");
                return false;
            }
            if (isMessageSending) {
                sendCacheQueue.Enqueue(bytes);
                return true;
            }
            isMessageSending = true;
            sendEventArgs.SetBuffer(bytes, 0, bytes.Length);
            bool suspend = socket.SendAsync(sendEventArgs);
            if (!suspend) {
                ProcessSend();
            }
            return true;
        }

        /// <summary>
        /// 处理发送后的异步事件
        /// / Process asynchronous events after sending.
        /// </summary>
        private void ProcessSend () {
            if (sendEventArgs.SocketError == SocketError.Success) {
                isMessageSending = false;
                if (sendCacheQueue.Count > 0) {
                    byte[] item = sendCacheQueue.Dequeue();
                    SendMsg(item);
                }
            }
            else {
                IOCPUtils.Logger.LogErrorFormat("Session Process [Send] Error!. {0}", sendEventArgs.SocketError.ToString());
                Close();
            }
        }

        #endregion


        protected IPEndPoint GetSocketIPEndPoint () {
            if (socket == null) {
                return null;
            }
            IPEndPoint localEndPoint = (IPEndPoint)socket.LocalEndPoint;
            return localEndPoint;
        }

        /// <summary>
        /// socket 异步时间完成后
        /// / After the socket asynchronous time is complete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnSocketAsyncCompleted (object sender, SocketAsyncEventArgs eventArgs) {
            switch (eventArgs.LastOperation) {
                case SocketAsyncOperation.Receive:
                    ProcessReceive();
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend();
                    break;
                default:
                    IOCPUtils.Logger.LogWarning("The last operation completed on the socket was not a receive or send op.");
                    break;
            }
        }


        /// <summary>
        /// 关闭当前的 session
        /// / Close the current session.
        /// </summary>
        public void Close () {
            if (socket != null) {
                sessionState = IOCPSessionState.Disconnected;
                OnDisconnected();

                OnSessionCloseCallback?.Invoke(sessionID);

                receiveCacheList.Clear();
                sendCacheQueue.Clear();
                isMessageSending = false;

                try {
                    socket.Shutdown(SocketShutdown.Send);
                }
                catch (Exception e) {
                    IOCPUtils.Logger.LogErrorFormat("[Session] socket shutdown Error! {0}", e.ToString());
                }
                finally {
                    socket.Close();
                    socket = null;
                }
            }
        }

    }
}

