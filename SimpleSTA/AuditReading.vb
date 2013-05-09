' ---------------------------------------------------
' The AuditReading class defines an object encapsulating a single reading
' collected during the Audit Check portion of the test workflow.
' ----------------------------------------------------
Option Explicit On
Public Class AuditReading
    Dim intRow As Integer
    Dim dblNominalResistance As Double
    Dim dblCurrent As Double
    Dim dblVoltage As Double
    Dim boolOpenCheck As Boolean
    ' ---------------------------------------------------
    ' Getters and Setters
    ' ---------------------------------------------------
    Public Property Open As Boolean
        Get
            Return boolOpenCheck
        End Get
        Set(boolValue As Boolean)
            boolOpenCheck = boolValue
        End Set
    End Property
    Public Property Row As Integer
        Get
            Return intRow
        End Get
        Set(intValue As Integer)
            intRow = intValue
        End Set
    End Property
    Public Property NominalResistance As Double
        Get
            Return dblNominalResistance
        End Get
        Set(dblValue As Double)
            dblNominalResistance = dblValue
        End Set
    End Property
    Public Property Current As Double
        Get
            Return dblCurrent
        End Get
        Set(dblValue As Double)
            dblCurrent = dblValue
        End Set
    End Property
    Public Property Voltage As Double
        Get
            Return dblVoltage
        End Get
        Set(dblValue As Double)
            dblVoltage = dblValue
        End Set
    End Property
    ' ----------------------------------------------------------------
    ' Utility Functions
    ' ----------------------------------------------------------------
    ' Name: ReadingFactory()
    ' Parameters:
    '           dblVoltage: The value for the Voltage property of the AuditReading object returned
    '           dblCurrent: The value for the Current property of the AuditReading object returned
    '           dblNominalResistance: The value, in ohms, for the NominalResistance property of the AuditReading object returned
    '           intRow: The switch matrix row on which the reading was taken
    ' Returns: AuditReading object
    ' Description: Because this object requires an empty constructor to serialize to XML, we define a factory function to return
    '           an instance of AuditReading with the input values set for the appropriate properties
    Public Shared Function ReadingFactory(ByVal dblVoltage As Double, ByVal dblCurrent As Double, ByVal dblNominalResistance As Double, ByVal intRow As Integer, Optional boolopen As Boolean = False) As AuditReading
        Try
            Dim ardOutReading As New AuditReading
            ardOutReading.Voltage = dblVoltage
            ardOutReading.Current = dblCurrent
            ardOutReading.NominalResistance = dblNominalResistance
            ardOutReading.Row = intRow
            ardOutReading.Open = boolopen
            Return ardOutReading
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return Nothing
        End Try
    End Function
    Public Sub New()

    End Sub
End Class
