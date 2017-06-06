Public Class frmSensorID

    Private Sub frmSensorID_Activated(sender As Object, e As EventArgs) Handles Me.Activated ' Changed to activated from load to allow autopopulation to occur each time the form is activated
        PopulateSensorID()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        'Verify boxes all filled
        If ValidateForm() Then
            strSensorIDHeader = Me.SensorHeader
            Me.Hide()
            'Show testform
            frmTestForm.Show()

        Else
            MsgBox("Sensor ID cannot be Blank")
        End If


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

    Public Sub PopulateSensorID()
        Dim txtTextBoxItem As Control
        For Each txtTextBoxItem In Me.Controls
            If TypeOf txtTextBoxItem Is TextBox Then
                txtTextBoxItem.Text = strTestID + "-" + Microsoft.VisualBasic.Right(txtTextBoxItem.Name, 2)
            End If

        Next
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

    Private Function ValidateForm() As Boolean
       
        Dim boolValidates As Boolean = True

        Dim emptyTextBox = From txt In Me.Controls.OfType(Of TextBox)()
                      Where txt.Text.Length = 0
                      Select txt.Name
        If emptyTextBox.Any Then
            boolValidates = False
        End If

        Return boolValidates


        
    End Function

    Private Sub frmSensorID_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        PopulateSensorID()
    End Sub


End Class