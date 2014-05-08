
Public Class Tile
    Enum Direction
        Right = 0
        Down = 1
        Left = 2
        Up = 3
    End Enum
    Private dir1 As Direction
    'Private num1 As Integer
    Private x1 As Integer
    Private y1 As Integer
    Sub New(dir As Direction, _x As Integer, _y As Integer)
        dir1 = dir
        ' num1 = index
        x1 = _x
        y1 = _y
    End Sub
    Property Dir As Direction
        Set(value As Direction)
            dir1 = value
        End Set
        Get
            Return dir1
        End Get
    End Property
    Property X As Integer
        Set(value As Integer)
            x1 = value
        End Set
        Get
            Return x1
        End Get
    End Property
    Property Y As Integer
        Set(value As Integer)
            y1 = value
        End Set
        Get
            Return y1
        End Get
    End Property
End Class