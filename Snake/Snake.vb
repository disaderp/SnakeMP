Public Class Snake
    Private s As List(Of Tile)
    Private headdirection As Tile.Direction
    Private t As Integer
    Property Time As Integer
        Set(value As Integer)
            t = value
        End Set
        Get
            Return t
        End Get
    End Property
    ReadOnly Property Raw As List(Of Tile)
        Get
            Return New List(Of Tile)(s) 'jako wartosc, usuniecie referencji - #net
        End Get
    End Property
    Property Dir As Tile.Direction
        Set(value As Tile.Direction)
            headdirection = value
        End Set
        Get
            Return headdirection
        End Get
    End Property
    Sub UpdatePosition()
        s(0).Dir = headdirection
        If s(0).Dir = Tile.Direction.Down Then
            s(0).Y += 1
        End If
        If s(0).Dir = Tile.Direction.Left Then
            s(0).X -= 1
        End If
        If s(0).Dir = Tile.Direction.Right Then
            s(0).X += 1
        End If
        If s(0).Dir = Tile.Direction.Up Then
            s(0).Y -= 1
        End If
        For i As Integer = (s.Count - 1) To 1 Step -1
            If s(i).Dir = Tile.Direction.Down Then
                s(i).Y += 1
            End If
            If s(i).Dir = Tile.Direction.Left Then
                s(i).X -= 1
            End If
            If s(i).Dir = Tile.Direction.Right Then
                s(i).X += 1
            End If
            If s(i).Dir = Tile.Direction.Up Then
                s(i).Y -= 1
            End If
            s(i).Dir = s(i - 1).Dir
        Next
    End Sub
    Sub New(len As Integer, dir As Tile.Direction, x As Integer, y As Integer, speed As Integer)
        t = speed
        s = New List(Of Tile)
        headdirection = dir
        Dim ftile As New Tile(headdirection, x, y)
        s.Add(ftile)
        Dim x1 As Integer = x
        Dim y1 As Integer = y
        For i As Integer = 2 To len
            If dir = Tile.Direction.Down Then
                y1 -= 1
            End If
            If dir = Tile.Direction.Left Then
                x1 += 1
            End If
            If dir = Tile.Direction.Right Then
                x1 -= 1
            End If
            If dir = Tile.Direction.Up Then
                y1 += 1
            End If
            ftile = New Tile(headdirection, x1, y1)
            s.Add(ftile)
        Next
    End Sub
    Function Length() As Integer
        Return s.Count()
    End Function
    Sub Remove()
        s.RemoveAt(s.Count - 1)
    End Sub
    Sub Add()
        Dim x As Integer
        Dim y As Integer
        If s(0).Dir = Tile.Direction.Down Then
            x = s(0).X
            y = s(0).Y + 1
        End If
        If s(0).Dir = Tile.Direction.Left Then
            x = s(0).X - 1
            y = s(0).Y
        End If
        If s(0).Dir = Tile.Direction.Right Then
            x = s(0).X + 1
            y = s(0).Y
        End If
        If s(0).Dir = Tile.Direction.Up Then
            x = s(0).X
            y = s(0).Y - 1
        End If
        Dim newtile As New Tile(s(0).Dir, x, y)
        s.Reverse()
        s.Add(newtile)
        s.Reverse()
    End Sub
    Function Alive(s2 As Snake) As Boolean
        For i As Integer = 0 To s.Count - 1
            If s(i).X < 0 Or s(i).X > Width - 1 Or s(i).Y < 0 Or s(i).Y > Height - 1 Then
                Return False
            End If
            If i > 0 Then
                If s(i).X = s(0).X And s(i).Y = s(0).Y Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.BackgroundColor = ConsoleColor.Red
                    Console.SetCursorPosition(s(0).X, s(0).Y)
                    Console.Write(" ")
                    
                    Return False
                End If
            Else
                For j As Integer = 1 To s2.Length() - 1
                    If s(0).X = s2.Raw(j).X And s(0).Y = s2.Raw(j).Y Then
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.BackgroundColor = ConsoleColor.Red
                        Console.SetCursorPosition(s(0).X, s(0).Y)
                        Console.Write(" ")
                        Return False
                    End If
                Next
            End If
        Next
        Return True
    End Function
    Sub CollisionCheck(p As Integer)
        For i As Integer = 0 To s.Count - 1
            For j As Integer = 0 To map.Count - 1
                If s(i).X = map(j).X And s(i).Y = map(j).Y Then
                    If map(j).Type = Type.Enlarge Then
                        Me.Add()
                        map.RemoveAt(j)
                        If p = 1 Then
                            MainGame.points += 1
                        Else
                            MainGame.p2points += 1
                        End If
                        If Not isMP Then
                            map.Add(New Item(Rand(0, MainGame.Width), Rand(0, MainGame.Height), 3))
                        End If
                        Exit For
                    End If
                    If map(j).Type = Type.Div Then
                        If Me.Length > 1 Then
                            Me.Remove()
                        End If
                        map.RemoveAt(j)
                        Exit Sub
                    End If
                    If map(j).Type = Type.Speed Then
                        If Me.Time > 100 Then
                            'Me.Time -= 30
                            MainGame.snake.Time -= 30
                            MainGame.p2snake.Time -= 30
                        End If
                        map.RemoveAt(j)
                        Exit For
                    End If
                    If map(j).Type = Type.Slow Then
                        'Me.Time += 30
                        MainGame.snake.Time += 30
                        MainGame.p2snake.Time += 30
                        map.RemoveAt(j)
                        Exit For
                    End If
                End If
            Next
        Next
    End Sub
End Class