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
        let memory = new MemoryStream()
        let rec receive () =
            let buffer = [| for _ in 1..10 -> byte(0) |]
            let len = socket.Receive(buffer)
            if len = 0 then
                printfn "接続が切れました"
                socket.Close()
                ()
            else
//                printfn "%s" <| Encoding.Default.GetString(buffer, 0, len)
                memory.Write(buffer, 0, len)
                receive ()
        receive ()
        printfn "%s" <| Encoding.Default.GetString(memory.GetBuffer())
        socket.Close()
        0
