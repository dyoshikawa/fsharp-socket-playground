open System.IO
open System.Net
open System.Net.Sockets
open System.Text

[<EntryPoint>]
let main _argv =
    let server = new Socket(AddressFamily.InterNetwork,
                            SocketType.Stream,
                            ProtocolType.Tcp)
//    server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true)
    
    let endPoint = new IPEndPoint(IPAddress.Loopback, 8080)
    server.Connect(endPoint)
    
    let buffer = Encoding.Default.GetBytes("test")
    
    let sendResult = server.Send(buffer)
    if sendResult = -1 then
        printfn "送信失敗"
        server.Close()
        1
    else
        server.Close()
        0
