Option Explicit On
Imports System.IO
Imports System.Net.Sockets

Module modGlobalVariables
    ' -----------------------------------------------------------------------------------------------------------
    ' The modGlobalVariables, as the name suggests, a collection of global variables for use in all objects 
    ' and forms
    ' -----------------------------------------------------------------------------------------------------------
    ' Testing, testing, 1,2,3

    'Constants
    Public Const strConfigFileName As String = "Config.xml"
    Public Const strApplicationName As String = "GlucoMatrix"
    Public Const strApplicationVersion As String = "0.0.2"
    Public Const strApplicationDescription As String = "Software for CGM Sensor release testing"
    ' The admin password to unlock the configuration settings is hardcoded.  In the future
    ' it may be desireable to incorporate database-driven user authentication / authorization for granular permissions
    Public strAdminPassword As String = "C0balt22"

    'Hardware Test Global Variables
    Public Const strAuditVolt As String = "0.65"
    Public Const strAuditCurrentRange As String = "0.000001"
    Public Const strAuditFilterType As String = "FILTER_REPEAT_AVG"
    Public Const strAuditFilterCount As String = "1"
    Public Const strAuditNPLC As String = "1"
    Public strHardwareErrorList As String 'Lists Hardware Errors 
    Public boolAuditPassFail() As Boolean
    Public boolAuditVerificationFailure As Boolean

    'Configuration Variables
    Public cfgGlobal As New clsConfiguration ' Global inexstance of the Configuration object that stores config information for the current session

    'File/Directory Variables
    Public strAppDir As String = My.Application.Info.DirectoryPath ' The path to the application install directory
    Public fCurrentTestFile As File

    'Status Variables
    Public boolConfigStatus As Boolean
    Public boolIOStatus As Boolean

    'Test Variables
    Public strTestID As String
    Public strSensorIDHeader As String

End Module
