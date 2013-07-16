Option Explicit On
Imports System
Imports System.IO
Imports System.Xml.Serialization
Imports SimpleSTA.modShared
Imports System.Runtime.InteropServices

' ----------------------------------------------------------------------------------------
' The Configuration class defines an object to contain test configuration settings
' It includes all the properties stored in the configuration as well as methods to load
' properties from a config file and update the config file with new properties.
' 
' The globally-instantiated Configuration object config is used throughout all modules
' to access global settings.
' -------------------------------------------------------------------------------------
Public Class clsConfiguration
    ' Declare vars, set defaults
    Dim dblBias As Double = 0.65
    Dim dblRecordInterval As Double = 8
    Dim crCurrentRange As CurrentRange = CurrentRange.one_uA
    Dim ftFilterType As FilterType = FilterType.FILTER_REPEAT_AVG
    Dim lngSamples As Long = 6
    Dim lngNPLC As Long = 1
    Dim intSettlingTime As Integer = 20 ' Settling time in milliseconds
    Dim aryResistorNominalValues(0 To 3) As Double
    Dim dblAuditCheckZero As Double = 10 ^ -10 ' "zero" value used for open circuit checks
    Dim dblAuditCheckTolerance As Double = 0.1  ' The audit check tolerance expressed as a % error from nominal expected current.  Valid values are 0 - 1 inclusive
    Dim ccfCardConfig As CardConfiguration = CardConfiguration.TWO_CARD_THIRTY_TWO_SENSORS

    ' The Address and STAID properties are dependent on the PC/Hardware configuration and cannot be assigned default values
    Dim strAddress As String
    Dim strSTAID As String

    ' Set default save paths based on installation directory (strAppDir)
    Dim strDumpDirectory As String = strAppDir & Path.DirectorySeparatorChar & "RawTestData"
    'Dim strSystemFileDirectory As String = strAppDir & Path.DirectorySeparatorChar & "SystemInfo"
    Public Property AuditZero As Double
        Get
            Return dblAuditCheckZero
        End Get
        Set(dblValue As Double)
            dblAuditCheckZero = dblValue
        End Set
    End Property
    Public Property ResistorNominalValues As Double()
        Get
            Return aryResistorNominalValues
        End Get
        Set(aryValue As Double())
            aryResistorNominalValues = aryValue
        End Set
    End Property
    'Public Property SystemFileDirectory As String
    '    Get
    '        Return strSystemFileDirectory
    '    End Get
    '    Set(ByVal strValue As String)
    '        strSystemFileDirectory = strValue
    '    End Set
    'End Property
    Public Property AuditTolerance As Double
        Get
            Return dblAuditCheckTolerance
        End Get
        Set(ByVal dblValue As Double)
            dblAuditCheckTolerance = dblValue
        End Set
    End Property
    Public Property SettlingTime As Integer
        Get
            Return intSettlingTime
        End Get
        Set(ByVal intValue As Integer)
            intSettlingTime = intValue
        End Set
    End Property
    Public Property DumpDirectory As String
        Get
            Return strDumpDirectory
        End Get
        Set(ByVal strValue As String)
            strDumpDirectory = strValue
        End Set
    End Property
    Public Property Bias() As Double
        Get
            Return dblBias
        End Get
        Set(ByVal dblValue As Double)
            dblBias = dblValue
        End Set
    End Property
    Public Property RecordInterval As Double
        Get
            Return dblRecordInterval
        End Get
        Set(ByVal dblValue As Double)
            dblRecordInterval = dblValue
        End Set
    End Property
    Public Property Range As CurrentRange
        Get
            Return crCurrentRange
        End Get
        Set(ByVal crValue As CurrentRange)
            crCurrentRange = crValue
        End Set
    End Property
    Public Property Filter As FilterType
        Get
            Return ftFilterType
        End Get
        Set(ByVal ftValue As FilterType)
            ftFilterType = ftValue
        End Set
    End Property
    Public Property Samples As Long
        Get
            Return lngSamples
        End Get
        Set(ByVal lngValue As Long)
            lngSamples = lngValue
        End Set
    End Property
    Public Property NPLC As Long
        Get
            Return lngNPLC
        End Get
        Set(ByVal lngValue As Long)
            lngNPLC = lngValue
        End Set
    End Property
    Public Property Address As String
        Get
            Return strAddress
        End Get
        Set(ByVal strValue As String)
            strAddress = strValue
        End Set
    End Property
    Public Property STAID As String
        Get
            Return strSTAID
        End Get
        Set(ByVal strValue As String)
            strSTAID = strValue
        End Set
    End Property
    Public Property CardConfig As CardConfiguration
        Get
            Return ccfCardConfig
        End Get
        Set(ByVal ccfValue As CardConfiguration)
            ccfCardConfig = ccfValue
        End Set
    End Property
    ' Name: WriteToFile()
    ' Parameters:
    '           strFilePath: The full path to the location to which the file is saved
    ' Return: Boolean: Indicates success or failure
    ' Description: Attempts to serialize the current instance to an xml file at the path given as a parameter
    Public Function WriteToFile(ByVal strFilePath As String) As Boolean
        Try
            Dim xsSerializer As New XmlSerializer(Me.GetType)
            Dim swWriter As New StreamWriter(strFilePath)
            xsSerializer.Serialize(swWriter, cfgGlobal)
            swWriter.Close()
            Return True
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return False
        End Try
    End Function
    ' Name: Validate()
    ' Returns: Boolean: Indicates success or failure
    ' Description: Validates the current values for properties of this instance and checks them
    '           against validation rules for format and content
    Public Function Validate() As Boolean
        Try
            Dim verifies As Boolean = True
            If (Me.Bias = Nothing) Then
                verifies = False
            End If
            If (Me.RecordInterval = Nothing) Then
                verifies = False
            End If
            If (Me.Range = Nothing) Then
                verifies = False
            End If
            If (Me.Filter = Nothing) Then
                verifies = False
            End If
            If (Me.Samples = Nothing) Then
                verifies = False
            End If
            If (Me.NPLC = Nothing) Then
                verifies = False
            End If
            If (Me.Address = Nothing Or Me.Address = "") Then
                verifies = False
            End If
            If (Me.STAID = Nothing Or Me.STAID = "") Then
                verifies = False
            End If
            If (Me.CardConfig = Nothing) Then
                verifies = False
            End If
            If (Me.ResistorNominalValues(0) <= 0 Or Me.ResistorNominalValues(1) <= 0 Or Me.ResistorNominalValues(2) <= 0 Or Me.ResistorNominalValues(3) <= 0) Then
                verifies = False
            End If
            Return verifies
        Catch comEx As COMException
            ComExceptionHandler(comEx)
            Return False
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return False
        End Try
    End Function
    Public Sub New()
        ' In order for this object to be properly serialized by the XMLSerializer class, the constructor must be empty
    End Sub
End Class
