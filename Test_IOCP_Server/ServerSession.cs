// ------------------------------------
//
// FileName: ServerSession.cs
//
// Author:   lisonghappy
// Email:    lisonghappy@gmail.com
// Date:     2025/4/12
// Desc:     server net session 
//
// ------------------------------------
using IOCPNet;
using NetProtocol;


namespace IOCP_Server {
    public class ServerSession : IOCPSession {
        protected override void OnConnected () {
            IOCPUtils.Logger.LogWithColorFormat(IOCPUtils.Logger.ELogColor.Green,
                "Client online, allocate session ID:{0}", sessionId);
        }

        protected override void OnDisconnected () {
            var _point = GetSocketIPEndPoint();
            if (_point != null) {
                IOCPUtils.Logger.LogWarningFormat("client disconnected. ip:{0}\t port:{1}", _point.Address, _point.Port);
            }
        }

        protected override void OnReceiveMessage (byte[] buff) {
            var _netMessage = ProtocolUtils.Deserialize<NetMessage>(buff);

            switch (_netMessage.Header.Cmd) {
                case Cmd.Login:
                    HandleLoginReq(_netMessage);
                    break;
                default:
                    break;
            }
        }

        private void HandleLoginReq (NetMessage netPkg) {
            var _loginRsq = netPkg.Body.requestLogin;
            IOCPUtils.Logger.LogWarningFormat("client login req: user:{0}\t psd:{1}", _loginRsq.Username, _loginRsq.Password);
        }
    }
}

