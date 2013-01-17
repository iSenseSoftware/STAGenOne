Public Class Sensor
    Dim intColumn As Integer ' This variable holds the SwitchMatrix column for the sensor
    Dim intSlot As Integer 'The card slot for the card to which the sensor is attached
    Dim strBatch As String
    Dim strFixtureSlot As String ' The fixture and channel number of the sensor's dipping fixture
    Dim aryReadings(0) As Reading ' This array holds the sensor readings
    Dim strSensorID As String ' The sensor's unique identifier
    Dim strCurrentSourceChannel As String ' The current source meter channel connected to the sensor
    Dim databaseId As Long
    Public ReadOnly Property ID As Long
        Get
            Return databaseId
        End Get
    End Property
    Public Property Batch As String
        Get
            Return strBatch
        End Get
        Set(ByVal value As String)
            strBatch = value
        End Set
    End Property
    Public Property FixtureSlot As String
        Get
            Return strFixtureSlot
        End Get
        Set(ByVal value As String)
            strFixtureSlot = value
        End Set
    End Property
    Public Property Readings As Reading()
        Get
            Return aryReadings
        End Get
        Set(ByVal value As Reading())
            aryReadings = value
        End Set
    End Property
    Public Property SensorID As String
        Get
            Return strSensorID
        End Get
        Set(ByVal value As String)
            strSensorID = value
        End Set
    End Property
    Public Property CurrentSourceChannel As String
        Get
            Return strCurrentSourceChannel
        End Get
        Set(ByVal value As String)

        End Set
    End Property
    Public Property Slot As Integer
        Get
            Return intSlot
        End Get
        Set(ByVal value As Integer)
            intSlot = value
        End Set
    End Property
    Public Property Column As Integer
        Get
            Return intColumn
        End Get
        Set(ByVal value As Integer)
            intColumn = value
        End Set
    End Property
    ' Add a new reading to the aryReadings array
    Public Sub addReading(ByVal time As DateTime, ByVal current As Double, ByVal potential As Double)
        Dim aReading As Reading
        aReading = Reading.readingFactory(time, current, potential)
        If aryReadings Is Nothing Then
            ReDim aryReadings(0)
            aryReadings(0) = aReading
        Else
            Dim upper As Long = aryReadings.GetUpperBound(0)
            ReDim Preserve aryReadings(upper + 1)
            aryReadings(upper + 1) = aReading
        End If

    End Sub
    'Public Sub WriteToDatabase()
    '    Try
    '        Dim strQuery As String
    '        strQuery = "INSERT INTO Sensors (batch, fixture_slot) VALUES (" & strBatch & ", " & strFixtureSlot & ")"
    '        If (dataBuddy.Execute(strQuery)) Then
    '            ''
    '            databaseId = dataBuddy.LastID
    '        Else
    '            Throw New Exception("Unable to insert sensor data into database record")
    '        End If
    '    Catch ex As Exception
    '        MsgBox("An exception occurred:" & Environment.NewLine & ex.Message & Environment.NewLine & ex.ToString)
    '    End Try

    'End Sub
    ' The sensorFactory returns an instance of this object with the inputted properties set.  
    ' This cannot be done in the constructor because we will be serializing this object to xml
    ' for the test file and the Serializer requires a parameterless constructor
    Public Shared Function sensorFactory(ByVal slot As Integer, ByVal column As Integer, ByVal batch As String, ByVal fixtureSlot As String)
        Dim returnSensor As New Sensor
        returnSensor.Slot = slot
        returnSensor.Column = column
        returnSensor.SensorID = batch & fixtureSlot
        returnSensor.FixtureSlot = fixtureSlot
        returnSensor.Batch = batch
        Return returnSensor
    End Function
End Class
