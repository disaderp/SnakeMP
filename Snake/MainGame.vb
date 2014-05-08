Imports System.IO

Module MainGame
    Public snake As Snake
    Public p2snake As Snake
    Dim thr As Threading.Thread
    Dim thr2 As Threading.Thread
    Public Const Width As Integer = 80
    Public Const Height As Integer = 40
    Public points As Integer = 0
    Public p2points As Integer = 0
    Dim nextpos As Tile.Direction
    Public pos As Tile.Direction
    Public isMP As Boolean = False
    Sub Main()
        Console.SetWindowSize(Width, Height)
        Console.Write("SP or MP?")
        If Console.ReadLine().ToLower.StartsWith("s") Then
            Items.map.Add(New Item(Rand(0, MainGame.Width), Rand(0, MainGame.Height), 3))

            snake = New Snake(6, Tile.Direction.Right, 40, 12, 210)
            p2snake = New Snake(1, 0, -2, -2, 0)

            thr = New Threading.Thread(AddressOf UpdateSP)
            thr.Start()
        Else
            isMP = True
            Console.Write("Enter name: ")
            'Items.map.Add(New Item(Rand(0, MainGame.Width), Rand(0, MainGame.Height), 3))
            If Net.connect(Console.ReadLine()) = 1 Then
                snake = New Snake(6, Tile.Direction.Right, 40, 12, 100)
                p2snake = New Snake(6, Tile.Direction.Left, 37, 11, 100)
                nextpos = Tile.Direction.Right
            Else
                snake = New Snake(6, Tile.Direction.Left, 37, 11, 100)
                p2snake = New Snake(6, Tile.Direction.Right, 40, 12, 100)
                nextpos = Tile.Direction.Left
            End If

            thr2 = New Threading.Thread(AddressOf Net.Receive)
            thr2.Start()
            thr = New Threading.Thread(AddressOf UpdateMP)
            thr.Start()
        End If

        While True
            Dim key As ConsoleKey = Console.ReadKey().Key

            If key = ConsoleKey.DownArrow Then
                If Not snake.Dir = Tile.Direction.Up Then
                    nextpos = Tile.Direction.Down
                End If
            End If
            If key = ConsoleKey.UpArrow Then
                If Not snake.Dir = Tile.Direction.Down Then
                    nextpos = Tile.Direction.Up
                End If
            End If
            If key = ConsoleKey.LeftArrow Then
                If Not snake.Dir = Tile.Direction.Right Then
                    nextpos = Tile.Direction.Left
                End If
            End If
            If key = ConsoleKey.RightArrow Then
                If Not snake.Dir = Tile.Direction.Left Then
                    nextpos = Tile.Direction.Right
                End If
            End If

        End While
    End Sub
    Sub PrintP1()
        Console.BackgroundColor = ConsoleColor.White
        Console.ForegroundColor = ConsoleColor.White
        For i As Integer = 0 To snake.Length - 1
            Console.SetCursorPosition(snake.Raw(i).X, snake.Raw(i).Y)
            Console.Write(" ")
        Next
    End Sub
    Sub PrintP2()
        Console.BackgroundColor = ConsoleColor.Cyan
        Console.ForegroundColor = ConsoleColor.Cyan
        For i As Integer = 0 To p2snake.Length - 1
            Console.SetCursorPosition(p2snake.Raw(i).X, p2snake.Raw(i).Y)
            Console.Write(" ")
        Next
    End Sub
    Sub UpdateMP()
        'Dim log As New StreamWriter("log.txt", False)
        While True
            'pos = nextpos
            Wait = True
            Assign(pos, nextpos)
            While Net.Wait
                Threading.Thread.Sleep(30)
            End While

            'snake.Dir = pos
            Assign(snake.Dir, pos)
            snake.UpdatePosition()

            p2snake.UpdatePosition()

            'If Rand(0, 60) = 5 Then
            'Items.RandItems()
            'End If
            snake.CollisionCheck(1)
            p2snake.CollisionCheck(2)

            If Not snake.Alive(p2snake) Then
                Console.BackgroundColor = ConsoleColor.White
                Console.ForegroundColor = ConsoleColor.Red
                Console.SetCursorPosition(0, 0)
                Console.WriteLine("You've lost")
                thr2.Abort()
                Net.Lost()
                'Net.Wait = True
                thr.Abort()
            End If
            If Not p2snake.Alive(snake) Then
                Console.BackgroundColor = ConsoleColor.White
                Console.ForegroundColor = ConsoleColor.Red
                Console.SetCursorPosition(0, 0)
                Console.WriteLine("You've won")
                'thr2.Abort()
                'Net.Wait = True
                thr.Abort()
            End If

            'Net.Wait = True

            Console.Clear()
            Console.SetCursorPosition(0, 2)
            Console.Write(Net.name & "'s Snake")
            Console.SetCursorPosition(0, 3)
            Console.Write("Points: " & points)
            Console.SetCursorPosition(0, 4)
            Console.Write("Length: " & snake.Length)
            Console.SetCursorPosition(0, 5)
            Console.Write("Speed: " & ((snake.Time - 100) / -30))

            Console.SetCursorPosition(0, 7)
            Console.Write(Net.p2name & "'s Snake")
            Console.SetCursorPosition(0, 8)
            Console.Write("Points: " & p2points)
            Console.SetCursorPosition(0, 9)
            Console.Write("Length: " & p2snake.Length)


            PrintP1()
            PrintP2()


            Items.DrawItems()

            Console.ForegroundColor = ConsoleColor.White
            Console.SetCursorPosition(0, 0)
            'log.WriteLine("P1(0): X:" & snake.Raw(0).X & ", Y:" & snake.Raw(0).Y & ", Dir: " & snake.Dir.ToString)
            'log.WriteLine("P2(0): X:" & p2snake.Raw(0).X & ", Y:" & p2snake.Raw(0).Y & ", Dir: " & p2snake.Dir.ToString)
            'log.WriteLine()
            'log.Flush()
            Threading.Thread.Sleep(snake.Time)
        End While
    End Sub
    Sub UpdateSP()
        While True

            snake.Dir = nextpos
            snake.UpdatePosition()

            If Rand(0, 60) = 5 Then
                Items.RandItems()
            End If
            snake.CollisionCheck(1)

            If Not snake.Alive(p2snake) Then
                Console.BackgroundColor = ConsoleColor.White
                Console.SetCursorPosition(0, 0)
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("You've lost")
                thr.Abort()
            End If


            Console.Clear()
            Console.SetCursorPosition(0, 2)
            Console.Write("Snake 2014")
            Console.SetCursorPosition(0, 3)
            Console.Write("Points: " & points)
            Console.SetCursorPosition(0, 4)
            Console.Write("Length: " & snake.Length)
            Console.SetCursorPosition(0, 5)
            Console.Write("Speed: " & ((snake.Time - 210) / -30))


            PrintP1()

            Items.DrawItems()

            Console.ForegroundColor = ConsoleColor.White
            Console.SetCursorPosition(0, 0)
            Threading.Thread.Sleep(snake.Time)
        End While
    End Sub
    Sub Assign(ByRef dest As Tile.Direction, ByVal source As Tile.Direction)
        If source = Tile.Direction.Down Then
            dest = Tile.Direction.Down
        ElseIf source = Tile.Direction.Left Then
            dest = Tile.Direction.Left
        ElseIf source = Tile.Direction.Right Then
            dest = Tile.Direction.Right
        ElseIf source = Tile.Direction.Up Then
            dest = Tile.Direction.Up
        End If
    End Sub
End Module
