# Qni_IOCPNet
A .net network framework implemented using IOCP. It can be used in .net core servers as well as Unity clients.


## CLient
```cs
    //client
    public class Client{
        var ip = "127.0.0.1";
        var port = 12180;
        IOCPClient<ClientSession> client = new IOCPClient<ClientSession>();

        // Start the client and connect to the server    
        client.Start(ip, port);
    }

    //session
    public class ClientSession : IOCPSession{}
```

## Server
```cs
    //server
    public class Server{
        var ip = "127.0.0.1";
        var port = 12180;
        IOCPServer<ServerSession> server = new IOCPServer<ServerSession>();

        //Start the server and listen on the specified IP and port
        //The third parameter is the maximum number of connections that can be established
        server.Start(ip, port, 10000);
    }


    //session
    public class ServerSession : IOCPSession{}
```