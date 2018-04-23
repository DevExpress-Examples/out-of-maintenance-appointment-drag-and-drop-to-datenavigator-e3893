using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors.Calendar;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Drawing;

namespace XtraSchedulerTest
{
    public partial class Form1 : Form
    {
        DateTime DateNavigatorHoverFirstDate = DateTime.MinValue;

        public Form1() {
            InitializeComponent();

            //Create resources
            for (int i = 0; i < 4; i++) {
                Resource r = new Resource(i, string.Format("Resource {0}", i));
                schedulerStorage1.Resources.Add(r);

                for (int i1 = 0; i1 < 3; i1++) {
                    Appointment apt = new Appointment(AppointmentType.Normal,
                        DateTime.Today.AddDays(i).AddHours(i + 0), DateTime.Today.AddDays(i).AddHours(i + 1),
                        string.Format("Appointment {0} {1}", i, i1));
                    apt.ResourceId = r.Id;
                    schedulerStorage1.Appointments.Add(apt);
                }
            }

            schedulerControl1.Start = DateTime.Now;
            schedulerControl1.AppointmentDrag += new AppointmentDragEventHandler(schedulerControl1_AppointmentDrag);
            schedulerControl1.GroupType = SchedulerGroupType.None;
        }


        void schedulerControl1_AppointmentDrag(object sender, AppointmentDragEventArgs e) {
            e.ForceUpdateFromStorage = true;
        }
     
        private void dateNavigator1_DragOver(object sender, DragEventArgs e) {
            if (schedulerControl1.ActiveView.Type != SchedulerViewType.Day)
                return;           

            DateTime cellDate = GetDateNavigatorCellDateTimeFromPoint(new Point(e.X, e.Y));
            if (cellDate != DateTime.MinValue &&
                !schedulerControl1.ActiveView.GetVisibleIntervals().Contains(new TimeInterval(cellDate, TimeSpan.FromHours(24))))
            {
                if (DateNavigatorHoverFirstDate == DateTime.MinValue)
                {
                    DateNavigatorHoverFirstDate = cellDate;
                    timer1.Start();
                }
                else if (DateNavigatorHoverFirstDate != cellDate)
                {
                    timer1.Stop();
                    DateNavigatorHoverFirstDate = cellDate;
                    timer1.Start();
                }
            }
        }

        private DateTime GetDateNavigatorCellDateTimeFromPoint(Point p)
        {
            Point pt = Point.Empty;
            this.Invoke(new MethodInvoker(delegate()
            {
                pt = dateNavigator1.PointToClient(p);
            }));

            CalendarHitInfo hitInfo = dateNavigator1.GetHitInfo(new MouseEventArgs(MouseButtons.None, 0, pt.X, pt.Y, 0));
            if (hitInfo.InfoType == CalendarHitInfoType.MonthNumber)
            {
                return ((DateNavigatorDayNumberCellInfo)hitInfo.HitObject).Date;
            }

            return DateTime.MinValue;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
  
            timer1.Stop();

            DateTime cellDate = GetDateNavigatorCellDateTimeFromPoint(MousePosition);

            if (DateNavigatorHoverFirstDate == cellDate)
            {                
                this.Invoke(new MethodInvoker(delegate()
                {
                    schedulerControl1.GoToDate(DateNavigatorHoverFirstDate);             
                }));
            }

            DateNavigatorHoverFirstDate = DateTime.MinValue;
        } 
    }
}