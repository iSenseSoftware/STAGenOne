Imports System.IO
Imports System
Imports System.Xml.Serialization
Imports SimpleSTA.SharedModule
Public Class frmConfig
    Dim errors As String()
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        saveConfiguration()
    End Sub

    Private Sub frmConfig_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'populate comboboxes
        Dim rangeList As List(Of KeyValuePair(Of CurrentRange, String)) = New List(Of KeyValuePair(Of CurrentRange, String))
        rangeList.Add(New KeyValuePair(Of CurrentRange, String)(CurrentRange.one_uA, "0 - 1 uA"))
        rangeList.Add(New KeyValuePair(Of CurrentRange, String)(CurrentRange.ten_uA, "1 - 10 uA"))
        rangeList.Add(New KeyValuePair(Of CurrentRange, String)(CurrentRange.hundred_uA, "1 - 100 uA"))
        cmbRange.DataSource = rangeList
        cmbRange.ValueMember = "Key"
        cmbRange.DisplayMember = "Value"

        Dim filterList As List(Of KeyValuePair(Of FilterType, String)) = New List(Of KeyValuePair(Of FilterType, String))
        filterList.Add(New KeyValuePair(Of FilterType, String)(FilterType.FILTER_MOVING_AVG, "Moving Avg"))
        filterList.Add(New KeyValuePair(Of FilterType, String)(FilterType.FILTER_MEDIAN, "Median"))
        filterList.Add(New KeyValuePair(Of FilterType, String)(FilterType.FILTER_REPEAT_AVG, "Repeat Avg"))
        cmbFilterType.DataSource = filterList
        cmbFilterType.ValueMember = "Key"
        cmbFilterType.DisplayMember = "Value"

        Dim cardConfigList As List(Of KeyValuePair(Of CardConfiguration, String)) = New List(Of KeyValuePair(Of CardConfiguration, String))
        cardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.ONE_CARD_SIXTEEN_SENSORS, "1 Card, 16 Sensors"))
        cardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.TWO_CARD_THIRTY_TWO_SENSORS, "2 Card, 32 Sensors"))
        cardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.THREE_CARD_FOURTY_EIGHT_SENSORS, "3 Card, 48 Sensors"))
        cardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.FOUR_CARD_SIXTY_FOUR_SENSORS, "4 Card, 64 Sensors"))
        cardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.FIVE_CARD_EIGHTY_SENSORS, "5 Card, 80 Sensors"))
        cardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.SIX_CARD_NINETY_SIX_SENSORS, "6 Card, 96 Sensors"))
        cmbCardConfig.DataSource = cardConfigList
        cmbCardConfig.ValueMember = "Key"
        cmbCardConfig.DisplayMember = "Value"

        populateConfigurationForm()

    End Sub

    Private Sub populateConfigurationForm()
        Try
            loadConfiguration()
            txtRecordInterval.Text = config.RecordInterval
            txtBias.Text = config.Bias
            cmbRange.SelectedValue = config.Range
            cmbFilterType.SelectedValue = config.Filter
            txtSamples.Text = config.Samples
            txtNPLC.Text = config.NPLC
            txtAddress.Text = config.Address
            txtSTAID.Text = config.STAID
            txtDumpDir.Text = config.DumpDir
            cmbCardConfig.SelectedValue = config.CardConfig
        Catch ex As Exception
            MsgBox("An exception occurred:" & Environment.NewLine & ex.Message)
            Me.Close()
        End Try
    End Sub
    Private Sub saveConfiguration()
        Try
            If (formValidates()) Then
                Dim serializer As New XmlSerializer(config.GetType)
                MsgBox(My.Application.Info.DirectoryPath & configFileName)
                Dim writer As New StreamWriter("C:\htdocs\Config.xml")
                config.Bias = CDbl(txtBias.Text)
                config.RecordInterval = CDbl(txtRecordInterval.Text)
                config.Range = cmbRange.SelectedValue
                config.Filter = cmbFilterType.SelectedValue
                config.Samples = CLng(txtSamples.Text)
                config.NPLC = CLng(txtNPLC.Text)
                config.Address = txtAddress.Text
                config.STAID = txtSTAID.Text
                config.DumpDir = txtDumpDir.Text
                config.CardConfig = cmbCardConfig.SelectedValue
                serializer.Serialize(writer, config)
                writer.Close()
                MsgBox("Success")
            Else
                MsgBox(errors)
            End If
        Catch ex As Exception
            MsgBox("An exception occurred:" & Environment.NewLine & ex.Message & Environment.NewLine & ex.ToString)
            Me.Close()
        End Try
    End Sub
    Private Function formValidates() As Boolean
        ' Add validation functions here later
        Return True
    End Function

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class
