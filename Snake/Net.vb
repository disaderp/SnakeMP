Imports System.Net.Sockets
Imports System.Net
Imports System.Text.Encoding

Module Net
    Dim client As New UdpClient
#If DEBUG Then
    Dim server As New IPEndPoint(IPAddress.Parse("127.0.0.1"), 8881)
#Else
    Dim server As New IPEndPoint(IPAddress.Parse("188.116.56.69"), 8881)
#End If
    Public Wait As Boolean = False
    Public Run As Boolean = True
    Public name As String
    Public p2name As String

    Function connect(name1 As String) As Integer
        client.Send([Default].GetBytes("CONN" & name1), [Default].GetByteCount("CONN" & name1), server)
        client.Client.ReceiveTimeout = 1000000
        Console.WriteLine("Connected. Waiting for second player")
        Dim mess As String = [Default].GetString(client.Receive(server))
        If mess = "WAIT" Then
            Console.WriteLine("Server is busy. Reconnecting in 5s")
            Threading.Thread.Sleep(5000)
            Return connect(name1)
        End If
        name = name1
        p2name = mess.Split(":")(1)
        Return mess.Split(":")(0)
    End Function
    Sub Receive()
        While Run
            While Not Wait
                Threading.Thread.Sleep(30)
            End While
            Dim dir As Integer = 0 + MainGame.pos
            client.Send([Default].GetBytes(dir), [Default].GetByteCount(dir), server)
            Dim mess As String = [Default].GetString(client.Receive(server))
            If mess = "WON" Then
                Run = False
                Console.BackgroundColor = ConsoleColor.White
                Console.ForegroundColor = ConsoleColor.Red
                Console.SetCursorPosition(0, 0)
                Console.WriteLine("You've won")
                Continue While
            End If
            If mess = "FDISCO" Then
                Run = False
                Console.BackgroundColor = ConsoleColor.White
                Console.ForegroundColor = ConsoleColor.Red
                Console.SetCursorPosition(0, 0)
                Console.WriteLine("Your opponent's disconnected.")
                Continue While
            End If
            Dim actions As String() = mess.Split("*")
            MainGame.p2snake.Dir = actions(0)
            If actions(1).Contains(":") Then
                Items.map.Add(New Item(actions(1).Split(":")(0), actions(1).Split(":")(1), actions(1).Split(":")(2)))
            End If
            Wait = False

        End While
    End Sub
    Sub Lost()
        client.Send([Default].GetBytes("END"), [Default].GetByteCount("END"), server)
    End Sub
End Module
