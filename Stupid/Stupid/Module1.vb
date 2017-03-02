Module Module1
    Dim coins As Integer ' only global variable :'(
    Dim steps As Integer ' Nevermind.
    Sub Main()
        Console.ForegroundColor = ConsoleColor.White
        Dim temp As Integer
        Console.Clear()
        Console.WriteLine("welcome to treasure hunt") ' basic menu system
        Console.WriteLine("1. Play")
        Console.WriteLine("2. Exit.")
        Try
            temp = Console.ReadLine()
        Catch ex As Exception
            Console.WriteLine("Invalid") ' there's gonna be a bunch of trycatches, it's a quick and dirty way to make sure the input is right
            Main()
        End Try
        If temp = 1 Then
            info()
        ElseIf temp = 2 Then
            Environment.Exit(0.0) 'if 1 continue, else kill process
        Else
            Main()
        End If
    End Sub
    Sub info()
        Dim x As Integer
        Dim y As Integer ' some working integers for setup
        Dim b As Integer
        Dim c As Integer
        Console.WriteLine("Enter grid size X")
        Try
            x = Console.ReadLine()
        Catch ex As Exception
            info() ' get grid sizes and validate
            x -= 1 'we'll - 1 here because counting is weird
        End Try
        Console.WriteLine("Enter grid size Y")
        Try
            y = Console.ReadLine()
        Catch ex As Exception
            info()
        End Try
        Console.WriteLine("Enter amount of bandits")
        Try
            b = Console.ReadLine()
        Catch ex As Exception
            info()
        End Try
        Console.WriteLine("Enter amount of chests") 'same but for the rest of the vars
        Try
            c = Console.ReadLine()
        Catch ex As Exception
            info()
        End Try
        pirates(x, y, b, c)
    End Sub
    Sub pirates(x As Integer, y As Integer, b As Integer, c As Integer)
        Dim pirates(y, x)
        Dim _pirates As New Random
        Dim foo As Integer
        Dim wiz As Integer

        For i = 1 To b

            foo = _pirates.Next(0, y + 1)
            wiz = _pirates.Next(0, x + 1)
            pirates(foo, wiz) = 1 'basic RNG with checks to stop it being on the starting tile 
            If pirates(y, 1) = 1 Then
                pirates(y, 1) = 0
                i -= 1
            End If
        Next
        '
        chests(x, y, b, c, pirates)
    End Sub
    Sub chests(x, y, b, c, pirates)
        Dim chests(y, x)
        Dim _chests As New Random
        Dim foo As Integer
        Dim wiz As Integer

        For i = 1 To c
            foo = _chests.Next(0, y + 1)
            wiz = _chests.Next(0, x + 1)
            If chests(y, 1) = 1 Then
                chests(y, 1) = 0
                i -= 1 'same here, also makes sure it can't be in the same place as the pirate

            End If
            If pirates(foo, wiz) = 0 Then
                chests(foo, wiz) = 1

            Else
                i -= 1
            End If


        Next
        InitialGrid(x, y, chests, pirates)
    End Sub
    Sub InitialGrid(x, y, chests, pirates)
        Dim player(1)
        player(0) = y
        player(1) = 0
        Console.Clear()
        For i = 0 To y
            Console.WriteLine()
            Console.WriteLine()
            For k = 0 To x
                If player(0) = i And player(1) = k Then
                    Console.ForegroundColor = ConsoleColor.Yellow
                    Console.Write(" X ")
                    Console.ForegroundColor = ConsoleColor.White ' initally draws the grid, could of got rid of this, but passing variables-

                    Console.Write(" _ ")
                End If

            Next



        Next
        movement(x, y, chests, pirates, player)
    End Sub

    Sub movement(x, y, chests, pirates, player)
        Dim ymov As Integer
        Dim xmov As Integer
        Try
            Console.WriteLine("How many up? ( - for down")
            ymov = Console.ReadLine
        Catch ex As Exception
            movement(x, y, chests, pirates, player)
        End Try
        Try
            Console.WriteLine("How many right? ( - for left") ' this movement is called later, checks for being in the grids area
            xmov = Console.ReadLine
        Catch ex As Exception
            movement(x, y, chests, pirates, player)
        End Try
        If player(0) - ymov <= 0 Then
            Console.WriteLine(" OUT OF BOUNDS ")
            Threading.Thread.Sleep(2000)
            movement(x, y, chests, pirates, player)
        ElseIf player(0) - ymov >= y + 1 Then
            Console.WriteLine(" OUT OF BOUNDS ")
            Threading.Thread.Sleep(2000)
            movement(x, y, chests, pirates, player)
        End If
        If player(1) + xmov <= -1 Then

            Console.WriteLine(" OUT OF BOUNDS ")
            Threading.Thread.Sleep(2000)
            movement(x, y, chests, pirates, player)
        ElseIf player(1) + xmov >= x + 1 Then
            Console.WriteLine(" OUT OF BOUNDS ")
            Threading.Thread.Sleep(2000)
            movement(x, y, chests, pirates, player)
        End If
        player(0) = player(0) - ymov
        player(1) = player(1) + xmov
        steps += 1
        detection(x, y, chests, pirates, player)


    End Sub
    Sub detection(x, y, chests, pirates, player)
        Dim chestnumber As Integer
        For i = 0 To x
            For k = 0 To y
                If chests(i, k) = 1 Then
                    chestnumber += 1
                Else
                End If
            Next
        Next
        Dim player2d(y, x)
        player2d(player(0), player(1)) = 1
        For i = 0 To y
            For k = 0 To x
                If player2d(i, k) = 1 And pirates(i, k) = 1 Then
                    coins = 0
                End If
                If player2d(i, k) = 1 And (chests(i, k) = 1 Or chests(i, k) = 2 Or chests(i, k) = 3) Then ' this is just a bunch of -
                    coins += 10 ' checks to check if array values line up
                    chests(i, k) += 1
                    If chests(i, k) = 4 Then
                        chests(i, k) = Nothing
                        pirates(i, k) = 1 ' switches chest's for pirates 
                        chestnumber -= 1
                    End If
                End If
            Next
        Next
        If coins = 100 Then
            win()
        End If
        If chestnumber = 0 Then
            lose()
        End If
        RefreshGrid(x, y, chests, pirates, player)

    End Sub
    Sub RefreshGrid(x, y, chests, pirates, player)
        Console.Clear()
        For i = 1 To y
            Console.WriteLine()
            Console.WriteLine()
            For k = 0 To x
                If player(0) = i And player(1) = k Then
                    Console.ForegroundColor = ConsoleColor.Yellow
                    Console.Write(" X ")
                    Console.ForegroundColor = ConsoleColor.White ' draws the grid, acutall version with just blanks
                Else
                    Console.Write(" _ ")
                End If
            Next
        Next
        Console.WriteLine("Current coins:" & coins)
        Console.WriteLine("Steps taken:" & steps) ' adds some basic counters
        Console.ReadLine()
        movement(x, y, chests, pirates, player)

    End Sub
    Sub win()
        Console.Clear()
        Console.WriteLine("YOU WIN!")
        Console.WriteLine("Press any key to close the program")
        Console.Read()
        Environment.Exit(0.1)
    End Sub ' super simple win lose subs
    Sub lose()
        Console.Clear()
        Console.WriteLine("You lost the game.")
        Console.WriteLine("Press any key to close the program")
        Console.Read()
        Environment.Exit(0.2)
    End Sub
End Module