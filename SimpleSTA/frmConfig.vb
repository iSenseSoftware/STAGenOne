Imports System.IO
Imports System
Imports System.Xml.Serialization
Imports SimpleSTA.SharedModule
Public Class frmConfig
    Dim errors As String()
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            saveConfiguration()
            Me.Close()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

    Private Sub frmConfig_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'populate comboboxes
            Dim rangeList As List(Of KeyValuePair(Of CurrentRange, String)) = New List(Of KeyValuePair(Of CurrentRange, String))
            rangeList.Add(New KeyValuePair(Of CurrentRange, String)(CurrentRange.one_uA, "0 - 1 uA"))
            rangeList.Add(New KeyValuePair(Of CurrentRange, String)(CurrentRange.ten_uA, "1 - 10 uA"))
            rangeList.Add(New KeyValuePair(Of CurrentRange, String)(CurrentRange.hundred_uA, "1 - 100 uA"))
            cmbRange.DataSource = rangeList
            cmbRange.ValueMember = "Key"
            cmbRange.DisplayMember = "Value"

            Dim cardConfigList As List(Of KeyValuePair(Of CardConfiguration, String)) = New List(Of KeyValuePair(Of CardConfiguration, String))
            cardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.ONE_CARD_SIXTEEN_SENSORS, "1 Card, 16 Sensors"))
            cardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.TWO_CARD_THIRTY_TWO_SENSORS, "2 Card, 32 Sensors"))
            'cardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.THREE_CARD_FOURTY_EIGHT_SENSORS, "3 Card, 48 Sensors"))
            'cardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.FOUR_CARD_SIXTY_FOUR_SENSORS, "4 Card, 64 Sensors"))
            'cardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.FIVE_CARD_EIGHTY_SENSORS, "5 Card, 80 Sensors"))
            'cardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.SIX_CARD_NINETY_SIX_SENSORS, "6 Card, 96 Sensors"))
            cmbCardConfig.DataSource = cardConfigList
            cmbCardConfig.ValueMember = "Key"
            cmbCardConfig.DisplayMember = "Value"

            populateConfigurationForm()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

    Private Sub populateConfigurationForm()
        Try
            loadConfiguration()
            cmbRange.SelectedValue = config.Range
            txtAddress.Text = config.Address
            txtSTAID.Text = config.STAID
            cmbCardConfig.SelectedValue = config.CardConfig
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Me.Close()
        End Try
    End Sub
    Private Sub saveConfiguration()
        Try
            If (ValidateForm()) Then
                Dim serializer As New XmlSerializer(config.GetType)
                Dim writer As New StreamWriter(appDir & configFileName)
                config.Range = cmbRange.SelectedValue
                config.Address = txtAddress.Text
                config.STAID = txtSTAID.Text
                config.CardConfig = cmbCardConfig.SelectedValue
                serializer.Serialize(writer, config)
                writer.Close()
            Else
                MsgBox(errors)
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Me.Close()
        End Try
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Function ValidateForm() As Boolean
        Try
            ' Check each field against it's requirements and return true/false
            Dim boolValidates As Boolean = True
            ' Check record interval: integer non-null

            ' Check Bias: double, non-null

            ' Check Current Range: CURRENT_RANGE, non-null

            ' Check filter type: FILTER_TYPE, non-null

            ' Check samples: integer, non-null

            ' Check NPLC: integer, non-null

            ' Check address: non-null

            ' STA ID: non-null

            ' Card Configuration: CARD_CONFIGURATION, non-null
            Return boolValidates
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return False
        End Try
    End Function

End Class
