open System.IO
open System.Net
open System.Net.Sockets
open System.Text

[<EntryPoint>]
let main _argv =
    let endPoint = new IPEndPoint(IPAddress.Any, 8080)
    let server = new Socket(AddressFamily.InterNetwork,
                            SocketType.Stream,
                            ProtocolType.Tcp)
    server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true)
    server.Bind(endPoint)
    server.Listen(10)
    
    printfn "client started (%s)" <| server.LocalEndPoint.ToString()
    
    let rec serve () =
        let client = server.Accept()
        printfn "client accepted (%s)" <| client.RemoteEndPoint.ToString()
        
        let buffer = [| for _ in 1..10 -> byte(0) |]
        let stream = new MemoryStream()
        let rec read () =
            let len = client.Receive(buffer)
            if len = 0 then
                ()
            else
                printfn "%s" <| Encoding.Default.GetString(buffer, 0, len)
                stream.Write(buffer, 0, len)
                read ()
        read ()
        
        let buffer = stream.GetBuffer()
        client.Send(buffer, buffer.Length, SocketFlags.None) |> ignore
        client.Close()
        
        serve ()
    
    serve () |> ignore
    
    0