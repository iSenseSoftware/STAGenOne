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
