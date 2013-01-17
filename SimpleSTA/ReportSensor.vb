Imports System
Imports System.IO
Imports System.Xml.Serialization
Public Class ReportSensor
    'Inherits Sensor
    Dim dblGlucoseMeanRunIn As Double
    Dim dblGlucoseMeanInjection1 As Double
    Dim dblGlucoseMeanInjection2 As Double
    Dim dblGlucoseMeanInjection3 As Double
    Dim dblGlucoseMeanInjection4 As Double
    Dim dblAPAPMeanRunIn As Double
    Dim dblAPAPMeanInjection1 As Double
    Dim dblAPAPNet As Double
    Dim dblAPAPRatio As Double
    Dim dblRSquared As Double
    Dim dblSlope As Double
    Dim dblIntercept As Double
    Dim dblErrorRunIn As Double
    Dim dblErrorInjection1 As Double
    Dim dblErrorInjection2 As Double
    Dim dblErrorInjection3 As Double
    Dim dblErrorInjection4 As Double
    Dim dtGlucoseInjection1Time As DateTime
    Dim dtGlucoseInjection2Time As DateTime
    Dim dtGlucoseInjection3Time As DateTime
    Dim dtGlucoseInjection4Time As DateTime
    Dim dtAPAPInjectionTime As DateTime
    Dim aryGlucoseReadings(0) As Reading
    Dim aryAPAPReadings(0) As Reading
    Dim intGlucoseInterval As Integer
    Dim intAPAPInverval As Integer
    Dim strSensorID As String
    Public Property GlucoseInterval As Integer
        Get
            Return intGlucoseInterval
        End Get
        Set(ByVal value As Integer)
            intGlucoseInterval = value
        End Set
    End Property
    Public Property APAPInterval As Integer
        Get
            Return intAPAPInverval
        End Get
        Set(ByVal value As Integer)
            intAPAPInverval = value
        End Set
    End Property
    Public Property SensorID As String
        Get
            Return strSensorID
        End Get
        Set(ByVal value As String)
            strSensorID = value
        End Set
    End Property
    Public Property GlucoseReadings As Reading()
        Get
            Return aryGlucoseReadings
        End Get
        Set(ByVal value As Reading())
            aryGlucoseReadings = value
        End Set
    End Property
    Public Property APAPReadings As Reading()
        Get
            Return aryAPAPReadings
        End Get
        Set(ByVal value As Reading())
            aryAPAPReadings = value
        End Set
    End Property
    Public Property APAPInjectionTime As DateTime
        Get
            Return dtAPAPInjectionTime
        End Get
        Set(ByVal value As DateTime)
            dtAPAPInjectionTime = value
        End Set
    End Property

    Public Property GlucoseInjection1Time As DateTime
        Get
            Return dtGlucoseInjection1Time
        End Get
        Set(ByVal value As DateTime)
            dtGlucoseInjection1Time = value
        End Set
    End Property
    Public Property GlucoseInjection2Time As DateTime
        Get
            Return dtGlucoseInjection2Time
        End Get
        Set(ByVal value As DateTime)
            dtGlucoseInjection2Time = value
        End Set
    End Property
    Public Property GlucoseInjection3Time As DateTime
        Get
            Return dtGlucoseInjection3Time
        End Get
        Set(ByVal value As DateTime)
            dtGlucoseInjection3Time = value
        End Set
    End Property
    Public Property GlucoseInjection4Time As DateTime
        Get
            Return dtGlucoseInjection4Time
        End Get
        Set(ByVal value As DateTime)
            dtGlucoseInjection4Time = value
        End Set
    End Property
    Public ReadOnly Property GlucoseRunInCurrent As Double
        Get
            Return dblGlucoseMeanRunIn
        End Get
    End Property
    Public ReadOnly Property GlucoseInjection1Current As Double
        Get
            Return dblGlucoseMeanInjection1
        End Get
    End Property
    Public ReadOnly Property GlucoseInjection2Current As Double
        Get
            Return dblGlucoseMeanInjection2
        End Get
    End Property
    Public ReadOnly Property GlucoseInjection3Current As Double
        Get
            Return dblGlucoseMeanInjection3
        End Get
    End Property
    Public ReadOnly Property GlucoseInjection4Current As Double
        Get
            Return dblGlucoseMeanInjection4
        End Get
    End Property
    Public ReadOnly Property APAPRunInCurrent As Double
        Get
            Return dblAPAPMeanRunIn
        End Get
    End Property
    Public ReadOnly Property APAPInjectionCurrent As Double
        Get
            Return dblAPAPMeanInjection1
        End Get
    End Property
    Public ReadOnly Property APAPNetCurrent As Double
        Get
            Return dblAPAPNet
        End Get
    End Property
    Public ReadOnly Property APAPRatio As Double
        Get
            Return dblAPAPRatio
        End Get
    End Property
    Public ReadOnly Property RSquared As Double
        Get
            Return dblRSquared
        End Get
    End Property
    Public ReadOnly Property Slope As Double
        Get
            Return dblSlope
        End Get
    End Property
    Public ReadOnly Property Intercept As Double
        Get
            Return dblIntercept
        End Get
    End Property
    Public ReadOnly Property RunInError As Double
        Get
            Return dblErrorRunIn
        End Get
    End Property
    Public ReadOnly Property Injection1Error As Double
        Get
            Return dblErrorInjection1
        End Get
    End Property
    Public ReadOnly Property Injection2Error As Double
        Get
            Return dblErrorInjection2
        End Get
    End Property
    Public ReadOnly Property Injection3Error As Double
        Get
            Return dblErrorInjection3
        End Get
    End Property
    Public ReadOnly Property Injection4Error As Double
        Get
            Return dblErrorInjection4
        End Get
    End Property
    Public Sub Analyze()
        ' Complete test analysis here

        ' Calculate Glucose Run-in current
        ' 1. Find first measurement after injection is noted
        ' 2. Count back 32s, then 92s and average the current for all point within this interval
        Dim endCounter As Long = 0
        Dim startIndex As Integer
        Dim endIndex As Integer
        Debug.Print(aryGlucoseReadings.Length)
        For i = 1 To aryGlucoseReadings.Length - 1
            'Debug.Print(aryGlucoseReadings(i).Time)
            If (aryGlucoseReadings(i).Time < dtGlucoseInjection1Time) Then
                endCounter += 1
            Else
                startIndex = endCounter - (92 / intGlucoseInterval)
                endIndex = endCounter - (32 / intGlucoseInterval)
                dblGlucoseMeanRunIn = AverageOfRange(aryGlucoseReadings, startIndex, endIndex)

                startIndex = endCounter + (180 / intGlucoseInterval)
                endIndex = endCounter + (240 / intGlucoseInterval)
                dblGlucoseMeanInjection1 = AverageOfRange(aryGlucoseReadings, startIndex, endIndex)

                Debug.Print(dblGlucoseMeanRunIn)
                Do While i < aryGlucoseReadings.Length
                    If (aryGlucoseReadings(i).Time < dtGlucoseInjection2Time) Then
                        endCounter += 1
                        i += 1
                    Else
                        startIndex = endCounter + (180 / intGlucoseInterval)
                        endIndex = endCounter + (240 / intGlucoseInterval)
                        dblGlucoseMeanInjection2 = AverageOfRange(aryGlucoseReadings, startIndex, endIndex)
                        Do While i < aryGlucoseReadings.Length
                            If (aryGlucoseReadings(i).Time < dtGlucoseInjection3Time) Then
                                endCounter += 1
                                i += 1
                            Else
                                startIndex = endCounter + (180 / intGlucoseInterval)
                                endIndex = endCounter + (240 / intGlucoseInterval)
                                dblGlucoseMeanInjection3 = AverageOfRange(aryGlucoseReadings, startIndex, endIndex)
                                i += 1
                            End If
                        Loop
                    End If
                Loop
            End If
        Next
        'For i = 1 To aryGlucoseReadings.Length - 1 Step 1
        '    If aryGlucoseReadings(i).Time < dtGlucoseInjection1Time Then
        '        ' do nothing
        '        Debug.Print(aryGlucoseReadings(i).Time)
        '        Debug.Print(dtGlucoseInjection1Time)
        '        Debug.Print("Note there yet")
        '        endCounter = endCounter + 1
        '    Else
        '        Debug.Print(aryGlucoseReadings(i).Time)
        '        Debug.Print(dtGlucoseInjection1Time)
        '        Debug.Print(endCounter)
        '        ' Once the first injection has been found, calculate the run-in current

        '        startIndex = endCounter - (92 / intGlucoseInterval)
        '        endIndex = endCounter - (32 / intGlucoseInterval)
        '        dblGlucoseMeanRunIn = AverageOfRange(aryGlucoseReadings, startIndex, endIndex)

        '        ' then calculate the 1st injection interval average
        '        startIndex = endCounter + (180 / intGlucoseInterval)
        '        endIndex = endCounter + (240 / intGlucoseInterval)
        '        dblGlucoseMeanInjection1 = AverageOfRange(aryGlucoseReadings, startIndex, endIndex)

        '        ' now continue to loop through readings until the second injection
        '        Do While endCounter < aryGlucoseReadings.Length
        '            If (aryGlucoseReadings(endCounter).Time < dtGlucoseInjection2Time) Then
        '                endCounter += 1
        '            Else
        '                ' then calculate the 2nd injection interval average
        '                startIndex = endCounter + (180 / intGlucoseInterval)
        '                endIndex = endCounter + (240 / intGlucoseInterval)
        '                dblGlucoseMeanInjection2 = AverageOfRange(aryGlucoseReadings, startIndex, endIndex)
        '                Do While endCounter < aryGlucoseReadings.Length
        '                    If (aryGlucoseReadings(endCounter).Time < dtGlucoseInjection3Time) Then
        '                        endCounter += 1
        '                    Else
        '                        ' then calculate the 3rd injection interval average
        '                        startIndex = endCounter + (180 / intGlucoseInterval)
        '                        endIndex = endCounter + (240 / intGlucoseInterval)
        '                        dblGlucoseMeanInjection3 = AverageOfRange(aryGlucoseReadings, startIndex, endIndex)
        '                        Do While endCounter < aryGlucoseReadings.Length
        '                            If (aryGlucoseReadings(endCounter).Time < dtGlucoseInjection4Time) Then
        '                                endCounter += 1
        '                            Else
        '                                startIndex = endCounter + (180 / intGlucoseInterval)
        '                                endIndex = endCounter + (240 / intGlucoseInterval)
        '                                dblGlucoseMeanInjection4 = AverageOfRange(aryGlucoseReadings, startIndex, endIndex)
        '                                Exit Do
        '                            End If
        '                        Loop
        '                    End If
        '                Loop
        '            End If
        '        Loop
        '    End If
        '    i += 1
        'Next

    End Sub
    Public Function AverageOfRange(ByVal inArray As Reading(), ByVal startIndex As Long, ByVal endIndex As Long) As Double
        Dim aryVals(0) As Double
        Dim average As Double
        Debug.Print("Start Index: " & startIndex)
        Debug.Print("End Index: " & endIndex)
        For i As Long = startIndex To endIndex
            Debug.Print(inArray(i).Time)
            If Not i = startIndex Then
                ReDim Preserve aryVals(aryVals.GetUpperBound(0) + 1)
                aryVals(aryVals.GetUpperBound(0)) = inArray(i).Current
            Else
                aryVals(0) = inArray(i).Current
            End If

        Next
        average = aryVals.Average
        Return average
    End Function
End Class
