Option Explicit On
Imports System.IO
Imports System.Xml.Serialization

Public Module modFileComm
    Dim swDataFile As StreamWriter  'StreamWriter for data file, to remain open until test is complete
    Public boolDataFileOpen As Boolean 'flag to indicate if the data file has been sucessfully created

    ' Name: OpenLogFile()
    ' Variables: None
    ' Description: This function attempts to open a new Log File.  If this is successful, it will set the 
    '              property of the StreamWriter to autoflush data to the file with each attempt to write data.
    'Added 09Jun2017 DB to add logging functionality
    Public Sub OpenLogFile()
        Try
            swLogFile = New StreamWriter(cfgGlobal.DumpDirectory & Path.DirectorySeparatorChar & "Log.txt", False)
            swLogFile.AutoFlush = True

        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

    ' Name: OpenDataFile()
    ' Variables: strFile
    ' Description: This function attempts to open a new Data File.  If this is successful, it will set the 
    '              property of the StreamWriter to autoflush data to the file with each attempt to write data.
    Public Function OpenDataFile(strPath As String, strFile As String) As Boolean
        Try
            If File.Exists(strPath & Path.DirectorySeparatorChar & strFile) Then
                boolDataFileOpen = False
                Return False
            Else
                swDataFile = New StreamWriter(strPath & Path.DirectorySeparatorChar & strFile, True)
                swDataFile.AutoFlush = True
                boolDataFileOpen = True
                Return True
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
            boolDataFileOpen = False
            Return False
        End Try
    End Function

    ' Name: WriteToDataFile()
    ' Variables: strFile
    ' Description: This function attempts to open a new Data File.  If this is successful, it will set the 
    '              property of the StreamWriter to autoflush data to the file with each attempt to write data.
    Public Sub WriteToDataFile(strData As String)
        Try
            swDataFile.WriteLine(strData)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

    ' Name: WriteToLogFile()
    ' Variables: strFile
    ' Description: This function attempts to open a new Data File.  If this is successful, it will set the 
    '              property of the StreamWriter to autoflush data to the file with each attempt to write data.
    'Added 09Jun2017 to create logging of data
    Public Sub WriteToLogFile(strData As String)
        Try
            swLogFile.WriteLine(strData)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

    ' Name: CloseDataFile()
    ' Variables: 
    ' Description: This function closes the data file.

    Public Sub CloseDataFile()
        Try
            swDataFile.Dispose()
            boolDataFileOpen = False
            If boolLogFile Then             'added 13Jun2017 DB to enable/disable logging
                swLogFile.Dispose()         'added 09Jun2017 to close log file when the test is completed
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub



    ' Name: LoadOrRefreshConfiguration()
    ' Returns: Boolean: Indicates success or failure
    ' Description: This function attempts to load cfgGlobal from the configuration file location in memory.  If this is
    '           unsuccessful, it will generate and attempt to save a new configuration object.
    Public Function LoadOrRefreshConfiguration() As Boolean
        Try
            If Not File.Exists(strAppDir & Path.DirectorySeparatorChar & strConfigFileName) Then
                ' If the config file cannot be found, update the UI and create a new object from defaults
                boolConfigStatus = False
                cfgGlobal = New clsConfiguration
            Else
                ' If the file exists, attempt to serialize it to the cfgGlobal object
                Dim srReader As New StreamReader(strAppDir & Path.DirectorySeparatorChar & strConfigFileName)
                Dim xsSerializer As New XmlSerializer(cfgGlobal.GetType)
                cfgGlobal = xsSerializer.Deserialize(srReader)
                srReader.Close()
            End If
            ' Attempt to validate the object
            If cfgGlobal.Validate() Then
                ' If it passes, attempt to write it to file
                If (cfgGlobal.WriteToFile(strAppDir & Path.DirectorySeparatorChar & strConfigFileName)) Then
                    ' Update the UI and return true
                    boolConfigStatus = True
                    Return True
                Else
                    ' update the UI and return false
                    boolConfigStatus = False
                    Return False
                End If
            Else
                ' update the UI and return false
                boolConfigStatus = False
                Return False
            End If
        Catch parseException As InvalidOperationException
            MsgBox("Invalid configuration file.  Delete " & strAppDir & Path.DirectorySeparatorChar & strConfigFileName & " and reload.")
            ' update the UI and return false
            boolConfigStatus = False
            Return False
        Catch ex As Exception
            GenericExceptionHandler(ex)
            ' update the UI and return false
            boolConfigStatus = False
            Return False
        End Try
    End Function
End Module
