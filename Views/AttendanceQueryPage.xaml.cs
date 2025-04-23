using DAL;
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
    /// AttendanceQueryPage.xaml 的交互逻辑
    /// </summary>
    public partial class AttendanceQueryPage : UserControl
    {
        private AttendanceService objAService = new AttendanceService();
        public AttendanceQueryPage()
        {
            InitializeComponent();
            dtpTime.SelectedDate = DateTime.Now;
        }
        private void btnQuery_Click(object sender, EventArgs e)
        {
            //查询考勤结果
            DateTime dt1 = Convert.ToDateTime(this.dtpTime.Text);
            DateTime dt2 = dt1.AddDays(1.0);
            this.dgvStudentList.AutoGenerateColumns = false;
            this.dgvStudentList.ItemsSource = objAService.GetStuByDate(dt1, dt2, this.txtName.Text.Trim());
            //new Common.DataGridViewStyle().DgvStyle3(this.dgvStudentList);


            //获取考勤的学员总数
            this.lblCount.Content = objAService.GetAllStudents();
            //显示实际的出勤人数
            this.lblReal.Content = objAService.GetAttendStudents(Convert.ToDateTime(this.dtpTime.Text), false);
            //显示缺勤人数
            this.lblAbsenceCount.Content = (Convert.ToInt32(this.lblCount.Content) - Convert.ToInt32(this.lblReal.Content)).ToString();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
    }
}
