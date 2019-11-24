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
    server.Bind(endPoint)
    server.Listen(10)
    
    printfn "server started (%s)" <| server.LocalEndPoint.ToString()
    
    let rec serve () =
        let client = server.Accept()
        printfn "client accepted (%s)" <| client.RemoteEndPoint.ToString()
        
        let rec read () =
            let buffer = [| for _ in 1..10 -> byte(0) |]
            let len = client.Receive(buffer)
            if len = 0 then
                printfn "接続が切れました"
                client.Close()
                ()
            else
                printfn "%s" <| Encoding.Default.GetString(buffer, 0, len)
                client.Send(buffer) |> ignore
                read ()
        read ()

        client.Close()
        
        serve ()
    
    serve () |> ignore
    
    0