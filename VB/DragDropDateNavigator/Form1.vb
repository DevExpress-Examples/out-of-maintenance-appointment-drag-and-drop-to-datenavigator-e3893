Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.XtraEditors.Calendar
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Drawing

Namespace XtraSchedulerTest
	Partial Public Class Form1
		Inherits Form
		Private DateNavigatorHoverFirstDate As DateTime = DateTime.MinValue

		Public Sub New()
			InitializeComponent()

			'Create resources
			For i As Integer = 0 To 3
				Dim r As New Resource(i, String.Format("Resource {0}", i))
				schedulerStorage1.Resources.Add(r)

				For i1 As Integer = 0 To 2
					Dim apt As New Appointment(AppointmentType.Normal, DateTime.Today.AddDays(i).AddHours(i + 0), DateTime.Today.AddDays(i).AddHours(i + 1), String.Format("Appointment {0} {1}", i, i1))
					apt.ResourceId = r.Id
					schedulerStorage1.Appointments.Add(apt)
				Next i1
			Next i

			schedulerControl1.Start = DateTime.Now
			AddHandler schedulerControl1.AppointmentDrag, AddressOf schedulerControl1_AppointmentDrag
			schedulerControl1.GroupType = SchedulerGroupType.None
		End Sub


		Private Sub schedulerControl1_AppointmentDrag(ByVal sender As Object, ByVal e As AppointmentDragEventArgs) Handles schedulerControl1.AppointmentDrag
			e.ForceUpdateFromStorage = True
			e.Handled = True
		End Sub

		Private Sub dateNavigator1_DragOver(ByVal sender As Object, ByVal e As DragEventArgs) Handles dateNavigator1.DragOver
			If schedulerControl1.ActiveView.Type <> SchedulerViewType.Day Then
				Return
			End If

			Dim cellDate As DateTime = GetDateNavigatorCellDateTimeFromPoint(New Point(e.X, e.Y))
			If cellDate <> DateTime.MinValue AndAlso (Not schedulerControl1.ActiveView.GetVisibleIntervals().Contains(New TimeInterval(cellDate, TimeSpan.FromHours(24)))) Then
				If DateNavigatorHoverFirstDate = DateTime.MinValue Then
					DateNavigatorHoverFirstDate = cellDate
					timer1.Start()
				ElseIf DateNavigatorHoverFirstDate <> cellDate Then
					timer1.Stop()
					DateNavigatorHoverFirstDate = cellDate
					timer1.Start()
				End If
			End If
		End Sub

		Private Function GetDateNavigatorCellDateTimeFromPoint(ByVal p As Point) As DateTime
			Dim pt As Point = Point.Empty
			Me.Invoke(New MethodInvoker(Function() AnonymousMethod1(p, pt)))

			Dim hitInfo As CalendarHitInfo = dateNavigator1.GetHitInfo(New MouseEventArgs(MouseButtons.None, 0, pt.X, pt.Y, 0))
			If hitInfo.InfoType = CalendarHitInfoType.MonthNumber Then
				Return (CType(hitInfo.HitObject, DateNavigatorDayNumberCellInfo)).Date
			End If

			Return DateTime.MinValue
		End Function
		
		Private Function AnonymousMethod1(ByVal p As Point, ByVal pt As Point) As Boolean
			pt = dateNavigator1.PointToClient(p)
			Return True
		End Function

		Private Sub timer1_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles timer1.Tick

			timer1.Stop()

			Dim cellDate As DateTime = GetDateNavigatorCellDateTimeFromPoint(MousePosition)

			If DateNavigatorHoverFirstDate = cellDate Then
				Me.Invoke(New MethodInvoker(Function() AnonymousMethod2()))
			End If

			DateNavigatorHoverFirstDate = DateTime.MinValue
		End Sub
		
		Private Function AnonymousMethod2() As Boolean
			schedulerControl1.GoToDate(DateNavigatorHoverFirstDate)
			Return True
		End Function
	End Class
End Namespace