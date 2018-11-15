<!-- default file list -->
*Files to look at*:

* [Form1.cs](./CS/DragDropDateNavigator/Form1.cs) (VB: [Form1.vb](./VB/DragDropDateNavigator/Form1.vb))
<!-- default file list end -->
# Appointment Drag-and-Drop to DateNavigator


<p>This example demonstrates how to implement the following scenario.<br />
An appointment is dragged from the Scheduler to the date within the DateNavigator control. The Scheduler DayView navigates to that date. The appointment being dragged is moved to that date. After dropping to the selected date, you can adjust an appointment as your needs dictate within the DayView.</p><p>The GetHitInfo method is used to get the date of the hovered cell in the DateNavigator. Navigation is performed by timer which calls the <a href="http://documentation.devexpress.com/#WindowsForms/DevExpressXtraSchedulerSchedulerControl_GoToDatetopic1553"><u>GotoDate</u></a> method of the Scheduler. The timer is triggered within the <strong>DragOver</strong> event handler of the DateNavigator control.<br />
To refresh the Scheduler View after it navigates to the hovered date, the <a href="http://documentation.devexpress.com/#WindowsForms/DevExpressXtraSchedulerSchedulerControl_AppointmentDragtopic"><u>AppointmentDrag</u></a> event is handled to set the <strong>AppointmentDragEventArgs.ForceUpdateFromStorage</strong> property to <strong>true</strong>.</p>

<br/>


