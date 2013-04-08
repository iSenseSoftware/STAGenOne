Public Class AuditReading
    Dim intRow As Integer
    Dim dblNominalResistance As Double
    Dim dblCurrent As Double
    Dim dblVoltage As Double
    Public Property Row As Integer
        Get
            Return intRow
        End Get
        Set(value As Integer)
            intRow = value
        End Set
    End Property
    Public Property NominalResistance As Double
        Get
            Return dblNominalResistance
        End Get
        Set(value As Double)
            dblNominalResistance = value
        End Set
    End Property
    Public Property Current As Double
        Get
            Return dblCurrent
        End Get
        Set(value As Double)
            dblCurrent = value
        End Set
    End Property
    Public Property Voltage As Double
        Get
            Return dblVoltage
        End Get
        Set(value As Double)
            dblVoltage = value
        End Set
    End Property
    Public Function ReadingFactory(ByVal Voltage As Double, ByVal Current As Double, ByVal NominalResistance As Double, ByVal Row As Integer) As AuditReading
        Try
            Dim outReading As New AuditReading
            outReading.Voltage = Voltage
            outReading.Current = Current
            outReading.NominalResistance = NominalResistance
            outReading.Row = Row
            Return outReading
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Function
    Public Sub New()

    End Sub
End Class
