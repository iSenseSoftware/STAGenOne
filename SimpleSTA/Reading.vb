Public Class Reading
    Dim dblPotential As Double ' The potential at the time of the measurement
    Dim dblCurrent As Double    ' The current at the time of the measurement
    Dim strTime As DateTime ' The timestamp for the time of the measurement
    Public Property Potential As Double
        Get
            Return dblPotential
        End Get
        Set(ByVal value As Double)
            dblPotential = value
        End Set
    End Property
    Public Property Current As Double
        Get
            Return dblCurrent
        End Get
        Set(ByVal value As Double)
            dblCurrent = value
        End Set
    End Property
    Public Property Time As DateTime
        Get
            Return strTime
        End Get
        Set(ByVal value As DateTime)
            strTime = value
        End Set
    End Property
    ' The readingFactory returns an instance of this object with the inputted properties set.  
    ' This cannot be done in the constructor because we will be serializing this object to xml
    ' for the test file and the Serializer requires a parameterless constructor
    Public Shared Function readingFactory(ByVal time As DateTime, ByVal current As Double, ByVal potential As Double) As Reading
        Try
            Dim returnReading As New Reading
            returnReading.Potential = potential
            returnReading.Current = current
            returnReading.Time = time
            Return returnReading
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return Nothing
        End Try
    End Function
End Class
