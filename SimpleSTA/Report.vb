Imports System
Imports System.IO
Imports System.Xml.Serialization
Public Class Report
    Dim tfGlucose As New TestFile
    Dim tfAPAP As New TestFile
    Dim arySensors() As ReportSensor

    Public Property GlucoseFile As TestFile
        Get
            Return tfGlucose
        End Get
        Set(ByVal value As TestFile)
            tfGlucose = value
        End Set
    End Property
    Public Property APAPFile As TestFile
        Get
            Return tfAPAP
        End Get
        Set(ByVal value As TestFile)
            tfAPAP = value
        End Set
    End Property
    Public Property Sensors As ReportSensor()
        Get
            Return arySensors
        End Get
        Set(ByVal value As ReportSensor())
            arySensors = value
        End Set
    End Property
    Public Sub AnalyzeTest()
        ' perform the test analysis and set object values
        For Each aSensor In arySensors
            aSensor.Analyze()
            Debug.Print(aSensor.GlucoseInjection1Current)
            Debug.Print(aSensor.GlucoseInjection2Current)
            Debug.Print(aSensor.GlucoseInjection3Current)
            Debug.Print(aSensor.GlucoseInjection4Current)
        Next

    End Sub
    Public Sub WriteToFile(ByVal strFilePath As String)
        ' write the test report to file
        Try
            Dim serializer As New XmlSerializer(Me.GetType)
            Dim writer As New StreamWriter(strFilePath)
            serializer.Serialize(writer, Me)
            writer.Close()
        Catch ex As Exception
            MsgBox("An exception occurred:" & Environment.NewLine & ex.Message & Environment.NewLine & ex.ToString)
            MsgBox("Unable to write report to file")
        End Try
    End Sub
    Public Function ValidateTestFiles() As Boolean
        ' Validate the test files given as input to the constructor
        Dim boolValidates As Boolean = True
        ' check that the files contain the correct number of injections
        If Not tfGlucose.Injections.Length = 4 Then
            boolValidates = False
        End If

        If Not tfAPAP.Injections.Length = 1 Then
            boolValidates = False
        End If
        Return boolValidates
    End Function
    Public Sub New()
        ' Note that the XMLSerializer requires a parameterless constructor.  The LoadTestFiles function must be called to deserialize the given test files
    End Sub

    Public Sub LoadTestFiles(ByVal strAPAPFile As String, ByVal strGlucoseFile As String)
        Try
            Dim serializer As New XmlSerializer(tfGlucose.GetType)
            Dim reader As New StreamReader(strAPAPFile)
            tfAPAP = serializer.Deserialize(reader)
            reader = New StreamReader(strGlucoseFile)
            tfGlucose = serializer.Deserialize(reader)
            Dim i As Long = 0
            i = 0
            For Each aSensor In tfGlucose.Sensors
                ' normalize reading times
                If (tfGlucose.Sensors(i).SensorID <> tfAPAP.Sensors(i).SensorID) Then
                    Throw New Exception("Sensor configurations do not match!")
                End If
                Dim z As Long = 1
                Do While (z < aSensor.Readings.Length)
                    'Debug.Print(z)
                    tfGlucose.Sensors(i).Readings(z).Time = tfGlucose.Sensors(0).Readings(z).Time
                    z += 1
                Loop
                z = 1
                Do While (z < tfAPAP.Sensors(i).Readings.Length)
                    tfAPAP.Sensors(i).Readings(z).Time = tfAPAP.Sensors(0).Readings(z).Time
                    z += 1
                Loop
                AddReportSensor()
                arySensors(i).GlucoseReadings = tfGlucose.Sensors(i).Readings
                arySensors(i).GlucoseInjection1Time = tfGlucose.Injections(0)
                arySensors(i).GlucoseInjection2Time = tfGlucose.Injections(1)
                arySensors(i).GlucoseInjection3Time = tfGlucose.Injections(2)
                arySensors(i).GlucoseInjection4Time = tfGlucose.Injections(3)
                arySensors(i).APAPInjectionTime = tfAPAP.Injections(0)
                arySensors(i).APAPReadings = tfAPAP.Sensors(i).Readings
                arySensors(i).SensorID = tfGlucose.Sensors(i).SensorID
                arySensors(i).GlucoseInterval = tfGlucose.Config.RecordInterval
                arySensors(i).APAPInterval = tfAPAP.Config.RecordInterval
                i += 1
            Next
            AnalyzeTest()
        Catch ex As Exception
            MsgBox("An exception of type" & ex.GetType().ToString & " occurred:" & Environment.NewLine & ex.Message & Environment.NewLine & ex.ToString)
            MsgBox("Unable to load test files")
        End Try
    End Sub

    Public Sub AddReportSensor()
        Dim newSensor As New ReportSensor
        If arySensors Is Nothing Then
            ReDim arySensors(0)
            arySensors(0) = newSensor
        Else
            Dim upper As Long = arySensors.GetUpperBound(0)
            ReDim Preserve arySensors(upper + 1)
            arySensors(upper + 1) = newSensor
        End If
    End Sub
End Class
