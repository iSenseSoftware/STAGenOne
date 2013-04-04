Public Class TestSystem
    Dim arySwitches(0) As Switch
    Dim arySources(0) As SourceMeter
    Public Property Switches As Switch()
        Get
            Return arySwitches
        End Get
        Set(value As Switch())
            arySwitches = value
        End Set
    End Property
    Public Property Sources As SourceMeter()
        Get
            Return arySources
        End Get
        Set(value As SourceMeter())
            arySources = value
        End Set
    End Property
    Public Sub AddSwitch()

    End Sub
    Public Sub AddSource()

    End Sub
    Public Function GetSourceBySerial(ByVal strSerial As String) As SourceMeter
        Return arySources(0)
    End Function
    Public Function GetSwitchBySerial(ByVal strSerial As String) As Switch
        Return arySwitches(0)
    End Function
End Class
