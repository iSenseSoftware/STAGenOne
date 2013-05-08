Option Explicit On
' -----------------------------------------------------------------------------------------
' The Sensor class defines an object which is used to store identifying information and
' test readings for a single sensor being tested.  
' -----------------------------------------------------------------------------------------
Public Class Sensor
    Dim intColumn As Integer ' This variable holds the SwitchMatrix column for the sensor
    Dim intSlot As Integer 'The card slot for the card to which the sensor is attached
    Dim strBatch As String
    Dim strFixtureSlot As String ' The fixture and channel number of the sensor's dipping fixture
    Dim aryReadings(0) As Reading ' This array holds the sensor readings
    Dim strSensorID As String ' The sensor's unique identifier
    Public Property Batch As String
        Get
            Return strBatch
        End Get
        Set(ByVal strValue As String)
            strBatch = strValue
        End Set
    End Property
    Public Property FixtureSlot As String
        Get
            Return strFixtureSlot
        End Get
        Set(ByVal strValue As String)
            strFixtureSlot = strValue
        End Set
    End Property
    Public Property Readings As Reading()
        Get
            Return aryReadings
        End Get
        Set(ByVal aryValue As Reading())
            aryReadings = aryValue
        End Set
    End Property
    Public Property SensorID As String
        Get
            Return strSensorID
        End Get
        Set(ByVal strValue As String)
            strSensorID = strValue
        End Set
    End Property
    Public Property Slot As Integer
        Get
            Return intSlot
        End Get
        Set(ByVal intValue As Integer)
            intSlot = intValue
        End Set
    End Property
    Public Property Column As Integer
        Get
            Return intColumn
        End Get
        Set(ByVal intValue As Integer)
            intColumn = intValue
        End Set
    End Property
    ' Name: AddReading()
    ' Parameters:
    '           rdgReading: The Reading to be added to the Readings array
    ' Description: Add a new reading to the aryReadings array
    Public Sub AddReading(ByRef rdgReading As Reading)
        Try
            If aryReadings Is Nothing Then
                ReDim aryReadings(0)
                aryReadings(0) = rdgReading
            Else
                Dim upper As Long = aryReadings.GetUpperBound(0)
                ReDim Preserve aryReadings(upper + 1)
                aryReadings(upper + 1) = rdgReading
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Name: SensorFactory()
    ' Parameters:
    '           intSlot: The slot for the card to which the sensor is attached
    '           intColumn: The switch matrix column to which the sensor is connected
    '           strBatch: The batch # for the sensor
    '           strFixtureSlot: The fixture and channel number for the sensor
    ' Description: The sensorFactory returns an instance of this object with the inputted properties set.  
    ' This cannot be done in the constructor because we will be serializing this object to xml
    ' for the test file and the Serializer requires a parameterless constructor
    Public Shared Function SensorFactory(ByVal intSlot As Integer, ByVal intColumn As Integer, ByVal strBatch As String, ByVal strFixtureSlot As String) As Sensor
        Try
            Dim ssrReturnSensor As New Sensor
            ssrReturnSensor.Slot = intSlot
            ssrReturnSensor.Column = intColumn
            ssrReturnSensor.SensorID = strBatch & strFixtureSlot
            ssrReturnSensor.FixtureSlot = strFixtureSlot
            ssrReturnSensor.Batch = strBatch
            Return ssrReturnSensor
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return Nothing
        End Try
    End Function
    Public Sub New()
        ' Empty constructor
    End Sub
End Class
