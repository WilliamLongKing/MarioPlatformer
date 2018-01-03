Module Background
    Structure background
        Dim picture As Bitmap
        Dim position As Point
        Dim width As Integer
        Dim height As Integer
    End Structure
    Public backdrop As background
    Public g As Graphics
    Public offG As Graphics
    Public imageOffScreen As Image
    Public Sub LoadBackground()
        backdrop.position.X = 0
        backdrop.position.Y = 0
        backdrop.picture = New Bitmap(IO.Path.GetDirectoryName(Application.ExecutablePath) & "\pics\Desert.png")
        backdrop.width = backdrop.picture.Width
        backdrop.height = backdrop.picture.Height
    End Sub
    Public Sub BackgroundDraw()
        offG.DrawImage(backdrop.picture, 0, 0)
        'offG.Clear(Color.Black)
    End Sub
    Public Declare Sub play Lib "winmm.dll" Alias "mciExecute" (ByVal command As String)
End Module
