' --------------------------------------------------------------------
' The AuditCheck class defines an object which contains the data gathered
' during the Audit Check step prior to running a test.  The class has a property
' indicating whether the AuditCheck passed as well as an array of AuditChannel objects which
' contain the AuditReadings collected for each column on each switch matrix card
' ---------------------------------------------------------------------
Option Explicit On
Public Class AuditCheck
    Dim boolPass As Boolean
    Dim aryAuditChannels(0) As AuditChannel
    ' ----------------------------------------------
    ' Getters and Setters
    ' ----------------------------------------------
    Public Property Pass As Boolean
        Get
            Return boolPass
        End Get
        Set(boolValue As Boolean)
            boolPass = boolValue
        End Set
    End Property
    Public Property AuditChannels As AuditChannel()
        Get
            Return aryAuditChannels
        End Get
        Set(aryValue As AuditChannel())
            aryAuditChannels = aryValue
        End Set
    End Property
    ' ----------------------------------------------------------------
    ' Utility Functions
    ' ----------------------------------------------------------------
    ' Name: AddChannel()
    ' Parameters:
    '           acChannel: The AuditChannel instance to be added to the AuditChannels array
    ' Description: Adds an AuditChannel object to the AuditChannels array, redimensioning as necessary
    Public Sub AddChannel(ByVal acChannel As AuditChannel)
        Try
            ' If the first element has not be assigned, assign the input to the first element
            If aryAuditChannels(0) Is Nothing Then
                ReDim aryAuditChannels(0)
                aryAuditChannels(0) = acChannel
            Else
                ' Otherwise, redimension the array to make room for the new object
                Dim lngUpper As Long = aryAuditChannels.GetUpperBound(0)
                ReDim Preserve aryAuditChannels(lngUpper + 1)
                aryAuditChannels(lngUpper + 1) = acChannel
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Name: Validate()
    ' Parameters: none
    ' Description: Checks the readings contained in the object instance and checks them against expected values to determine pass/fail
    '              and saets the value of the Pass property
    Public Sub Validate()
        Try
            Dim boolValidates As Boolean = True
            For Each acChannel In aryAuditChannels
                For Each ardReading In acChannel.AuditReadings
                    If ardReading.Open Then
                        If (Math.Abs(ardReading.Current) > cfgGlobal.AuditZero) Then
                            boolValidates = False
                        End If
                    Else
                        If (Math.Abs(((ardReading.Voltage / ardReading.NominalResistance) - ardReading.Current) / (ardReading.Voltage / ardReading.NominalResistance)) > cfgGlobal.AuditTolerance) Then
                            boolValidates = False
                        End If
                    End If
                    
                Next
            Next
            boolPass = boolValidates
        Catch ex As Exception
            GenericExceptionHandler(ex)
            boolPass = False
        End Try
    End Sub
    Public Sub New()
        ' In order for this object to be properly serialized by the XMLSerializer class, the constructor must be empty
    End Sub
End Class
