/***********************************

* Author    : lisonghappy
* Email     : lisonghappy@gmail.com
* Date      : 2025-04-11
* Desc      : IOCP Net Server

************************************/


using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace IOCPNet {

    public class IOCPServer<T_Session> : IOCPNet<T_Session> where T_Session : IOCPSession, new() {
        private const string server_lock = "server_lock";


        //config data
        private int maxConnectCount;// client max connect count

        //control client connect
        private int currentConnectCount = 0;
        private Semaphore acceptSeamaphore;


        //session pool
        private List<T_Session> sessionList;
        private IOCPServerSessionPool<T_Session> sessionPool;

        public IOCPServer ():base() {
            sessionList = new List<T_Session>();
        }

        /// <summary>
        /// server start
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="maxConnectCount">The server accepts the maximum number of connections from the client.</param>
        public void Start (string ip, int port, int maxConnectCount) {
            this.ip = ip;
            this.port = port;
            this.maxConnectCount = maxConnectCount;

            if (string.IsNullOrEmpty(ip) || port == -1) {
                IOCPUtils.Logger.LogError("server ip or prot error!");
                return;
            }

            currentConnectCount = 0;
            acceptSeamaphore = new Semaphore(maxConnectCount, maxConnectCount);

            sessionPool = new IOCPServerSessionPool<T_Session>(maxConnectCount);
            for (int i = 0; i < maxConnectCount; i++) {
                T_Session session = new T_Session {
                    sessionId = GenerateUniqueSessionID()
                };
                sessionPool.Push(session);
            }
            sessionList = new List<T_Session>();

            IPEndPoint pt = new IPEndPoint(IPAddress.Parse(ip), port);
            socket = new Socket(pt.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(pt);
            socket.Listen(IOCPConfig.NET_SERVER_BACKLOG);

            IOCPUtils.Logger.LogWithColor(IOCPUtils.Logger.ELogColor.Cyan, "***************** Server Start *****************");

            StartAccept();
        }


        private void StartAccept () {
            eventArgs.AcceptSocket = null;
            acceptSeamaphore.WaitOne();

            try {
                if (socket != null && socket.IsBound) {
                    bool suspend = socket.AcceptAsync(eventArgs);
                    if (!suspend) {
                        ProcessAccept();
                    }
                }
            }
            catch (System.Exception ex) {
                IOCPUtils.Logger.LogError("Socket AcceptAsync Error.  " + ex.ToString());
            }

        }

        protected override void ProcessAccept () {
            Interlocked.Increment(ref currentConnectCount);

            T_Session session = sessionPool.Pop();
            lock (server_lock) {
                if (sessionList != null) {
                    sessionList.Add(session);
                }
            }
            session.Init(eventArgs.AcceptSocket, OnSessionClose);

            StartAccept();
        }

        private void OnSessionClose (uint sessionId) {
            int index = -1;
            for (int i = 0; i < sessionList.Count; i++) {
                if (sessionList[i].sessionId == sessionId) {
                    index = i;
                    break;
                }
            }
            if (index != -1) {
                sessionPool.Push(sessionList[index]);
                lock (server_lock) {
                    sessionList.RemoveAt(index);
                }
                Interlocked.Decrement(ref currentConnectCount);
                acceptSeamaphore.Release();
            }
            else {
                IOCPUtils.Logger.LogErrorFormat("Session:{0} cannot find in server session list.", sessionId);
            }
        }


        public bool Broadcast (byte[] bytes) {
            if (sessionList == null) {
                IOCPUtils.Logger.LogWarning("Server broadcast error.");
                return false;
            }

            if (bytes == null) {
                IOCPUtils.Logger.LogWarning("Server broadcast error, message data is incorrect.");
                return false;
            }

            var _data = IOCPUtils.NetMessage.PackingNetMessage(bytes);

            var _isOk = true;
            for (int i = 0; i < sessionList.Count; i++) {
                _isOk = _isOk && sessionList[i].SendMsg(_data);
            }
            return _isOk;
        }

        public override void Close () {
            lock (server_lock) {
                if (sessionList != null) {
                    for (int i = 0; i < sessionList.Count; i++) {
                        sessionList[i].Close();
                    }
                }
                sessionList = null;
            }

            if (socket != null) {
                socket.Close();
                socket = null;
            }

            IOCPUtils.Logger.LogWithColor(IOCPUtils.Logger.ELogColor.Magenta, "***************** Server Closed. *****************");
        }

        public List<T_Session> GetSessionList () {
            return sessionList;
        }




        private uint sid = 0;
        private uint GenerateUniqueSessionID () {
            lock (sessionList) {
                while (true) {
                    ++sid;
                    if (sid == uint.MaxValue) {
                        sid = 1;
                    }

                    var _isContain = false;
                    foreach (var item in sessionList) {
                        if (item.sessionId == sid) {
                            _isContain = true;
                            break;
                        }
                    }

                    if (!_isContain) {
                        break;
                    }
                }
            }
            return sid;
        }
    }
}

