open System.IO
open System.Net
open System.Net.Sockets
open System.Text

[<EntryPoint>]
let main _argv =
    let socket = new Socket(AddressFamily.InterNetwork,
                            SocketType.Stream,
                            ProtocolType.Tcp)
    
    let endPoint = new IPEndPoint(IPAddress.Loopback, 8080)
    socket.Connect(endPoint)
    
    let buffer = Encoding.Default.GetBytes("test")
    
    let sendResult = socket.Send(buffer)
    if sendResult = -1 then
        printfn "送信失敗"
        socket.Close()
        1
    else
        socket.Close()
        0
