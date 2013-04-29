Imports System.IO
Imports System.Xml.Serialization
Imports Keithley.Ke37XX.Interop
Imports System.Runtime.InteropServices
Imports Ivi.Driver.Interop

' The SharedModule is, as the name suggests, a collection of shared utlity functions, enumerated variables
' and global variables for use in all objects and forms
Public Module SharedModule
    '------------------------------
    ' Define constants and global variables
    '------------------------------
    Public Const configFileName As String = "Config.xml"
    Public Const systemInfoFileName As String = "SystemInfo.xml"

    Public appDir As String = My.Application.Info.DirectoryPath ' The path to the application install directory
    Public config As New Configuration ' Global instance of the Configuration object that stores config information for the current session
    Public testSystemInfo As New TestSystem ' Global instance of the TestSystem object which tracks system ID info (serials, models) and switch counts per card
    Public currentSwitch As Switch ' Global instance of the Switch object which represents the System Switch in the measurement hardware
    Public currentSource As SourceMeter ' Global instance of the SourceMeter object which represents the Source Meter in the measurement hardware
    Public currentTestFile As New TestFile ' Global instance of the TestFile object stores output data and writes itself to file
    Public currentCards() As Card ' Global array instance of Card objects which represents the Cards in the attached switch
    ' Keithley IVI-COM Driver for communicating with 3700 series system switches.  
    ' This instance is referenced by all functions which communicate with the measurement hardware
    Public switchDriver As New Ke37XX

    ' These boolean values are used to maintain program state info
    Public boolSystemInfoLoaded As Boolean = False ' Has the system info file been loaded successfully?
    Public boolConfigLoaded As Boolean = False ' Has a valid configuration been loaded?
    Public boolIOEstablished As Boolean = False ' Has the switchDriver been successfully initialized
    Public boolTestFileLoaded As Boolean = False ' Has the currentTestFile object been populated?
    Public boolSeriesLoaded As Boolean = False ' Are there any series present in the Test Chart?

    ' The admin password to unlock the configuration settings is hardcoded.  In the future
    ' it may be desireable to incorporate database-driven user authentication / authorization for granular permissions
    Public strAdminPassword As String = "C0balt22"

    ' @ NOTE: Because of a quirk of VB, a value of 0 for an enumerated variable is equivalent to Nothing, making validation difficult
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
    ' Deserialize Configuration object from xml file
    ' Returns nothing on failure
    Public Function loadConfiguration(ByVal strFilePath As String) As Configuration
        Try
            Dim reader As New StreamReader(strFilePath)
            Dim aConfig As New Configuration
            Dim serializer As New XmlSerializer(aConfig.GetType)
            aConfig = serializer.Deserialize(reader)
            reader.Close()
            Return aConfig
        Catch parseException As InvalidOperationException
            MsgBox("Invalid configuration file.  Delete " & strFilePath & " and reload.")
            Return Nothing
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return Nothing
        End Try
    End Function
    Public Sub initializeConfiguration()
        Try
            Dim serializer As New XmlSerializer(config.GetType)
            Dim writer As New StreamWriter(appDir & "\" & configFileName)
            serializer.Serialize(writer, config)
            writer.Close()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' This function attempts to load a TestSystem object from file and update it with current system info
    ' If no file is found, the user will be alerted and a new file generated
    Public Function loadSystemInfo(ByRef strFilePath As String) As TestSystem
        Try
            If (boolConfigLoaded And Not config Is Nothing) Then
                If (boolIOEstablished And switchDriver.Initialized()) Then
                    Dim theSystem As New TestSystem
                    If (System.IO.File.Exists(config.SystemFileDirectory & "\" & systemInfoFileName)) Then
                        Dim serializer As New XmlSerializer(theSystem.GetType)
                        Dim reader As New StreamReader(config.SystemFileDirectory & "\" & systemInfoFileName)
                        testSystemInfo = serializer.Deserialize(reader)
                        reader.Close()
                    Else
                        MsgBox("System info file not found.  Generating new file from defaults.")
                    End If
                    If updateTestSystemInfo(theSystem) Then
                        If theSystem.writeToFile() Then
                            Return theSystem
                        Else
                            MsgBox("Could not write system info to file.  Make sure the System Info directory is accessible.")
                            Return Nothing
                        End If
                    Else
                        MsgBox("Could not update test system info with current system information")
                        Return Nothing
                    End If
                Else
                    MsgBox("System I/O has not been established")
                    Return Nothing
                End If
            Else
                MsgBox("Configuration info has not been loaded")
                Return Nothing
            End If
        Catch parseException As InvalidOperationException
            MsgBox("Invalid System Information File.  Delete " & config.SystemFileDirectory & "\" & systemInfoFileName)
            Return Nothing
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return Nothing
        End Try
    End Function
    Public Function updateTestSystemInfo(ByRef theInfo As TestSystem) As Boolean
        '
    End Function
    Public Function initializeSystemInfo() As Boolean
        Try
            Dim serializer As New XmlSerializer(testSystemInfo.GetType)
            System.IO.Directory.CreateDirectory(config.SystemFileDirectory)
            Dim writer As New StreamWriter(config.SystemFileDirectory & "\SystemInfo.xml")
            serializer.Serialize(writer, testSystemInfo)
            writer.Close()
            Return True
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return False
        End Try
    End Function
    Public Function verifyConfiguration(ByRef theConfig As Configuration) As Boolean
        Try
            If theConfig Is Nothing Then
                Return False
            End If

            Dim verifies As Boolean = True
            If (theConfig.Bias = Nothing) Then
                verifies = False
            End If
            If (theConfig.RecordInterval = Nothing) Then
                verifies = False
            End If
            If (theConfig.Range = Nothing) Then
                verifies = False
            End If
            If (theConfig.Filter = Nothing) Then
                verifies = False
            End If
            If (theConfig.Samples = Nothing) Then
                verifies = False
            End If
            If (theConfig.NPLC = Nothing) Then
                verifies = False
            End If
            If (theConfig.Address = Nothing Or theConfig.Address = "") Then
                verifies = False
            End If
            If (theConfig.STAID = Nothing Or theConfig.STAID = "") Then
                verifies = False
            End If
            If (theConfig.CardConfig = Nothing) Then
                verifies = False
            End If
            If (theConfig.Resistor1Resistance <= 0 Or theConfig.Resistor2Resistance <= 0 Or theConfig.Resistor3Resistance <= 0) Then
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
    Public Function verifySystemInfo(ByRef theInfo As TestSystem) As Boolean
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
    ' Generates a generic error message for the given exception
    Public Sub GenericExceptionHandler(ByVal theException As Exception)
        MsgBox(theException.GetType.ToString() & Environment.NewLine & theException.Message & Environment.NewLine & theException.ToString)
    End Sub
    ' COM Exceptions are thrown by COM-based drivers, in this case the Ke37xx driver.
    ' This function queries the instrument for details about the error and generates and error message
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
    ' The ParseIDNxxx functions parse an input string returned from the Keithley instrument command slot[x].idn
    ' to extract the indicated value
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
    Public Function PopulateSystemInfo() As Boolean
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
            Return True
        Catch comEx As COMException
            ComExceptionHandler(comEx)
            testSystemInfo = Nothing
            Return False
        Catch ex As Exception
            GenericExceptionHandler(ex)
            testSystemInfo = Nothing
            Return False
        End Try
    End Function
    Public Function RunAuditCheck() As Boolean
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
            Return True
        Catch ex As COMException
            ComExceptionHandler(ex)
            Return False
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return False
        End Try
    End Function
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
                frmMain.chkIOStatus.Checked = True
                Return True
            Else
                frmMain.chkIOStatus.Checked = False
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
    ' This function attempts to either load or refresh the configuration from file.
    Public Function loadOrRefreshConfiguration() As Boolean
        Try
            If Not File.Exists(appDir & Path.DirectorySeparatorChar & configFileName) Then
                frmMain.chkConfigStatus.Checked = False
                Return False
            Else
                ' Create one
            End If
            Dim reader As New StreamReader(appDir & Path.DirectorySeparatorChar & configFileName)
            Dim serializer As New XmlSerializer(config.GetType)
            config = serializer.Deserialize(reader)
            reader.Close()
            If (verifyConfiguration(config)) Then
                If (config.WriteToFile(appDir & Path.DirectorySeparatorChar & configFileName)) Then
                    frmMain.chkConfigStatus.Checked = True
                    Return True
                Else
                    frmMain.chkConfigStatus.Checked = False
                    Return False
                End If
            Else
                frmMain.chkConfigStatus.Checked = False
                Return False
            End If
        Catch parseException As InvalidOperationException
            MsgBox("Invalid configuration file.  Delete " & appDir & Path.DirectorySeparatorChar & configFileName & " and reload.")
            frmMain.chkConfigStatus.Checked = False
            Return False
        Catch ex As Exception
            GenericExceptionHandler(ex)
            frmMain.chkConfigStatus.Checked = False
            Return False
        End Try
    End Function
    Public Function establishIO() As Boolean
        Try
            If (frmMain.chkConfigStatus.Checked) Then
                If (switchDriver.Initialized) Then
                    frmMain.chkIOStatus.Checked = True
                    Return True
                Else
                    Dim options As String
                    ' An option string must be explicitly declared or the driver throws a COMException.
                    options = "QueryInstStatus=true, RangeCheck=false, Cache=false, Simulate=false, RecordCoercions=false, InterchangeCheck=false"
                    switchDriver.Initialize(config.Address, False, True, options)
                    If (switchDriver.Initialized) Then
                        switchDriver.TspLink.Reset()
                        frmMain.chkIOStatus.Checked = True
                        Return True
                    Else
                        frmMain.chkIOStatus.Checked = False
                        Return False
                    End If
                End If  
            Else
                frmMain.chkIOStatus.Checked = False
                Return False
            End If
        Catch comEx As COMException
            ComExceptionHandler(comEx)
            frmMain.chkIOStatus.Checked = False
            Return False
        Catch ex As Exception
            GenericExceptionHandler(ex)
            frmMain.chkIOStatus.Checked = False
            Return False
        End Try
    End Function
    Public Function loadAndUpdateSystemInfo() As Boolean
        Dim boolOut As Boolean = False
        Dim reader As StreamReader
        Try
            If (frmMain.chkConfigStatus.Checked) Then
                If (switchDriver.Initialized) Then
                    If (File.Exists(config.SystemFileDirectory & "/" & systemInfoFileName)) Then
                        reader = New StreamReader(config.SystemFileDirectory & "/" & systemInfoFileName)
                        Dim serializer As New XmlSerializer(testSystemInfo.GetType)
                        testSystemInfo = serializer.Deserialize(reader)
                        reader.Close()
                        If (verifySystemInfo(testSystemInfo)) Then
                            If (PopulateSystemInfo()) Then
                                If (testSystemInfo.writeToFile()) Then
                                    boolOut = True
                                Else
                                    ' do nothing
                                End If
                            Else
                                'do nothing
                            End If
                        Else
                            ' do nothing
                        End If
                    Else
                        ' Generate a new one
                        If initializeSystemInfo() Then
                            If (verifySystemInfo(testSystemInfo)) Then
                                If (PopulateSystemInfo()) Then
                                    If (testSystemInfo.writeToFile()) Then
                                        boolOut = True
                                    Else
                                        ' do nothing
                                    End If
                                Else
                                    ' do nothing
                                End If
                            Else
                                ' do nothing
                            End If
                        Else
                            ' do nothing
                        End If
                    End If
                Else
                    ' do nothing
                End If
            Else
                ' do nothing
            End If
            Return boolOut
        Catch comEx As COMException
            ComExceptionHandler(comEx)
            frmMain.chkIOStatus.Checked = False
            Return False
        Catch ex As Exception
            GenericExceptionHandler(ex)
            frmMain.chkSysInfoStatus.Checked = False
            Return False
        Finally
            frmMain.chkSysInfoStatus.Checked = boolOut
        End Try
    End Function
End Module
