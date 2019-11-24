open System.IO
open System.Net
open System.Net.Sockets
open System.Text

[<EntryPoint>]
let main _argv =
    printfn "送信する文字列を入力して下さい"
    let input = stdin.ReadLine()
    
    let socket = new Socket(AddressFamily.InterNetwork,
                            SocketType.Stream,
                            ProtocolType.Tcp)
    
    let endPoint = new IPEndPoint(IPAddress.Loopback, 8080)
    
    printfn "接続開始"
    socket.Connect(endPoint)
    
    let sendBuf = Encoding.Default.GetBytes(input)
    let sendBufLen = sendBuf.Length
    
    printfn "送信開始"
    let sendResult = socket.Send(sendBuf)
    if sendResult = -1 then
        printfn "送信失敗"
        socket.Close()
        1
    else
        printfn "送信完了"
        printfn "受信開始"
        let memory = new MemoryStream()
        let rec receive (total : int) : unit =
            if total >= sendBufLen then
                printfn "受信完了"
                printfn "接続終了"
                socket.Close()
                ()
            else
                let buffer = [| for _ in 1..10 -> byte(0) |]
                let len = socket.Receive(buffer)
                memory.Write(buffer, 0, len)
                receive (total + len)
        receive 0
        printfn "受信: %s" <| Encoding.Default.GetString(memory.GetBuffer())
        socket.Close()
        0
