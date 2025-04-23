using Common;
using DAL;
using Models.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentManagerWPF.Views
{
    /// <summary>
    /// AttendancePage.xaml 的交互逻辑
    /// </summary>
    public partial class AttendancePage : UserControl
    {
        private AttendanceService objAttendanceService = new AttendanceService();
        public AttendancePage()
        {
            InitializeComponent();
            //获取考勤的学员总数
            this.lblCount.Content = objAttendanceService.GetAllStudents();
            timer1_Tick(null, null);
            ShowStat();
        }
        private void ShowStat()
        {
            //显示实际的出勤人数
            this.lblReal.Content = objAttendanceService.GetAttendStudents(DateTime.Now, true);
            //显示缺勤人数
            this.lblAbsenceCount.Content = (Convert.ToInt32(this.lblCount.Content) - Convert.ToInt32(this.lblReal.Content)).ToString();
        }
        //显示当前时间
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.lblYear.Content = DateTime.Now.Year.ToString();
            this.lblMonth.Content = DateTime.Now.Month.ToString();
            this.lblDay.Content = DateTime.Now.Day.ToString();
            this.lblTime.Content = DateTime.Now.ToLongTimeString();

            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    this.lblWeek.Content = "一";
                    break;
                case DayOfWeek.Tuesday:
                    this.lblWeek.Content = "二";
                    break;
                case DayOfWeek.Wednesday:
                    this.lblWeek.Content = "三";
                    break;
                case DayOfWeek.Thursday:
                    this.lblWeek.Content = "四";
                    break;
                case DayOfWeek.Friday:
                    this.lblWeek.Content = "五";
                    break;
                case DayOfWeek.Saturday:
                    this.lblWeek.Content = "六";
                    break;
                case DayOfWeek.Sunday:
                    this.lblWeek.Content = "日";
                    break;
            }
        }

        //学员打卡        
        private void txtStuCardNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtStuCardNo.Text.Trim().Length ==10)
            {
                //显示学员信息
                StudentExt objStu = new StudentService().GetStudentByCardNo(this.txtStuCardNo.Text.Trim());
                if (objStu == null)
                {
                    MessageBox.Show("卡号不正确！", "信息提示");
                    this.lblInfo.Content = "打卡失败！";
                    this.txtStuCardNo.SelectAll();
                    this.lblStuName.Content = "";
                    this.lblStuClass.Content = "";
                    this.lblStuId.Content = "";
                    this.pbStu.Source = null;
                    return;
                }
                this.lblStuName.Content = objStu.StudentName;
                this.lblStuClass.Content = objStu.ClassName;
                this.lblStuId.Content = objStu.StudentId.ToString();
                if (objStu.StuImage != null && objStu.StuImage.Length != 0)
                    this.pbStu.Source = new BitmapImage(new Uri(objStu.StuImage, UriKind.Absolute));
                else
                    this.pbStu.Source = new BitmapImage(new Uri("default.png", UriKind.Relative));
                //添加打卡信息
                string result = objAttendanceService.AddRecord(this.txtStuCardNo.Text.Trim());
                if (result != "success")
                {
                    this.lblInfo.Content = "打卡失败！";
                    MessageBox.Show(result, "错误提示");
                }
                else
                {
                    this.lblInfo.Content = "打卡成功！";
                    ShowStat();
                    this.txtStuCardNo.Text = ""; //等待下一个打卡
                    this.txtStuCardNo.Focus();
                }
            }
        }

        //结束打卡
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visibility=Visibility.Hidden;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtStuCardNo.Focus();
        }
    }
}
