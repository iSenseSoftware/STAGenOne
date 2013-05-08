' ----------------------------------------------------------------------------------
' The Card class defines an object representing a switch card module in the system switch
' component of the measurement hardware.  It contains identifying info (model, SN, etc) as well
' as hardware usage history.  This class is used both by the TestSystem object which represents
' the whole measurement hardware subsystem as well as each TestFile to represent the hardware
' configuration used for a particular test.
' -------------------------------------------------------------------------------------
Option Explicit On
Public Class Card
    Dim strSerial As String
    Dim strModel As String
    Dim strRevision As String
    Dim dtFirstTest As DateTime
    Dim dtLastTest As DateTime
    Dim lngTotalSwitches As Long
    Dim boolActive As Boolean
    Dim intSlot As Integer
    Public Property Slot As Integer
        Get
            Return intSlot
        End Get
        Set(intValue As Integer)
            intSlot = intValue
        End Set
    End Property
    Public Property Revision As String
        Get
            Return strRevision
        End Get
        Set(strValue As String)
            strRevision = strValue
        End Set
    End Property
    Public Property SerialNumber As String
        Get
            Return strSerial
        End Get
        Set(strValue As String)
            strSerial = strValue
        End Set
    End Property
    Public Property ModelNumber As String
        Get
            Return strModel
        End Get
        Set(strValue As String)
            strModel = strValue
        End Set
    End Property
    Public Property FirstTest As DateTime
        Get
            Return dtFirstTest
        End Get
        Set(strValue As DateTime)
            dtFirstTest = strValue
        End Set
    End Property
    Public Property LastTest As DateTime
        Get
            Return dtLastTest
        End Get
        Set(dtValue As DateTime)
            dtLastTest = dtValue
        End Set
    End Property
    Public Property TotalSwitches As Long
        Get
            Return lngTotalSwitches
        End Get
        Set(lngValue As Long)
            lngTotalSwitches = lngValue
        End Set
    End Property
    Public Property Active As Boolean
        Get
            Return boolActive
        End Get
        Set(boolValue As Boolean)
            boolActive = boolValue
        End Set
    End Property
    Public Sub New()
        ' Constructor must be kept empty to allow the XMLSerializer object to serialize the Card to xml.
    End Sub
End Class
