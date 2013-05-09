Option Explicit On
Imports System
Imports System.IO
Imports System.Xml.Serialization
' -------------------------------------------------------------------------------
' The TestFile class defines the highest level object created during the course of a test.  It contains an array
' of all sensors tested as well as basic information about the test (date, operatory, device serials, etc)
' ------------------------------------------------------------------------------------------------
Public Class TestFile
    Dim cfgConfig As Configuration ' The configuration used for the test
    Dim arySensors() As Sensor ' A collection of Sensor objects which contain sensor info and test readings
    Dim aryFullCircuitReadings() As Reading ' This array contains all readings made at the end of each measurement cycle across all sensor channels.
    Dim strDumpFile As String
    Dim strOperator As String ' The operator's initials / name
    Dim aryInjections() As DateTime ' An array containing timestamps for each time the "Note Injection" button is pressed
    Dim strComments As String 'Any comments made by the operator during the test
    Dim strTestName As String ' The name given to the test at the time of performance
    Dim lngTestLength As Double 'The total length of the test in seconds
    Dim dtTestStart As DateTime 'Time stamp for the start of the test
    Dim dtTestEnd As DateTime 'Time stamp for the end of the test
    Dim strSourceMeterID As String 'The serial number for the source meter
    Dim strSwitchID As String 'The serial number for the switch
    Dim swtSwitch As Switch
    Dim srcSource As SourceMeter
    Dim acAuditCheck As AuditCheck
    Public Property Switch As Switch
        Get
            Return swtSwitch
        End Get
        Set(ByVal swtValue As Switch)
            swtSwitch = swtValue
        End Set
    End Property
    Public Property Source As SourceMeter
        Get
            Return srcSource
        End Get
        Set(ByVal smValue As SourceMeter)
            srcSource = smValue
        End Set
    End Property
    Public Property AuditCheck As AuditCheck
        Get
            Return acAuditCheck
        End Get
        Set(ByVal acValue As AuditCheck)
            acAuditCheck = acValue
        End Set
    End Property
    Public Property DumpFile As String
        Get
            Return strDumpFile
        End Get
        Set(ByVal strValue As String)
            strDumpFile = strValue
        End Set
    End Property
    Public Property Config As Configuration
        Get
            Return cfgConfig
        End Get
        Set(ByVal cfgValue As Configuration)
            cfgConfig = cfgValue
        End Set
    End Property
    Public Property Sensors As Sensor()
        Get
            Return arySensors
        End Get
        Set(ByVal aryValue As Sensor())
            arySensors = aryValue
        End Set
    End Property
    Public Property OperatorID As String
        Get
            Return strOperator
        End Get
        Set(ByVal strValue As String)
            strOperator = strValue
        End Set
    End Property
    Public Property Injections As DateTime()
        Get
            Return aryInjections
        End Get
        Set(ByVal aryValue As DateTime())
            aryInjections = aryValue
        End Set
    End Property
    Public Property Comments As String
        Get
            Return strComments
        End Get
        Set(ByVal strValue As String)
            strComments = strValue
        End Set
    End Property
    Public Property Name As String
        Get
            Return strTestName
        End Get
        Set(ByVal strValue As String)
            strTestName = strValue
        End Set
    End Property
    Public Property TestLength As Long
        Get
            Return lngTestLength
        End Get
        Set(ByVal lngValue As Long)
            lngTestLength = lngValue
        End Set
    End Property
    Public Property TestStart As DateTime
        Get
            Return dtTestStart
        End Get
        Set(ByVal dtValue As DateTime)
            dtTestStart = dtValue
        End Set
    End Property
    Public Property TestEnd As DateTime
        Get
            Return dtTestEnd
        End Get
        Set(ByVal dtValue As DateTime)
            dtTestEnd = dtValue
        End Set
    End Property
    Public Property SourceMeterSerial As String
        Get
            Return strSourceMeterID
        End Get
        Set(ByVal strValue As String)
            strSourceMeterID = strValue
        End Set
    End Property
    Public Property SwitchSerial As String
        Get
            Return strSwitchID
        End Get
        Set(ByVal strValue As String)
            strSwitchID = strValue
        End Set
    End Property
    Public Property FullCircuitReadings As Reading()
        Get
            Return aryFullCircuitReadings
        End Get
        Set(ByVal rdgValue As Reading())
            aryFullCircuitReadings = rdgValue
        End Set
    End Property
    ' Name: TestFileFactory()
    ' Parameters:
    '           strFilePath: The path to the file from which the TestFile object is to be loaded
    ' Returns: TestFile: A test file object deserialized from the input file
    ' Description: Attempts to deserialized a TestFile object stored in the input file.  Returns nothing on failure
    Public Shared Function TestFileFactory(ByVal strFilePath As String) As TestFile
        Try
            Dim output As New TestFile
            Dim serializer As New XmlSerializer(output.GetType)
            Dim reader As New StreamReader(strFilePath)
            output = serializer.Deserialize(reader)
            reader.Close()
            Return output
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return Nothing
        End Try
    End Function
    ' Name: WriteToFile()
    ' Return: Boolean: Indicates success or failure
    ' Description: Attempts to serialize the current instance to an xml file at the path given as a parameter
    Public Sub WriteToFile()
        Try
            Dim serializer As New XmlSerializer(Me.GetType)
            Dim writer As New StreamWriter(Me.DumpFile)
            serializer.Serialize(writer, Me)
            writer.Close()
        Catch ex As Exception
            ' Rethrow the exception to the calling function
            Throw
        End Try
    End Sub
    ' Name: AddSensor()
    ' Parameters:
    '           ssrNewSensor: The Sensor object to be added to this instances Sensors array
    ' Description: Adds the input Sensor to the Sensors array, redimensioning as necessary
    Public Sub AddSensor(ByVal newSensor As Sensor)
        Try
            If arySensors Is Nothing Then
                ReDim arySensors(0)
                arySensors(0) = newSensor
            Else
                Dim upper As Long = arySensors.GetUpperBound(0)
                ReDim Preserve arySensors(upper + 1)
                arySensors(upper + 1) = newSensor
            End If
        Catch ex As Exception
            ' Rethrow the exception to the caller
            Throw
        End Try
    End Sub
    ' Name: AddInjection()
    ' Parameters:
    '           dtTimestamp: The timestamp to be appended to the Injections array
    ' Description: Adds the timestamp given as input to the TestFile's Injections array, redimensioning as necessary
    Public Sub AddInjection(ByVal dtTimestamp As DateTime)
        Try
            If aryInjections Is Nothing Then
                ReDim aryInjections(0)
                aryInjections(0) = dtTimestamp
            Else
                Dim lngUpper As Long = aryInjections.GetUpperBound(0)
                ReDim Preserve aryInjections(lngUpper + 1)
                aryInjections(lngUpper + 1) = dtTimestamp
            End If
        Catch ex As Exception
            ' Rethrow exception to the caller
            Throw
        End Try
    End Sub
    ' Name: AddFullCircuitReading()
    ' Parameters:
    '           dtTime: The timestamp for the reading to be added
    '           dblCurrent: The measured current for the reading
    '           dblPotential: The measured potential for the reading
    Public Sub addFullCircuitReading(ByVal dtTime As DateTime, ByVal dblCurrent As Double, ByVal dblPotential As Double)
        Try
            Dim rdgReading As Reading
            rdgReading = Reading.ReadingFactory(dtTime, dblCurrent, dblPotential)
            If aryFullCircuitReadings Is Nothing Then
                ReDim aryFullCircuitReadings(0)
                aryFullCircuitReadings(0) = rdgReading
            Else
                Dim lngUpper As Long = aryFullCircuitReadings.GetUpperBound(0)
                ReDim Preserve aryFullCircuitReadings(lngUpper + 1)
                aryFullCircuitReadings(lngUpper + 1) = rdgReading
            End If
        Catch ex As Exception
            ' Rethrow the exception to the caller
            Throw
        End Try
    End Sub
    Public Sub New()

    End Sub
End Class
