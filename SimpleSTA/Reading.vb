Option Explicit On
' ---------------------------------------------------------------------------------------
' The Reading class defines an object used to store a single current|voltage|timestamp
' for a sensor reading taken during a test
' ---------------------------------------------------------------------------------------
Public Class Reading
    Dim dblVoltage As Double ' The potential at the time of the measurement
    Dim dblCurrent As Double    ' The current at the time of the measurement
    Dim strTime As DateTime ' The timestamp for the time of the measurement
    Public Property Voltage As Double
        Get
            Return dblVoltage
        End Get
        Set(ByVal dblValue As Double)
            dblVoltage = dblValue
        End Set
    End Property
    Public Property Current As Double
        Get
            Return dblCurrent
        End Get
        Set(ByVal dblValue As Double)
            dblCurrent = dblValue
        End Set
    End Property
    Public Property Time As DateTime
        Get
            Return strTime
        End Get
        Set(ByVal dtValue As DateTime)
            strTime = dtValue
        End Set
    End Property
    ' Name: ReadingFactory()
    ' Parameters:
    '           dtTime: The timestamp for the reading
    '           dblCurrent: The current for the reading
    '           dblPotential: The potential for the reading
    ' Description:
    ' The readingFactory returns an instance of this object with the inputted properties set.  
    ' This cannot be done in the constructor because we will be serializing this object to xml
    ' for the test file and the Serializer requires a parameterless constructor
    Public Shared Function ReadingFactory(ByVal dtTime As DateTime, ByVal dblCurrent As Double, ByVal dblPotential As Double) As Reading
        Try
            Dim rdgReturnReading As New Reading
            rdgReturnReading.Voltage = dblPotential
            rdgReturnReading.Current = dblCurrent
            rdgReturnReading.Time = dtTime
            Return rdgReturnReading
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return Nothing
        End Try
    End Function
End Class
