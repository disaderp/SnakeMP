﻿Imports System.Net
Imports System.Net.Sockets
Imports System.Text.Encoding
Module Server
    Dim server As New UdpClient(8881)
    Dim client As IPEndPoint
    Dim match(2) As IPEndPoint
    Dim names(2) As String
    Dim Running As Boolean = False
    Dim i As Integer = 0
    Dim Time As Integer = 5
    Dim thr As Threading.Thread
    Sub Main()
        While True
            Try
                If Running Then
f:
                    If Not Running Then
                        Continue While
                    End If
                    Console.WriteLine("waiting for 1packet")
                    client = New IPEndPoint(IPAddress.Any, 8881)
                    Dim p1dir As String = ""
                    Dim p2dir As String = ""
                    Dim mess As String = [Default].GetString(server.Receive(client))
                    If mess = "END" Then
                        Running = False
                        i = 0
                        Console.WriteLine("Match finished##########")
                        If client.Equals(match(0)) Then
                            server.Send([Default].GetBytes("WON"), [Default].GetByteCount("WON"), match(1))
                        ElseIf client.Equals(match(1)) Then
                            server.Send([Default].GetBytes("WON"), [Default].GetByteCount("WON"), match(0))
                        End If
                        Continue While
                    End If
                    If client.Equals(match(0)) Then
                        p1dir = mess
                    ElseIf client.Equals(match(1)) Then
                        p2dir = mess
                    Else
                        server.Send([Default].GetBytes("WAIT"), [Default].GetByteCount("WAIT"), client)
                        GoTo f
                    End If
s:
                    If Not Running Then
                        Continue While
                    End If
                    Console.WriteLine("waiting for 2packet")
                    client = New IPEndPoint(IPAddress.Any, 8881)
                    mess = [Default].GetString(server.Receive(client))
                    If mess = "END" Then
                        Running = False
                        i = 0
                        Console.WriteLine("Match finished##########")
                        If client.Equals(match(0)) Then
                            server.Send([Default].GetBytes("WON"), [Default].GetByteCount("WON"), match(1))
                        ElseIf client.Equals(match(1)) Then
                            server.Send([Default].GetBytes("WON"), [Default].GetByteCount("WON"), match(0))
                        End If
                        Continue While
                    End If
                    If client.Equals(match(0)) Then
                        p1dir = mess
                    ElseIf client.Equals(match(1)) Then
                        p2dir = mess
                    Else
                        server.Send([Default].GetBytes("WAIT"), [Default].GetByteCount("WAIT"), client)
                        GoTo s
                    End If

                    Dim item As String = ""
                    If Rand(0, 60) = 5 Then
                        item = Rand(0, 80) & ":" & Rand(0, 29) & ":" & (Rand(0, 10000) Mod 4)
                    End If

                    Time = 5
                    server.Send([Default].GetBytes(p2dir & "*" & item), [Default].GetByteCount(p2dir & "*" & item), match(0))
                    server.Send([Default].GetBytes(p1dir & "*" & item), [Default].GetByteCount(p1dir & "*" & item), match(1))
                    Console.WriteLine("exchanged gameinfo")
                Else
                    Dim mess As String = [Default].GetString(server.Receive(client))
                    If mess.StartsWith("CONN") Then
                        If i = 0 Then
                            match(0) = client
                            names(0) = mess.Substring(4)
                            i = 1
                            Console.WriteLine("1 connected")

                        ElseIf i = 1 Then
                            match(1) = client
                            names(1) = mess.Substring(4)
                            i = 2
                            Running = True
                            Console.WriteLine("2 connected")
                            server.Send([Default].GetBytes("2:" & names(0)), [Default].GetByteCount("2:" & names(0)), match(1))
                            server.Send([Default].GetBytes("1:" & names(1)), [Default].GetByteCount("1:" & names(1)), match(0))
                            Console.WriteLine("sent basic info")
                            Time = 5
                            thr = New Threading.Thread(AddressOf Update)
                            thr.IsBackground = True
#If Not Debug Then
                             thr.Start()
#End If
                        End If
                    End If
                End If

            Catch ex As Exception
                Console.WriteLine(ex.ToString)
            End Try
        End While
    End Sub
    Function Rand(ByVal Min As Integer, ByVal Max As Integer) As Integer
        Static Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function
    Sub Update()
        While True
            If Time < 1 And Running Then
                Try
                    server.Send([Default].GetBytes("FDISCO"), [Default].GetByteCount("FDISCO"), match(0))
                Catch
                End Try
                Try
                    server.Send([Default].GetBytes("FDISCO"), [Default].GetByteCount("FDISCO"), match(1))
                Catch
                End Try
                Running = False
                i = 0
                Console.WriteLine("DISCONNECTED###########")
                thr.Abort()
            End If
            If Running Then
                Time -= 1
            End If
            Threading.Thread.Sleep(1000)
        End While
    End Sub
End Module