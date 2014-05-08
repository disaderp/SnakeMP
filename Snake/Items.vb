Module Items

    Public map As New List(Of Item)
    Enum Type
        Enlarge = 3
        Speed = 1
        Slow = 2
        Div = 0
    End Enum
    Sub RandItems()
        map.Add(New Item(Rand(0, MainGame.Width), Rand(0, MainGame.Height), Rand(0, 10000) Mod 3))
    End Sub
    Function Rand(ByVal Min As Integer, ByVal Max As Integer) As Integer
        Static Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function
    
    Sub DrawItems()
        Console.BackgroundColor = ConsoleColor.Black
        For i As Integer = 0 To map.Count - 1
            Console.SetCursorPosition(map(i).X, map(i).Y)
            If map(i).Type = Type.Enlarge Then
                Console.ForegroundColor = ConsoleColor.Green
                Console.Write("+")
            End If
            If map(i).Type = Type.Speed Then
                Console.ForegroundColor = ConsoleColor.Yellow
                Console.Write(">")
            End If
            If map(i).Type = Type.Slow Then
                Console.ForegroundColor = ConsoleColor.Blue
                Console.Write("<")
            End If
            If map(i).Type = Type.Div Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.Write("-")
            End If
        Next
    End Sub
End Module
Class Item
    Dim x1 As Integer
    Dim y1 As Integer
    Dim t1 As Type
    Sub New(_x As Integer, _y As Integer, _t As Type)
        x1 = _x
        y1 = _y
        t1 = _t
    End Sub
    ReadOnly Property X As Integer
        Get
            Return x1
        End Get
    End Property
    ReadOnly Property Y As Integer
        Get
            Return y1
        End Get
    End Property
    ReadOnly Property Type As Type
        Get
            Return t1
        End Get
    End Property
End Class