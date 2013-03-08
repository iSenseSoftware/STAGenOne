Imports System
Imports System.IO
Imports System.Xml.Serialization
Public Class TestFile

    ' The TestFile class defines the highest level object created during the course of a test.  It contains an array
    ' of all sensors tested as well as basic information about the test (date, operatory, device serials, etc)

    Dim cfgConfig As Configuration ' The configuration used for the test
    Dim databaseId As Long  ' This is used to store the primary key id of the database record for the test
    Dim arySensors() As Sensor ' A collection of Sensor objects which contain sensor info and test readings
    Dim strDumpFile As String
    Dim strOperator As String ' The operator's initials / name
    Dim aryInjections() As DateTime ' An array containing timestamps for each time the "Note Injection" button is pressed
    Dim strComments As String 'Any comments made by the operator during the test
    Dim strTestName As String ' The name given to the test at the time of performance
    Dim lngTestLength As Double 'The total length of the test in seconds
    Dim dtTestStart As DateTime 'Time stamp for the start of the test
    Dim strTestEnd As String 'Time stamp for the end of the test
    Dim strSourceMeterID As String 'The serial number for the source meter
    Dim strSwtichID As String 'The serial number for the switch
    Dim strCardOneID As String 'The serial number for the slot 1 matrix card
    Dim strCardTwoID As String 'The serial number for the slot 2 matrix card
    Dim strCardThreeID As String 'The serial number for the slot 3 matrix card
    Dim strCardFourID As String 'The serial number for the slot 4 matrix card
    Dim strCardFiveID As String 'The serial number for the slot 5 matrix card
    Dim strCardSixID As String 'The serial number for the slot 6 matrix card
    Dim acAuditCheck As AuditCheck
    Public Property AuditCheck As AuditCheck
        Get
            Return acAuditCheck
        End Get
        Set(value As AuditCheck)
            acAuditCheck = value
        End Set
    End Property
    Public ReadOnly Property ID As Long
        Get
            Return databaseId
        End Get
    End Property
    Public Property DumpFile As String
        Get
            Return strDumpFile
        End Get
        Set(ByVal value As String)
            strDumpFile = value
        End Set
    End Property
    Public Property Config As Configuration
        Get
            Return cfgConfig
        End Get
        Set(ByVal value As Configuration)
            cfgConfig = value
        End Set
    End Property
    Public Property Sensors As Sensor()
        Get
            Return arySensors
        End Get
        Set(ByVal value As Sensor())
            arySensors = value
        End Set
    End Property
    Public Property OperatorID As String
        Get
            Return strOperator
        End Get
        Set(ByVal value As String)
            strOperator = value
        End Set
    End Property
    Public Property Injections As DateTime()
        Get
            Return aryInjections
        End Get
        Set(ByVal value As DateTime())
            aryInjections = value
        End Set
    End Property
    Public Property Comments As String
        Get
            Return strComments
        End Get
        Set(ByVal value As String)
            strComments = value
        End Set
    End Property
    Public Property Name As String
        Get
            Return strTestName
        End Get
        Set(ByVal value As String)
            strTestName = value
        End Set
    End Property
    Public Property TestLength As Long
        Get
            Return lngTestLength
        End Get
        Set(ByVal value As Long)
            lngTestLength = value
        End Set
    End Property
    Public Property TestStart As DateTime
        Get
            Return dtTestStart
        End Get
        Set(ByVal value As DateTime)
            dtTestStart = value
        End Set
    End Property
    Public Property TestEnd As String
        Get
            Return strTestEnd
        End Get
        Set(ByVal value As String)
            strTestEnd = value
        End Set
    End Property
    Public Property SourceMeterSerial As String
        Get
            Return strSourceMeterID
        End Get
        Set(ByVal value As String)
            strSourceMeterID = value
        End Set
    End Property
    Public Property SwitchSerial As String
        Get
            Return strSwtichID
        End Get
        Set(ByVal value As String)
            strSwtichID = value
        End Set
    End Property
    Public Property MatrixCardOneSerial As String
        Get
            Return strCardOneID
        End Get
        Set(ByVal value As String)
            strCardOneID = value
        End Set
    End Property
    Public Property MatrixCardTwoSerial As String
        Get
            Return strCardTwoID
        End Get
        Set(ByVal value As String)
            strCardTwoID = value
        End Set
    End Property
    Public Property MatrixCardThreeSerial As String
        Get
            Return strCardThreeID
        End Get
        Set(ByVal value As String)
            strCardThreeID = value
        End Set
    End Property
    Public Property MatrixCardFourSerial As String
        Get
            Return strCardFourID
        End Get
        Set(ByVal value As String)
            strCardFourID = value
        End Set
    End Property
    Public Property MatrixCardFiveSerial As String
        Get
            Return strCardFiveID
        End Get
        Set(ByVal value As String)
            strCardFiveID = value
        End Set
    End Property
    Public Property MatrixCardSixSerial As String
        Get
            Return strCardSixID
        End Get
        Set(ByVal value As String)
            strCardSixID = value
        End Set
    End Property
    Public Shared Function testFileFactory(ByVal strFilePath As String) As TestFile
        ' The testFileFactory function is used to return an instance of the TestFile object
        ' deserialized from the file at the given location (strFilePath)
        Try
            Dim output As New TestFile
            Dim serializer As New XmlSerializer(output.GetType)
            Dim reader As New StreamReader(strFilePath)
            output = serializer.Deserialize(reader)
            reader.Close()
            Return output
        Catch ex As Exception
            MsgBox("An exception occurred:" & Environment.NewLine & ex.Message & Environment.NewLine & ex.ToString)
            MsgBox("Unable to load test data from file")
            Return Nothing
        End Try
    End Function

    Public Sub writeToFile()
        ' Serializes the instance to file, using the file path defined by strDumpFile
        Try
            Dim serializer As New XmlSerializer(Me.GetType)
            Dim writer As New StreamWriter(Me.DumpFile)
            serializer.Serialize(writer, Me)
            writer.Close()
        Catch ex As Exception
            MsgBox("An exception occurred:" & Environment.NewLine & ex.Message & Environment.NewLine & ex.ToString)
            MsgBox("Unable to dump test data to file")
        End Try
    End Sub
    Public Sub addSensor(ByVal newSensor As Sensor)
        If arySensors Is Nothing Then
            ReDim arySensors(0)
            arySensors(0) = newSensor
        Else
            Dim upper As Long = arySensors.GetUpperBound(0)
            ReDim Preserve arySensors(upper + 1)
            arySensors(upper + 1) = newSensor
        End If

    End Sub
    Public Sub addInjection(ByVal timestamp As DateTime)
        If aryInjections Is Nothing Then
            ReDim aryInjections(0)
            aryInjections(0) = timestamp
        Else
            Dim upper As Long = aryInjections.GetUpperBound(0)
            ReDim Preserve aryInjections(upper + 1)
            aryInjections(upper + 1) = timestamp
        End If
    End Sub
    Public Sub New()

    End Sub
End Class
