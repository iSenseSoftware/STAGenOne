
Public Class frmSensorID


    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        'Verify boxes all filled
        MsgBox("Add code to verify that all boxes are filled")
        Me.Hide()
        'Show testform
        frmTestForm.Show()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        'hide form
        Me.Hide()
        'run end test routine
        EndTest()
    End Sub

    Private Sub frmSensorID_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        'hide form
        Me.Hide()
        'run end test routine
        EndTest()
    End Sub

    Private Sub frmSensorID_Load(sender As Object, e As EventArgs) Handles Me.Load
        PopulateSensorID()
    End Sub
    Public Property SensorHeader As String
        Get
            Dim strHeader As String = ""   'string to return with a comma separated list of the sensor IDs
            Dim txtTextBoxItem As Control
            Dim strTextBoxName As String
            Dim intLoop As Integer          ' loop counter variabls

            For intLoop = 1 To cfgGlobal.CardConfig * 16
                For Each txtTextBoxItem In Me.Controls
                    If TypeOf txtTextBoxItem Is TextBox Then
                        strTextBoxName = Microsoft.VisualBasic.Right(txtTextBoxItem.Name, 2)
                        If CInt(strTextBoxName) = intLoop Then
                            strHeader = strHeader & txtTextBoxItem.Text & ","
                        End If
                    End If
                Next
            Next
            Return strHeader
        End Get
        Set(value As String)
            'Doesn't set anything
        End Set
    End Property
End Class