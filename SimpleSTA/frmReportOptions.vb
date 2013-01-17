Public Class frmReportOptions

    Private Sub btnSelectGlucoseFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectGlucoseFile.Click
        OpenFileDialog1.Title = "Select Glucose Test File"
        OpenFileDialog1.InitialDirectory = "C:\"
        OpenFileDialog1.ValidateNames = True
        OpenFileDialog1.DefaultExt = ".xml"
        OpenFileDialog1.Filter = "XML Files (*.xml)|*.xml"
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub btnSelectAPAPFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectAPAPFile.Click
        OpenFileDialog2.Title = "Select APAP Test File"
        OpenFileDialog2.InitialDirectory = "C:\"
        OpenFileDialog2.ValidateNames = True
        OpenFileDialog2.DefaultExt = ".xml"
        OpenFileDialog2.Filter = "XML Files (*.xml)|*.xml"
        OpenFileDialog2.ShowDialog()
    End Sub


    Private Sub OpenFileDialog1_FileOk(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        txtGlucoseTestFile.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub OpenFileDialog2_FileOk(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog2.FileOk
        txtAPAPTestFile.Text = OpenFileDialog2.FileName
    End Sub

    Private Sub btnCreateReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateReport.Click
        Dim newReport As New Report
        newReport.LoadTestFiles(txtAPAPTestFile.Text, txtGlucoseTestFile.Text)

    End Sub
End Class