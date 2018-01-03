Module ModSprite
    Structure sprite
        Dim Picture As Bitmap
        Dim CellWidth As Integer
        Dim CellHeight As Integer
        Dim CellCount As Integer
        Dim celltop As Integer
        Dim StartPosition As Point
        Dim Position As Point
        Dim Speed As Point
        Dim MaxSpeed As Point
        Dim Acceleration As Point
        Dim mRectangle As Rectangle
        Dim facingright As Boolean
        Dim onfloor As Boolean
        Dim onWall As Boolean
        Dim Patrol As Boolean
    End Structure

    Public mario As sprite
    Public Const gravity As Integer = 1
    Public Const numfloors As Integer = 10
    Public floors(numfloors) As sprite
    Public Const numEnemies As Integer = 1
    Public Enemies(numEnemies) As sprite
    Public portalA As sprite
    Public portalB As sprite


    Public Sub Loadguy(ByRef guy As sprite, ByVal filename As String, ByVal cells As Integer, ByVal xspeed As Integer, ByVal yspeed As Integer, ByVal xposition As Integer, ByVal yposition As Integer)
        guy.Picture = Image.FromFile(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\" + filename)
        guy.CellCount = cells
        guy.CellWidth = guy.Picture.Width
        guy.CellHeight = guy.Picture.Height / guy.CellCount
        guy.StartPosition.X = xposition
        guy.StartPosition.Y = yposition
        guy.Position.X = guy.StartPosition.X
        guy.Position.Y = guy.StartPosition.Y
        guy.Speed.X = xspeed
        guy.Speed.Y = yspeed
        guy.Acceleration.X = 0
        guy.Acceleration.Y = guy.Speed.Y
        guy.mRectangle.Width = guy.CellWidth
        guy.mRectangle.Height = guy.CellHeight
        guy.mRectangle.X = guy.Position.X
        guy.mRectangle.Y = guy.Position.Y
        guy.MaxSpeed.X = guy.Speed.X
        guy.MaxSpeed.Y = guy.Speed.Y
    End Sub
    Public Sub moveguy(ByRef guy As sprite)
        Dim index As Integer
        Call AccelerateSprite(guy)
        Call SpriteFriction(guy)
        guy.Speed.Y += gravity
        guy.Position.X += guy.Speed.X
        For index = 0 To numfloors
            If checkBoxCollision(guy, floors(index)) Then
                If guy.Speed.X > 0 Then
                    guy.Position.X = floors(index).Position.X - guy.CellWidth
                ElseIf guy.Speed.X < 0 Then
                    guy.Position.X = floors(index).Position.X + floors(index).CellWidth
                Else
                    If floors(index).Speed.X > 0 Then
                        guy.Position.X = floors(index).Position.X + floors(index).CellWidth
                    ElseIf floors(index).Speed.X < 0 Then
                        guy.Position.X = floors(index).Position.X - guy.CellWidth
                    End If
                End If
                guy.Speed.X = 0
                    guy.onWall = True
                    If guy.Patrol = True Then
                        guy.Acceleration.X *= -1
                    End If
                    Exit For
            End If
            guy.onWall = False
        Next

        If guy.Patrol = True Then
            If guy.Acceleration.X > 0 Then
                guy.Position.X += guy.CellWidth
            Else
                guy.Position.X -= guy.CellWidth
            End If

        End If

        guy.Position.Y += guy.Speed.Y

        For index = 0 To numfloors
            If checkBoxCollision(guy, floors(index)) = True Then
                'If floors(index).Speed.Y > 0 Then
                'ElseIf floors(index).Speed.Y < 0 Then
                'Else
                If guy.Speed.Y > 0 Then
                    guy.Position.Y = floors(index).Position.Y - guy.CellHeight
                    guy.onfloor = True
                    'If floors(index).Speed.X <> 0 Then
                    '    guy.Position.X += floors(index).Speed.X
                    'End If
                Else
                    guy.Position.Y = floors(index).Position.Y + floors(index).CellHeight
                End If
            'End If
            guy.Speed.Y = 0
            Exit For
            Else
            guy.onfloor = False
            End If
        Next

        If guy.Patrol = True Then
            If guy.Acceleration.X > 0 Then
                guy.Position.X -= guy.CellWidth
            Else
                guy.Position.X += guy.CellWidth
            End If
            If guy.Speed.Y > 0 Then
                guy.Position.X -= guy.Speed.X
                guy.Position.Y -= guy.Speed.Y
                guy.Acceleration.X *= -1
            End If

        End If

        If guy.Position.Y > 600 Then
            'Form1.Hide()
            'Youdied.Show()
            'Form1.Show()
            guy.Position.Y = 300
            guy.Speed.Y = 0
        End If
        guy.mRectangle.X = guy.Position.X
        guy.mRectangle.Y = guy.Position.Y

    End Sub
    Public Sub guyDraw(ByRef guy As sprite)
        Dim tempRec As Rectangle
        Dim offset As Point

        offset.X = (639 - mario.CellWidth) / 2
        offset.Y = (433 - mario.CellWidth) / 2

        tempRec = guy.mRectangle
        tempRec.X -= mario.mRectangle.X - offset.X
        tempRec.Y -= mario.mRectangle.Y - offset.Y

        offG.DrawImage(guy.Picture, tempRec, 0, guy.celltop, guy.CellWidth, guy.CellHeight, System.Drawing.GraphicsUnit.Pixel)
    End Sub
    Public Sub animatemario()
        If mario.Speed.X = 0 And mario.Speed.Y = 0 Then
            If mario.facingright = True Then
                mario.celltop = mario.CellHeight * 6
            Else
                mario.celltop = mario.CellHeight * 7
            End If
        End If
        If mario.Speed.X <> 0 And mario.Speed.Y = 0 Then
            Call animateRunning()
        End If
        If mario.onfloor = False Then
            If mario.facingright = True Then
                mario.celltop = mario.CellHeight * 8
            Else
                mario.celltop = mario.CellHeight * 9
            End If
        End If
        'animateDying()
    End Sub
    Public Sub animateRunning()
        If mario.facingright = True Then
            If mario.celltop = mario.CellHeight * 0 Then
                mario.celltop = mario.CellHeight * 1
            ElseIf mario.celltop = mario.CellHeight * 1 Then
                mario.celltop = mario.CellHeight * 2
            Else
                mario.celltop = mario.CellHeight * 0
            End If
        Else
            If mario.celltop = mario.CellHeight * 3 Then
                mario.celltop = mario.CellHeight * 4
            ElseIf mario.celltop = mario.CellHeight * 4 Then
                mario.celltop = mario.CellHeight * 5
            Else
                mario.celltop = mario.CellHeight * 3
            End If
        End If
    End Sub
    Public Sub animateDying()
        If mario.celltop = mario.CellHeight * 10 Then
            mario.celltop = mario.CellHeight * 11
        ElseIf mario.celltop = mario.CellHeight * 11 Then
            mario.celltop = mario.CellHeight * 12
        ElseIf mario.celltop = mario.CellHeight * 12 Then
            mario.celltop = mario.CellHeight * 13
        Else

            mario.celltop = mario.CellHeight * 10
        End If
    End Sub
    Public Function checkBoxCollision(ByVal pic1 As sprite, ByVal pic2 As sprite)
        Dim pic1Top As Integer = pic1.Position.Y
        Dim pic1bottom As Integer = pic1.Position.Y + pic1.CellHeight
        Dim pic1left As Integer = pic1.Position.X
        Dim pic1right As Integer = pic1.Position.X + pic1.CellWidth
        Dim pic2Top As Integer = pic2.Position.Y
        Dim pic2bottom As Integer = pic2.Position.Y + pic2.CellHeight
        Dim pic2left As Integer = pic2.Position.X
        Dim pic2right As Integer = pic2.Position.X + pic2.CellWidth
        If pic1Top < pic2bottom Then
            If pic1bottom > pic2Top Then
                If pic1left < pic2right Then
                    If pic1right > pic2left Then
                        Return True
                    End If
                End If
            End If
        End If
        Return False
    End Function

    Public Sub AccelerateSprite(ByRef guy As sprite)
        guy.Speed.X += guy.Acceleration.X
        If guy.Speed.X > guy.MaxSpeed.X Then
            guy.Speed.X = guy.MaxSpeed.X
        End If
        If guy.Speed.X < -guy.MaxSpeed.X Then
            guy.Speed.X = -guy.MaxSpeed.X
        End If
        If guy.Speed.Y > 50 Then
            guy.Speed.Y = 50
        End If
    End Sub
    Public Sub SpriteFriction(ByRef guy As sprite)
        'If guy.onfloor = True Then
        If guy.Speed.X > 0 Then
            guy.Speed.X -= 1
        End If
        If guy.Speed.X < 0 Then
            guy.Speed.X += 1
        End If
        ' End If
    End Sub

    Public Sub SetEnemyFloor(ByRef enemy As sprite, ByVal floor As sprite)
        enemy.Position.X = floor.Position.X
        enemy.Position.Y = floor.Position.Y - enemy.CellHeight
    End Sub
    Public Sub AnimateEnemy()
        Dim index As Integer
        For index = 0 To numEnemies
            If Enemies(index).Speed.X > 0 Then
                If Enemies(index).celltop = Enemies(index).CellHeight * 0 Then
                    Enemies(index).celltop = Enemies(index).CellHeight * 1
                Else
                    Enemies(index).celltop = Enemies(index).CellHeight * 0
                End If
            Else
                If Enemies(index).celltop = Enemies(index).CellHeight * 2 Then
                    Enemies(index).celltop = Enemies(index).CellHeight * 3
                Else
                    Enemies(index).celltop = Enemies(index).CellHeight * 2
                End If
            End If
        Next
    End Sub

    Public Sub CheckEnemyCollision()
        Dim index As Integer
        For index = 0 To numEnemies
            If checkBoxCollision(mario, Enemies(index)) Then
                mario.Speed.Y = -22
                mario.Speed.X = Enemies(index).Speed.X * 1000
            End If
        Next
    End Sub
    Public Sub MoveFloor(ByRef P As sprite, ByVal max As Point, ByVal min As Point)
        If P.Speed.X > 0 Then
            If P.Position.X > max.X Then
                P.Speed.X *= -1
            End If
        Else
            If P.Position.X < min.X Then
                P.Speed.X *= -1
            End If
        End If
        If P.Speed.Y > 0 Then
            If P.Position.Y > max.Y Then
                P.Speed.Y *= -1
            End If
        Else
            If P.Position.Y < min.Y Then
                P.Speed.Y *= -1
            End If
        End If
        P.Position.X += P.Speed.X
        P.Position.Y += P.Speed.Y
        P.mRectangle.X = P.Position.X
        P.mRectangle.Y = P.Position.Y
    End Sub
End Module
