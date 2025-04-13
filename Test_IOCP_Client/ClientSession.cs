// ------------------------------------
//
// FileName: ClientSession.cs
//
// Author:   lisonghappy
// Email:    lisonghappy@gmail.com
// Date:     2025/4/12
// Desc:     client net session
//
// ------------------------------------
using IOCPNet;
using NetProtocol;


namespace IOCP_Client {
    public class ClientSession : IOCPSession {


        protected override void OnConnected () {
            IOCPUtils.Logger.LogWithColor(IOCPUtils.Logger.ELogColor.Green, "connect cerver.");
        }

        protected override void OnDisconnected () {
            IOCPUtils.Logger.LogError("disconnect cerver.");

        }

        protected override void OnReceiveMessage (byte[] buff) {
            var _netMessage = ProtocolUtils.Deserialize<NetMessage>(buff);

            switch (_netMessage.Header.Cmd) {
                case Cmd.Login:
                    HandleLoginRsp(_netMessage);
                    break;
                case Cmd.BagInfo:
                    HandleBagRsp(_netMessage);
                    break;
                default:
                    break;
            }

        }


        private void HandleLoginRsp (NetMessage netMessage) {
            var _loginRsp = netMessage.Body.responseLogin;
            var _succ = _loginRsp.Success;
            var _msg = _loginRsp.Message;
            IOCPUtils.Logger.LogFormat("Login rep.\t succ:{0}\t msg:{1}", _succ, _msg);
        }

        private void HandleBagRsp (NetMessage netMessage) {
            var _bagRsp = netMessage.Body.responseBagInfo;
            foreach (var item in _bagRsp.Items) {
                IOCPUtils.Logger.LogFormat("Bag Ok.\t Id:{0}\t Name:{1}\t Quantity:{2}", item.Id, item.Name, item.Quantity);
            }
        }
    }
}

