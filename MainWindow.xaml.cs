using StudentManagerWPF.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace StudentManagerWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            #region 登陆窗体验证

            UserLoginWindow loginWindow = new UserLoginWindow();

            loginWindow.ShowDialog();

            if (loginWindow.DialogResult != Convert.ToBoolean(1))
            {
                Environment.Exit(0);              
            }
            #endregion
            try
            {
                //显示当前用户
                lblCurrentUser.Text = App.objCurentAdmin.AdminName + "]";
                //显示版本号
                this.lblVersion.Text = "版本号 : " + ConfigurationManager.AppSettings["pversion"].ToString();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }

        #region 添加学员
        private void menuAddStu_Click(object sender, RoutedEventArgs e)
        {
            btnAddStu_Click(null, null);
        }
        private void btnAddStu_Click(object sender, RoutedEventArgs e)
        {
            Grid_Content.Children.Clear();
            AddStuPage addStudent = new AddStuPage();
            Grid_Content.Children.Add(addStudent);
        }

        #endregion

        #region 学员管理
        private void menuManagerStu_Click(object sender, RoutedEventArgs e)
        {
            btnStuManage_Click(null, null);
        }
        private void btnStuManage_Click(object sender, RoutedEventArgs e)
        {
            Grid_Content.Children.Clear();
            StuManagePage stuManager = new StuManagePage();
            Grid_Content.Children.Add(stuManager);
        }
        #endregion

        #region 考勤打卡
        private void btnCard_Click(object sender, RoutedEventArgs e)
        {
            Grid_Content.Children.Clear();
            AttendancePage attendance = new AttendancePage();
            Grid_Content.Children.Add(attendance);
        }
        #endregion

        #region 考勤查询
        private void btnAttendanceQuery_Click(object sender, RoutedEventArgs e)
        {
            Grid_Content.Children.Clear();
            AttendanceQueryPage attendanceQuery = new AttendanceQueryPage();
            Grid_Content.Children.Add(attendanceQuery);
        }
        #endregion

        #region 成绩浏览
        private void btnScoreQuery_Click(object sender, RoutedEventArgs e)
        {
            Grid_Content.Children.Clear();
            ScoreQueryPage scoreQuery = new ScoreQueryPage();
            Grid_Content.Children.Add(scoreQuery);
        }
        #endregion

        #region 成绩分析
        private void btnScoreAnalasys_Click(object sender, RoutedEventArgs e)
        {
            Grid_Content.Children.Clear();
            ScoreManagePage scoreManage = new ScoreManagePage();
            Grid_Content.Children.Add(scoreManage);
        }
        #endregion

        #region 批量导入
        private void btnImportStu_Click(object sender, RoutedEventArgs e)
        {
            Grid_Content.Children.Clear();
            ImportDataPage importData = new ImportDataPage();
            Grid_Content.Children.Add(importData);
        }
        #endregion

        #region 退出系统确认

        //退出系统
        private void tmiClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void LayoutWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认退出吗？", "退出询问",
               MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result != MessageBoxResult.OK)
            {
                e.Cancel = true;
            }
        }
        #endregion

        #region 密码修改
        private void btnModifyPwd_Click(object sender, RoutedEventArgs e)
        {
            Grid_Content.Children.Clear();
            ModifyPwdWindow modifyPwd = new ModifyPwdWindow();
            modifyPwd.ShowDialog();
        }
        #endregion

        #region 账号切换
        private void btnChangeAccount_Click(object sender, RoutedEventArgs e)
        {
            Grid_Content.Children.Clear();
            //UserLoginWindow userLogin = new UserLoginWindow();
            //userLogin.Show();

            //创建登录窗体
            UserLoginWindow objUserLogin = new UserLoginWindow();
            objUserLogin.Title = "[账号切换]";
            System.Windows.Forms.DialogResult result;
            if (objUserLogin.ShowDialog() == true)
            {
                result = System.Windows.Forms.DialogResult.OK;
                //根据窗体返回值判断用户登录是否成功
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    this.lblCurrentUser.Text = App.currentAdmin.AdminName + "]";
                }
            };
        }
        #endregion

        #region 访问官网
        private void btnGoXiketang_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("iexplore.exe", "http://www.xiketang.com");
        }
        #endregion

        #region 快捷键

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            try
            {
                if (e.Key == Key.F2)
                {
                    this.Close();
                }
                else if (e.Key == Key.Escape)
                {
                    this.WindowState = WindowState.Normal;
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
                else if (e.Key == Key.F4)
                {
                    this.WindowState = WindowState.Maximized;
                    this.WindowStyle = WindowStyle.None;
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion       


    }
}
