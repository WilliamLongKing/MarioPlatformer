Public Class Form1
    Dim MarioAcceleration As Integer = 2
    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Left Then
            mario.Acceleration.X = -MarioAcceleration
            mario.facingright = False
        End If
        If e.KeyCode = Keys.Right Then
            mario.Acceleration.X = MarioAcceleration
            mario.facingright = True
        End If
        If e.KeyCode = Keys.Up Then

            If mario.onWall = True And mario.onfloor = False Then
                play("play jump.wav")
                If mario.facingright Then
                    mario.Speed.X = -10
                    mario.facingright = False
                Else
                    mario.Speed.X = 10
                    mario.facingright = True
                End If
                mario.Speed.Y = -mario.Acceleration.Y
            End If
            If mario.onfloor = True Then
                play("play jump.wav")
                mario.Speed.Y = -mario.Acceleration.Y
                mario.onfloor = False
            End If
        End If
    End Sub

    Private Sub Form1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Right Or e.KeyCode = Keys.Left Then
            mario.Acceleration.X = 0
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Loadguy(mario, "mario.png", 15, 15, 18, 300, 300)
        Call Loadguy(floors(0), "platform.png", 1, 0, 0, 400, 100)
        Call Loadguy(floors(1), "platform0.png", 1, 0, 0, 200, 200)
        Call Loadguy(floors(2), "floor.png", 1, 0, 0, 300, 300) 'LEAVE THIS AS FLOOR 
        Call Loadguy(floors(3), "pow1.bmp", 1, 0, 0, 700, 400)
        Call Loadguy(floors(4), "floor.png", 1, 0, 0, 500, 500) 'LEAVE THIS AS FLOOR
        Call Loadguy(floors(5), "floor.png", 1, 0, 0, 500, -50)
        Call Loadguy(floors(6), "pipe.png", 1, 0, 0, 885, 500)
        Call Loadguy(floors(7), "wall.png", 1, 0, 0, 800, 0)
        Call Loadguy(floors(8), "wall.png", 1, 0, 0, 1000, 0)
        Call Loadguy(floors(9), "platform.png", 1, 10, 0, 500, -200)
        Call Loadguy(floors(10), "platform0.png", 1, 0, 10, 300, 200)
        Call Loadguy(Enemies(0), "army.png", 10, 5, 0, 0, 0)
        Call Loadguy(portalA, "portals.png", 4, 5, 0, 0, 0)
        Call Loadguy(portalB, "portals.png", 4, 5, 0, 500, 50)

        Enemies(0).Patrol = True
        Enemies(0).Acceleration.X = 2
        Call SetEnemyFloor(Enemies(0), floors(2))
        Call Loadguy(Enemies(1), "army.png", 10, 5, 0, 0, 0)
        Enemies(1).Patrol = True
        Enemies(1).Acceleration.X = 2
        Call SetEnemyFloor(Enemies(1), floors(4))
        Call LoadBackground()
        Call DrawScreenSet()
        mario.Speed.X = 0
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        play("play Super_Mario_Brothers_1_Music_Main_Theme_Overworld.wav")
        Call ScreenDraw()
        Call MoveFloor(floors(9), New Point(750, 650), New Point(0, 0))
        Call MoveFloor(floors(10), New Point(300, 250), New Point(0, 0))
        Call moveguy(mario)
        Call animatemario()
        Call MoveEnemies()
        Call AnimateEnemy()
        Call CheckEnemyCollision()
        Label1.Text = mario.onWall
        Label2.Text = mario.Speed.X
    End Sub
    Public Sub DrawScreenSet()
        imageOffScreen = backdrop.picture.Clone
        picDrawOnScreen.Left = backdrop.position.X
        picDrawOnScreen.Top = backdrop.position.X
        picDrawOnScreen.Width = backdrop.width
        picDrawOnScreen.Height = backdrop.height
    End Sub
    Public Sub ScreenDraw()
        Dim index As Integer
        g = picDrawOnScreen.CreateGraphics
        offG = Graphics.FromImage(imageOffScreen)
        Call BackgroundDraw()
        Call guyDraw(mario)
        For index = 0 To numfloors
            Call guyDraw(floors(index))
        Next
        For index = 0 To numEnemies
            Call guyDraw(Enemies(index))
        Next
        Call guyDraw(portalA)
        Call guyDraw(portalB)
        g.DrawImage(imageOffScreen, 0, 0)
        g.Dispose()
        offG.Dispose()
    End Sub

    Public Sub MoveEnemies()
        Dim index As Integer
        For index = 0 To numEnemies
            moveguy(Enemies(index))
        Next
    End Sub
End Class
