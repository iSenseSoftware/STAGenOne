Imports System
Imports System.IO
Imports System.Xml.Serialization
Imports SimpleSTA.SharedModule
Public Class Configuration
    '  The configuration class defines an object to contain test configuration settings
    '  It includes all the properties stored in the configuration as well as methods to load
    '  properties from a config file and update the config file with new properties

    ' Declare vars, set defaults
    Dim dblBias As Double = 0.65
    Dim dblRecordInterval As Double = 8
    Dim crCurrentRange As CurrentRange = CurrentRange.one_uA
    Dim ftFilterType As FilterType = FilterType.FILTER_REPEAT_AVG
    Dim lngSamples As Long = 6
    Dim lngNPLC As Long = 1
    Dim strAddress As String
    Dim strSTAID As String
    Dim ccfCardConfig As CardConfiguration = CardConfiguration.TWO_CARD_THIRTY_TWO_SENSORS
    Dim strDumpDirectory As String = appDir & Path.DirectorySeparatorChar & "RawTestData"
    Dim strSystemFileDirectory As String = appDir & Path.DirectorySeparatorChar & "SystemInfo"
    Dim intSettlingTime As Integer = 20
    Dim dblResistor1NominalResistance As Double = 10 ^ 8    '1 megaohm
    Dim dblResistor2NominalResistance As Double = 10 ^ 7    '10 megaohm
    Dim dblResistor3NominalResistance As Double = 10 ^ 6    '100 megaohm
    Dim dblAuditCheckTolerance As Double = 0.1  ' The audit check tolerance expressed as a % error from nominal expected current.  Valid values are 0 - 1 inclusive
    Public Property AuditTolerance As Double
        Get
            Return dblAuditCheckTolerance
        End Get
        Set(value As Double)
            dblAuditCheckTolerance = value
        End Set
    End Property
    Public Property Resistor1Resistance As Double
        Get
            Return dblResistor1NominalResistance
        End Get
        Set(value As Double)
            dblResistor1NominalResistance = value
        End Set
    End Property
    Public Property Resistor2Resistance As Double
        Get
            Return dblResistor2NominalResistance
        End Get
        Set(value As Double)
            dblResistor2NominalResistance = value
        End Set
    End Property
    Public Property Resistor3Resistance As Double
        Get
            Return dblResistor3NominalResistance
        End Get
        Set(value As Double)
            dblResistor3NominalResistance = value
        End Set
    End Property
    Public Property SettlingTime As Integer
        Get
            Return intSettlingTime
        End Get
        Set(value As Integer)
            intSettlingTime = value
        End Set
    End Property
    Public Property DumpDirectory As String
        Get
            Return strDumpDirectory
        End Get
        Set(value As String)
            strDumpDirectory = value
        End Set
    End Property
    Public Property Bias() As Double
        Get
            Return dblBias
        End Get
        Set(ByVal value As Double)
            dblBias = value
        End Set
    End Property
    Public Property RecordInterval As Double
        Get
            Return dblRecordInterval
        End Get
        Set(ByVal value As Double)
            dblRecordInterval = value
        End Set
    End Property
    Public Property Range As CurrentRange
        Get
            Return crCurrentRange
        End Get
        Set(ByVal value As CurrentRange)
            crCurrentRange = value
        End Set
    End Property
    Public Property Filter As FilterType
        Get
            Return ftFilterType
        End Get
        Set(ByVal value As FilterType)
            ftFilterType = value
        End Set
    End Property
    Public Property Samples As Long
        Get
            Return lngSamples
        End Get
        Set(ByVal value As Long)
            lngSamples = value
        End Set
    End Property
    Public Property NPLC As Long
        Get
            Return lngNPLC
        End Get
        Set(ByVal value As Long)
            lngNPLC = value
        End Set
    End Property
    Public Property Address As String
        Get
            Return strAddress
        End Get
        Set(ByVal value As String)
            strAddress = value
        End Set
    End Property
    Public Property STAID As String
        Get
            Return strSTAID
        End Get
        Set(ByVal value As String)
            strSTAID = value
        End Set
    End Property
    Public Property CardConfig As CardConfiguration
        Get
            Return ccfCardConfig
        End Get
        Set(ByVal value As CardConfiguration)
            ccfCardConfig = value
        End Set
    End Property

    Public Sub New()

    End Sub

End Class
