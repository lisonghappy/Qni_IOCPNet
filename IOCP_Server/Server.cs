using IOCPNet;
using NetProtocol;


namespace IOCP_Server {

    public class Server {

        static void Main (string[] args) {


            var ip = "192.168.0.108";
            var port = 12180;
            IOCPServer<ServerSession> server = new IOCPServer<ServerSession>();


            server.Start(ip, port, 10000);

            while (true) {
                string ipt = Console.ReadLine();
                if (ipt == "quit") {
                    server.Close();
                    break;
                }
                else {
                    if (!string.IsNullOrEmpty(ipt) && ipt == "bag") {
                        var _info = "Server.Broadcast: " + ipt;
                        IOCPUtils.Logger.Log(_info);
                        var _netMessage = new NetMessage{
                            Header = new NetMessageHeader {
                            Cmd = Cmd.BagInfo
                            },
                            Body = new NetMessageBody {
                                responseBagInfo = new NetMessageResponseBagInfo {
                                    Items = new List<NetMessageBagInfoItem> {
                                        new NetMessageBagInfoItem{ Id= 1, Name = "item_1", Quantity = 12 },
                                        new NetMessageBagInfoItem{ Id= 2, Name = "item_2", Quantity = 43 },
                                        new NetMessageBagInfoItem{ Id= 3, Name = "item_3", Quantity = 2 },
                                        new NetMessageBagInfoItem{ Id= 4, Name = "item_4", Quantity = 998 },
                                    }
                                }
                            }
                        };

                        server.Broadcast(ProtocolUtils.Serialize(_netMessage));
                    }

                }
            }

            Console.ReadKey();

        }
    }
}