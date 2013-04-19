Imports System.IO
Imports System.Xml.Serialization
Imports Keithley.Ke37XX.Interop
Imports System.Runtime.InteropServices
Imports Ivi.Driver.Interop

' The SharedModule is, as the name suggests, a collection of shared utlity functions, enumerated variables
' and global variables for use in all objects and forms
Public Module SharedModule
    Public Const configFileName As String = "\Config.xml"
    Public config As New Configuration
    Public testSystemInfo As New TestSystem
    Public currentSwitch As Switch
    Public currentSource As SourceMeter
    Public currentTestFile As New TestFile
    Public currentCards() As Card
    Public switchDriver As New Ke37XX
    Public appDir As String
    Public boolSystemInfoLoaded As Boolean = False
    Public boolConfigLoaded As Boolean = False
    Public boolIOEstablished As Boolean = False
    Public boolTestFileLoaded As Boolean = False
    Public boolSeriesLoaded As Boolean = False
    ' The admin password to unlock the configuration settings is hardcoded.  In the future
    ' it may be desireable to incorporate user authentication / authorization modules for granular permissions
    Public strAdminPassword As String = "C0balt22"
    ' Declare Enums for configuration settings
    ' Because of a quirk of VB, a value of 0 for an enumerated variable is equivalent to Nothing, making validation difficult
    ' When the CurrentRange and FilterType enums are used for direct input to the test system switch we must subtract 1
    Public Enum CurrentRange
        one_uA = 1
        ten_uA = 2
        hundred_uA = 3
    End Enum
    Public Enum FilterType
        FILTER_REPEAT_AVG = 1
        FILTER_MOVING_AVG = 2
        FILTER_MEDIAN = 3
    End Enum
    Public Enum CardConfiguration
        ONE_CARD_SIXTEEN_SENSORS = 1
        TWO_CARD_THIRTY_TWO_SENSORS = 2
        THREE_CARD_FOURTY_EIGHT_SENSORS = 3
        FOUR_CARD_SIXTY_FOUR_SENSORS = 4
        FIVE_CARD_EIGHTY_SENSORS = 5
        SIX_CARD_NINETY_SIX_SENSORS = 6
    End Enum
    ' Load configuration from file and set the values in the global Configuration object config
    ' @TODO: Make the file location optional rather than hard-coded and store between program sessions
    Public Sub loadConfiguration()
        Try
            Dim serializer As New XmlSerializer(config.GetType)
            Dim reader As New StreamReader(appDir & configFileName)
            config = serializer.Deserialize(reader)
            reader.Close()
        Catch parseException As InvalidOperationException
            MsgBox("Invalid configuration file.  Delete " & appDir & configFileName)
            boolConfigLoaded = False
            frmMain.Close()
        Catch ex As Exception
            GenericExceptionHandler(ex)
            boolConfigLoaded = False
        End Try
    End Sub
    Public Sub initializeConfiguration()
        Try
            Dim serializer As New XmlSerializer(config.GetType)
            Dim writer As New StreamWriter(appDir & configFileName)
            serializer.Serialize(writer, config)
            writer.Close()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub loadSystemInfo()
        Try
            Dim serializer As New XmlSerializer(testSystemInfo.GetType)
            Dim reader As New StreamReader(config.SystemFileDirectory & "\SystemInfo.xml")
            testSystemInfo = serializer.Deserialize(reader)
            reader.Close()
        Catch parseException As InvalidOperationException
            MsgBox("Invalid System Information File.  Delete " & config.SystemFileDirectory & "\SystemInfo.xml")
            boolSystemInfoLoaded = False
            frmMain.Close()
        Catch ex As Exception
            GenericExceptionHandler(ex)
            boolSystemInfoLoaded = False
        End Try
    End Sub
    Public Sub initializeSystemInfo()
        Try
            Dim serializer As New XmlSerializer(testSystemInfo.GetType)
            System.IO.Directory.CreateDirectory(config.SystemFileDirectory)
            Dim writer As New StreamWriter(config.SystemFileDirectory & "\SystemInfo.xml")
            serializer.Serialize(writer, testSystemInfo)
            writer.Close()
        Catch ex As Exception
            GenericExceptionHandler(ex)
            boolSystemInfoLoaded = False
        End Try
    End Sub
    Public Function verifyConfiguration() As Boolean
        Try
            Dim verifies As Boolean = True
            If (config.Bias = Nothing) Then
                verifies = False
            End If
            If (config.RecordInterval = Nothing) Then
                verifies = False
            End If
            If (config.Range = Nothing) Then
                verifies = False
            End If
            If (config.Filter = Nothing) Then
                verifies = False
            End If
            If (config.Samples = Nothing) Then
                verifies = False
            End If
            If (config.NPLC = Nothing) Then
                verifies = False
            End If
            If (config.Address = Nothing Or config.Address = "") Then
                verifies = False
            End If
            If (config.STAID = Nothing Or config.STAID = "") Then
                verifies = False
            End If
            If (config.CardConfig = Nothing) Then
                verifies = False
            End If
            If (config.Resistor1Resistance <= 0 Or config.Resistor2Resistance <= 0 Or config.Resistor3Resistance <= 0) Then
                verifies = False
            End If
            Return verifies
        Catch comEx As COMException
            ComExceptionHandler(comEx)
            Return False
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return False
        End Try
    End Function
    Public Function verifySystemInfo() As Boolean
        Return True
    End Function
    ' As the name suggests, this is simply a wrapper for the System.DirectIO.WriteString method of the Ke37xx driver.
    Public Sub directIOWrapper(ByVal command As String)
        Try
            switchDriver.System.DirectIO.WriteString(command)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub Delay(ByVal milliseconds As Long)
        Try
            Dim watch As New Stopwatch
            watch.Start()
            Do

            Loop Until watch.ElapsedMilliseconds >= milliseconds
            watch.Stop()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Utility function to pad channel strings with leading zeroes
    Public Function strPad(ByVal input As String, ByVal padTo As Integer) As String
        Try
            If (input.Length < padTo) Then
                Do While (input.Length < padTo)
                    input = "0" & input
                Loop
            End If
            Return input
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Function
    Public Sub GenericExceptionHandler(ByVal theException As Exception)
        MsgBox(theException.GetType.ToString() & Environment.NewLine & theException.Message & Environment.NewLine & theException.ToString)
    End Sub
    Public Sub ComExceptionHandler(ByRef theException As COMException)
        If theException.ErrorCode = IviDriver_ErrorCodes.E_IVI_INSTRUMENT_STATUS Then
            ' ErrorQuery should give us more information
            Dim errCode As Integer = 0
            Dim errMsg As String = ""
            switchDriver.Utility.ErrorQuery(errCode, errMsg)
            ' Print the error
            MsgBox("Instrument Error: " & errCode & Environment.NewLine & errMsg)
        Else
            ' Print the exception
            If (theException.Message.Contains("Unknown resource")) Then
                MsgBox("Could not establish communication with the System Switch")
            Else
                MsgBox(theException.Message)
            End If
        End If
    End Sub
    Public Function GetBestDynamicRange(maxCurrent As Double) As CurrentRange
        Return CurrentRange.one_uA
    End Function
    Public Function ParseIDNForSerial(ByVal idnString As String) As String
        Try
            Dim splitString As String() = Split(idnString, ",")
            If (splitString.Length < 4) Then
                Return ""
            End If
            Return splitString(3)
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return ""
        End Try
    End Function
    Public Function ParseIDNForModel(ByVal idnString As String) As String
        Try
            Dim splitString As String() = Split(idnString, ",")
            If (splitString.Length < 4) Then
                Return ""
            End If
            Return splitString(0)
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return ""
        End Try
    End Function
    Public Function ParseIDNForRevision(ByVal idnString As String) As String
        Try
            Dim splitString As String() = Split(idnString, ",")
            If (splitString.Length < 4) Then
                Return ""
            End If
            Return splitString(2)
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return ""
        End Try
    End Function
    Public Sub PopulateSystemInfo()
        Try
            Dim serialNo As String
            Dim idnString As String
            
            switchDriver.System.DirectIO.FlushRead()
            directIOWrapper("print(localnode.serialno)")
            serialNo = switchDriver.System.DirectIO.ReadString()
            currentTestFile.SwitchSerial = serialNo
            switchDriver.System.DirectIO.FlushRead()
            If Not testSystemInfo.GetSwitchBySerial(serialNo) Is Nothing Then
                currentSwitch = testSystemInfo.GetSwitchBySerial(serialNo)
                currentSwitch.Active = True
            Else
                currentSwitch = New Switch
                currentSwitch.SerialNumber = serialNo
                currentSwitch.Active = True
                currentSwitch.FirstTest = Now()
                directIOWrapper("print(localnode.model)")
                currentSwitch.ModelNumber = switchDriver.System.DirectIO.ReadString()
                switchDriver.System.DirectIO.FlushRead()
                directIOWrapper("print(localnode.revision)")
                currentSwitch.Revision = switchDriver.System.DirectIO.ReadString()
                switchDriver.System.DirectIO.FlushRead()
                testSystemInfo.AddSwitch(currentSwitch)
            End If
            switchDriver.System.DirectIO.FlushRead()
            directIOWrapper("print(node[2].serialno)")
            serialNo = switchDriver.System.DirectIO.ReadString()
            currentTestFile.SourceMeterSerial = serialNo
            switchDriver.System.DirectIO.FlushRead()
            If Not testSystemInfo.GetSourceBySerial(serialNo) Is Nothing Then
                currentSource = testSystemInfo.GetSourceBySerial(serialNo)
                currentSource.Active = True
            Else
                currentSource = New SourceMeter
                currentSource.SerialNumber = serialNo
                currentSource.Active = True
                currentSource.FirstTest = Now()
                directIOWrapper("print(node[2].model)")
                currentSource.ModelNumber = switchDriver.System.DirectIO.ReadString()
                switchDriver.System.DirectIO.FlushRead()
                directIOWrapper("print(node[2].revision)")
                currentSource.Revision = switchDriver.System.DirectIO.ReadString()
                switchDriver.System.DirectIO.FlushRead()
                testSystemInfo.AddSource(currentSource)
            End If
            switchDriver.System.DirectIO.FlushRead()
            directIOWrapper("print(slot[1].idn)")
            idnString = switchDriver.System.DirectIO.ReadString()
            currentTestFile.MatrixCardOneSerial = ParseIDNForSerial(idnString)
            switchDriver.System.DirectIO.FlushRead()
            If Not idnString.Contains("Empty Slot") Then
                If Not currentSwitch.GetCardBySerial(currentTestFile.MatrixCardOneSerial) Is Nothing Then
                    If currentCards Is Nothing Then
                        ReDim currentCards(0)
                        currentCards(0) = currentSwitch.GetCardBySerial(currentTestFile.MatrixCardOneSerial)
                        currentCards(0).Active = True
                        currentCards(0).Slot = 1
                    Else
                        Dim upper As Long = currentCards.GetUpperBound(0)
                        ReDim Preserve currentCards(upper + 1)
                        currentCards(upper + 1) = currentSwitch.GetCardBySerial(currentTestFile.MatrixCardOneSerial)
                        currentCards(upper + 1).Active = True
                        currentCards(upper + 1).Slot = 1
                    End If
                Else
                    Dim newCard As New Card
                    newCard.SerialNumber = currentTestFile.MatrixCardOneSerial
                    newCard.ModelNumber = ParseIDNForModel(idnString)
                    newCard.Revision = ParseIDNForRevision(idnString)
                    newCard.FirstTest = Now()
                    newCard.Slot = 1
                    If currentCards Is Nothing Then
                        ReDim currentCards(0)
                        currentCards(0) = newCard
                        currentCards(0).Active = True
                    Else
                        Dim upper As Long = currentCards.GetUpperBound(0)
                        ReDim Preserve currentCards(upper + 1)
                        currentCards(upper + 1) = newCard
                        currentCards(upper + 1).Active = True
                    End If
                End If
            End If
            directIOWrapper("print(slot[2].idn)")
            idnString = switchDriver.System.DirectIO.ReadString()
            currentTestFile.MatrixCardTwoSerial = ParseIDNForSerial(idnString)
            switchDriver.System.DirectIO.FlushRead()
            If Not idnString.Contains("Empty Slot") Then
                If Not currentSwitch.GetCardBySerial(currentTestFile.MatrixCardTwoSerial) Is Nothing Then
                    If currentCards Is Nothing Then
                        ReDim currentCards(0)
                        currentCards(0) = currentSwitch.GetCardBySerial(currentTestFile.MatrixCardTwoSerial)
                        currentCards(0).Active = True
                    Else
                        Dim upper As Long = currentCards.GetUpperBound(0)
                        ReDim Preserve currentCards(upper + 1)
                        currentCards(upper + 1) = currentSwitch.GetCardBySerial(currentTestFile.MatrixCardTwoSerial)
                        currentCards(upper + 1).Active = True
                    End If
                Else
                    Dim newCard As New Card
                    newCard.SerialNumber = currentTestFile.MatrixCardTwoSerial
                    newCard.ModelNumber = ParseIDNForModel(idnString)
                    newCard.Revision = ParseIDNForRevision(idnString)
                    newCard.FirstTest = Now()
                    newCard.Slot = 2
                    If currentCards Is Nothing Then
                        ReDim currentCards(0)
                        currentCards(0) = newCard
                        currentCards(0).Active = True
                    Else
                        Dim upper As Long = currentCards.GetUpperBound(0)
                        ReDim Preserve currentCards(upper + 1)
                        currentCards(upper + 1) = newCard
                        currentCards(upper + 1).Active = True
                    End If
                End If
            End If
            directIOWrapper("print(slot[3].idn)")
            idnString = switchDriver.System.DirectIO.ReadString()
            currentTestFile.MatrixCardThreeSerial = ParseIDNForSerial(idnString)
            switchDriver.System.DirectIO.FlushRead()
            If Not (idnString.Contains("Empty Slot")) Then

                If Not currentSwitch.GetCardBySerial(currentTestFile.MatrixCardThreeSerial) Is Nothing Then
                    If currentCards Is Nothing Then
                        ReDim currentCards(0)
                        currentCards(0) = currentSwitch.GetCardBySerial(currentTestFile.MatrixCardThreeSerial)
                        currentCards(0).Active = True
                    Else
                        Dim upper As Long = currentCards.GetUpperBound(0)
                        ReDim Preserve currentCards(upper + 1)
                        currentCards(upper + 1) = currentSwitch.GetCardBySerial(currentTestFile.MatrixCardThreeSerial)
                        currentCards(upper + 1).Active = True
                    End If
                Else
                    Dim newCard As New Card
                    newCard.SerialNumber = currentTestFile.MatrixCardThreeSerial
                    newCard.ModelNumber = ParseIDNForModel(idnString)
                    newCard.Revision = ParseIDNForRevision(idnString)
                    newCard.FirstTest = Now()
                    newCard.Slot = 3
                    If currentCards Is Nothing Then
                        ReDim currentCards(0)
                        currentCards(0) = newCard
                        currentCards(0).Active = True
                    Else
                        Dim upper As Long = currentCards.GetUpperBound(0)
                        ReDim Preserve currentCards(upper + 1)
                        currentCards(upper + 1) = newCard
                        currentCards(upper + 1).Active = True
                    End If
                End If
            End If
            directIOWrapper("print(slot[4].idn)")
            idnString = switchDriver.System.DirectIO.ReadString()
            currentTestFile.MatrixCardFourSerial = ParseIDNForSerial(idnString)
            switchDriver.System.DirectIO.FlushRead()
            If Not idnString.Contains("Empty Slot") Then
                If Not currentSwitch.GetCardBySerial(currentTestFile.MatrixCardFourSerial) Is Nothing Then
                    If currentCards Is Nothing Then
                        ReDim currentCards(0)
                        currentCards(0) = currentSwitch.GetCardBySerial(currentTestFile.MatrixCardFourSerial)
                        currentCards(0).Active = True
                    Else
                        Dim upper As Long = currentCards.GetUpperBound(0)
                        ReDim Preserve currentCards(upper + 1)
                        currentCards(upper + 1) = currentSwitch.GetCardBySerial(currentTestFile.MatrixCardFourSerial)
                        currentCards(upper + 1).Active = True
                    End If
                Else
                    Dim newCard As New Card
                    newCard.SerialNumber = currentTestFile.MatrixCardFourSerial
                    newCard.ModelNumber = ParseIDNForModel(idnString)
                    newCard.Revision = ParseIDNForRevision(idnString)
                    newCard.FirstTest = Now()
                    newCard.Slot = 4
                    If currentCards Is Nothing Then
                        ReDim currentCards(0)
                        currentCards(0) = newCard
                        currentCards(0).Active = True
                    Else
                        Dim upper As Long = currentCards.GetUpperBound(0)
                        ReDim Preserve currentCards(upper + 1)
                        currentCards(upper + 1) = newCard
                        currentCards(upper + 1).Active = True
                    End If
                End If
            End If
            directIOWrapper("print(slot[5].idn)")
            idnString = switchDriver.System.DirectIO.ReadString()
            currentTestFile.MatrixCardFiveSerial = ParseIDNForSerial(idnString)
            switchDriver.System.DirectIO.FlushRead()
            If Not idnString.Contains("Empty Slot") Then
                If Not currentSwitch.GetCardBySerial(currentTestFile.MatrixCardFiveSerial) Is Nothing Then
                    If currentCards Is Nothing Then
                        ReDim currentCards(0)
                        currentCards(0) = currentSwitch.GetCardBySerial(currentTestFile.MatrixCardFiveSerial)
                        currentCards(0).Active = True
                    Else
                        Dim upper As Long = currentCards.GetUpperBound(0)
                        ReDim Preserve currentCards(upper + 1)
                        currentCards(upper + 1) = currentSwitch.GetCardBySerial(currentTestFile.MatrixCardFiveSerial)
                        currentCards(upper + 1).Active = True
                    End If
                Else
                    Dim newCard As New Card
                    newCard.SerialNumber = currentTestFile.MatrixCardFiveSerial
                    newCard.ModelNumber = ParseIDNForModel(idnString)
                    newCard.Revision = ParseIDNForRevision(idnString)
                    newCard.FirstTest = Now()
                    newCard.Slot = 5
                    If currentCards Is Nothing Then
                        ReDim currentCards(0)
                        currentCards(0) = newCard
                        currentCards(0).Active = True
                    Else
                        Dim upper As Long = currentCards.GetUpperBound(0)
                        ReDim Preserve currentCards(upper + 1)
                        currentCards(upper + 1) = newCard
                        currentCards(upper + 1).Active = True
                    End If
                End If
            End If
            directIOWrapper("print(slot[6].idn)")
            idnString = switchDriver.System.DirectIO.ReadString()
            currentTestFile.MatrixCardSixSerial = ParseIDNForSerial(idnString)
            switchDriver.System.DirectIO.FlushRead()
            If Not idnString.Contains("Empty Slot") Then
                If Not currentSwitch.GetCardBySerial(currentTestFile.MatrixCardSixSerial) Is Nothing Then
                    If currentCards Is Nothing Then
                        ReDim currentCards(0)
                        currentCards(0) = currentSwitch.GetCardBySerial(currentTestFile.MatrixCardSixSerial)
                        currentCards(0).Active = True
                    Else
                        Dim upper As Long = currentCards.GetUpperBound(0)
                        ReDim Preserve currentCards(upper + 1)
                        currentCards(upper + 1) = currentSwitch.GetCardBySerial(currentTestFile.MatrixCardSixSerial)
                        currentCards(upper + 1).Active = True
                    End If
                Else
                    Dim newCard As New Card
                    newCard.SerialNumber = currentTestFile.MatrixCardSixSerial
                    newCard.ModelNumber = ParseIDNForModel(idnString)
                    newCard.Revision = ParseIDNForRevision(idnString)
                    newCard.FirstTest = Now()
                    newCard.Slot = 6
                    If currentCards Is Nothing Then
                        ReDim currentCards(0)
                        currentCards(0) = newCard
                        currentCards(0).Active = True
                    Else
                        Dim upper As Long = currentCards.GetUpperBound(0)
                        ReDim Preserve currentCards(upper + 1)
                        currentCards(upper + 1) = newCard
                        currentCards(upper + 1).Active = True
                    End If
                End If
            End If
            ' Set all cards not just found to inactive
            currentSwitch.Cards = currentCards
            Dim boolCardFound As Boolean
            For Each exCard In currentSwitch.Cards
                boolCardFound = False
                For Each inCard In currentCards
                    If inCard.SerialNumber = exCard.SerialNumber Then
                        boolCardFound = True
                    End If
                Next
                If Not boolCardFound Then
                    exCard.Active = False
                End If
            Next
            currentTestFile.SystemSwitch = currentSwitch
            currentTestFile.SystemSource = currentSource
            testSystemInfo.writeToFile()
        Catch comEx As COMException
            ComExceptionHandler(comEx)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Function checkIOStatus()
        If (switchDriver.Initialized()) Then
            boolIOEstablished = True
            Return True
        Else
            Dim options As String
            ' An option string must be explicitly declared or the driver throws a COMException.  This may be fixed by firmware upgrades
            options = "QueryInstStatus=true, RangeCheck=true, Cache=true, Simulate=false, RecordCoercions=false, InterchangeCheck=false"
            switchDriver.Initialize(config.Address, False, False, options)
            If (switchDriver.Initialized()) Then
                boolIOEstablished = True
                switchDriver.TspLink.Reset()
                Return True
            Else
                boolIOEstablished = False
                If Not testSystemInfo Is Nothing Then
                    testSystemInfo = Nothing
                End If
                boolSystemInfoLoaded = False
                Return False
            End If
        End If
    End Function
    Public Sub RunAuditCheck()
        Try

            switchDriver.Channel.OpenAll()
            directIOWrapper("node[2].display.clear()")
            directIOWrapper("node[2].display.settext('Running Self Check')")
            ' set both SMU channels to DC volts
            ' Note: The 2602A does not appear to understand the enum variables spelled out in the user manual.  Integers are used instead
            directIOWrapper("node[2].smub.source.func = 1")
            ' Set the bias for both channels based on the value in config
            directIOWrapper("node[2].smub.source.levelv = " & config.Bias)
            ' Range is hard-coded to 1.  Does this need to be a config setting in the future?
            directIOWrapper("node[2].smub.source.rangev = 1")
            ' disable autorange for both output channels
            directIOWrapper("node[2].smub.source.autorangei = 0")
            Dim i As Integer = 1
            Dim z As Integer = 1
            Dim aChannel As New AuditChannel
            For i = 1 To config.CardConfig
                For z = 1 To 16
                    currentTestFile.AuditCheck.AddChannel(aChannel.ChannelFactory(i, z))
                Next
            Next

            directIOWrapper("node[2].smub.source.output = 1")

            ' Set connection rule to "make before break"
            ' @TODO: This is the setting from the old software.  Should this be changed?
            directIOWrapper("node[1].channel.connectrule = 2")
            directIOWrapper("node[2].display.clear()")
            ' Configure the DMM
            directIOWrapper("node[2].smua.measure.filter.type = " & config.Filter - 1)
            directIOWrapper("node[2].smub.measure.filter.type = " & config.Filter - 1)
            directIOWrapper("node[2].smua.measure.filter.count = " & config.Samples)
            directIOWrapper("node[2].smub.measure.filter.count = " & config.Samples)
            directIOWrapper("node[2].smua.measure.filter.enable = 1")
            directIOWrapper("node[2].smub.measure.filter.enable = 1")
            directIOWrapper("node[2].smua.measure.nplc = " & config.NPLC)
            directIOWrapper("node[2].smub.measure.nplc = " & config.NPLC)
            ' Clear the non-volatile measurement buffers
            directIOWrapper("node[2].smub.nvbuffer1.clear()")
            directIOWrapper("node[2].smub.nvbuffer2.clear()")
            directIOWrapper("node[2].smub.source.rangei = .00001")

            For Each aChannel In currentTestFile.AuditCheck.AuditChannels
                'Take readings from first resistor
                Dim row As Integer = 3
                switchDriver.System.DirectIO.FlushRead()
                directIOWrapper("node[1].channel.exclusiveclose('" & aChannel.Card & 2 & strPad(aChannel.Column, 2) & "," & aChannel.Card & row & strPad(aChannel.Column, 2) & "," & aChannel.Card & "912," & aChannel.Card & "913')")
                Debug.Print("node[1].channel.exclusiveclose('" & aChannel.Card & 2 & strPad(aChannel.Column, 2) & "," & aChannel.Card & row & strPad(aChannel.Column, 2) & "," & aChannel.Card & "912," & aChannel.Card & "913')")
                Delay(config.SettlingTime)
                directIOWrapper("node[2].smub.measure.iv(node[2].smub.nvbuffer1, node[2].smub.nvbuffer2)")
                directIOWrapper("printbuffer(1, node[2].smub.nvbuffer1.n, node[2].smub.nvbuffer1)")
                Dim current As Double = CDbl(switchDriver.System.DirectIO.ReadString())
                switchDriver.System.DirectIO.FlushRead()
                directIOWrapper("printbuffer(1, node[2].smub.nvbuffer2.n, node[2].smub.nvbuffer2)")
                Dim volts As Double = CDbl(switchDriver.System.DirectIO.ReadString())
                switchDriver.System.DirectIO.FlushRead()
                Dim aReading As New AuditReading
                aChannel.AddReading(aReading.ReadingFactory(volts, current, config.Resistor1Resistance, row))


                'Take readings from second resistor
                row = 4
                directIOWrapper("node[1].channel.exclusiveclose('" & aChannel.Card & 2 & strPad(aChannel.Column, 2) & "," & aChannel.Card & row & strPad(aChannel.Column, 2) & "," & aChannel.Card & "912," & aChannel.Card & "914')")
                Debug.Print("node[1].channel.exclusiveclose('" & aChannel.Card & 2 & strPad(aChannel.Column, 2) & "," & aChannel.Card & row & strPad(aChannel.Column, 2) & "," & aChannel.Card & "912," & aChannel.Card & "914')")
                Delay(config.SettlingTime)
                directIOWrapper("node[2].smub.measure.iv(node[2].smub.nvbuffer1, node[2].smub.nvbuffer2)")
                directIOWrapper("printbuffer(1, node[2].smub.nvbuffer1.n, node[2].smub.nvbuffer1)")
                current = CDbl(switchDriver.System.DirectIO.ReadString())
                switchDriver.System.DirectIO.FlushRead()
                directIOWrapper("printbuffer(1, node[2].smub.nvbuffer2.n, node[2].smub.nvbuffer2)")
                volts = CDbl(switchDriver.System.DirectIO.ReadString())
                switchDriver.System.DirectIO.FlushRead()
                aChannel.AddReading(aReading.ReadingFactory(volts, current, config.Resistor2Resistance, row))

                'Take readings from third resistor
                row = 5
                directIOWrapper("node[1].channel.exclusiveclose('" & aChannel.Card & 2 & strPad(aChannel.Column, 2) & "," & aChannel.Card & row & strPad(aChannel.Column, 2) & "," & aChannel.Card & "912," & aChannel.Card & "915')")
                Debug.Print("node[1].channel.exclusiveclose('" & aChannel.Card & 2 & strPad(aChannel.Column, 2) & "," & aChannel.Card & row & strPad(aChannel.Column, 2) & "," & aChannel.Card & "912," & aChannel.Card & "915')")
                Delay(config.SettlingTime)
                directIOWrapper("node[2].smub.measure.iv(node[2].smub.nvbuffer1, node[2].smub.nvbuffer2)")
                directIOWrapper("printbuffer(1, node[2].smub.nvbuffer1.n, node[2].smub.nvbuffer1)")
                current = CDbl(switchDriver.System.DirectIO.ReadString())
                switchDriver.System.DirectIO.FlushRead()
                directIOWrapper("printbuffer(1, node[2].smub.nvbuffer2.n, node[2].smub.nvbuffer2)")
                volts = CDbl(switchDriver.System.DirectIO.ReadString())
                switchDriver.System.DirectIO.FlushRead()
                aChannel.AddReading(aReading.ReadingFactory(volts, current, config.Resistor3Resistance, row))
                ' Add switches to total
                testSystemInfo.addSwitchEvent(aChannel.Card, 3)
            Next
            directIOWrapper("node[2].smub.source.output = 0 node[2].smua.source.output = 0")
        Catch ex As COMException
            ComExceptionHandler(ex)
            Exit Sub
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Function initializeDriver() As Boolean
        Try
            switchDriver = Nothing
            switchDriver = New Ke37XX
            Dim options As String
            ' An option string must be explicitly declared or the driver throws a COMException.
            options = "QueryInstStatus=true, RangeCheck=false, Cache=false, Simulate=false, RecordCoercions=false, InterchangeCheck=false"
            switchDriver.Initialize(config.Address, False, True, options)
            If (switchDriver.Initialized()) Then
                switchDriver.TspLink.Reset()
                Return True
            Else
                Return False
            End If
        Catch comEx As COMException
            ComExceptionHandler(comEx)
            Return False
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return False
        End Try
    End Function
End Module
