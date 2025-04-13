// ------------------------------------
//
// FileName: Client.cs
//
// Author:   lisonghappy
// Email:    lisonghappy@gmail.com
// Date:     2025/4/12
// Desc:     client
//
// ------------------------------------

using IOCPNet;
using NetProtocol;



namespace IOCP_Client;
class Client {
    static void Main (string[] args) {

        var ip = "127.0.0.1";
        var port = 12180;
        IOCPClient<ClientSession> client = new IOCPClient<ClientSession>();


        client.Start(ip, port);

        while (true) {
            string ipt = Console.ReadLine();
            if (ipt == "quit") {
                client.Close();
                break;
            }
            else {
                if (!string.IsNullOrEmpty(ipt) && ipt == "login") {
                    var _info = "Client Send : login";
                    IOCPUtils.Logger.Log(_info);
                    var _netMessage = new NetMessage {
                        Header = new NetMessageHeader {
                            Cmd = Cmd.Login
                        },
                        Body = new NetMessageBody {
                            requestLogin = new NetMessageRequestLogin {
                                Username = "tester",
                                Password = "tester_psd"
                            }
                        }
                    };

                    client.Send(ProtocolUtils.Serialize(_netMessage));
                }

            }
        }

    }


}

