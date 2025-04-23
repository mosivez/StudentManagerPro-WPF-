using Common;
using DAL;
using Microsoft.Win32;
using Models;
using MyVideo;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// AddStuPage.xaml 的交互逻辑
    /// </summary>
    public partial class AddStuPage : UserControl
    {     
        //创建数据访问对象
        private StudentClassService objClassService = new StudentClassService();
        private StudentService objStudentService = new StudentService();
        private List<Student> stuList = new List<Student>();
        OpenFileDialog objFileDialog = new OpenFileDialog();
        //private Video objVideo;
        public AddStuPage()
        {
            InitializeComponent();

            dtpBirthday.SelectedDate = DateTime.Now;
            //初始化班级下拉框
            this.cboClassName.ItemsSource = objClassService.GetAllClasses();
            this.cboClassName.DisplayMemberPath = "ClassName";//设置下拉框的显示文本
            this.cboClassName.SelectedValuePath = "ClassId";//设置下拉框显示文本对应的value
            this.cboClassName.SelectedIndex = 0;
            //this.KeyPreview = true;//设置窗体接收键盘按下事件

            this.dgvStudentList.AutoGenerateColumns = false;
        }

        //添加新学员
        private void btnAdd_Click(object sender, EventArgs e)
        {
            #region  验证数据

            if (this.txtStudentName.Text.Trim().Length == 0)
            {
                MessageBox.Show("学生姓名不能为空！", "提示信息");
                this.txtStudentName.Focus();
                return;
            }
            if (this.txtCardNo.Text.Trim().Length == 0)
            {
                MessageBox.Show("考勤卡号不能为空！", "提示信息");
                this.txtCardNo.Focus();
                return;
            }
            //验证性别
            if (this.rdoFemale.IsChecked==false&& this.rdoMale.IsChecked == false)
            {
                MessageBox.Show("请选择学生性别！", "提示信息");
                return;
            }
            //验证班级
            if (this.cboClassName.SelectedIndex == -1)
            {
                MessageBox.Show("请选择班级！", "提示信息");
                return;
            }
            //验证年龄
            int age = DateTime.Now.Year - Convert.ToDateTime(this.dtpBirthday.Text).Year;
            if (age > 35 && age < 18)
            {
                MessageBox.Show("年龄必须在28-35岁之间！", "提示信息");
                return;
            }
            //验证身份证号是否符合要求
            if (!DataValidate.IsIdentityCard(this.txtStudentIdNo.Text.Trim()))
            {
                MessageBox.Show("身份证号不符合要求！", "验证提示");
                this.txtStudentIdNo.Focus();
                return;
            }


            if (!this.txtStudentIdNo.Text.Trim().Contains(Convert.ToDateTime(this.dtpBirthday.Text).ToString("yyyyMMdd")))
            {
                MessageBox.Show("身份证号和出生日期不匹配！", "验证提示");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            //验证身份证号是否重复
            if (objStudentService.IsIdNoExisted(this.txtStudentIdNo.Text.Trim()))
            {
                MessageBox.Show("身份证号不能和现有学员身份证号重复！", "验证提示");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            //验证卡号是否重复
            if (objStudentService.IsCardNoExisted(this.txtCardNo.Text.Trim()))
            {
                MessageBox.Show("当前卡号已经存在！", "验证提示");
                this.txtCardNo.Focus();
                this.txtCardNo.SelectAll();
                return;
            }
            #endregion

            #region 封装学生对象
            Student objStudent = new Student()
            {
                StudentName = this.txtStudentName.Text.Trim(),
                Gender = this.rdoMale.IsChecked==true ? "男" : "女",
                Birthday = Convert.ToDateTime(this.dtpBirthday.Text),
                StudentIdNo = this.txtStudentIdNo.Text.Trim(),
                PhoneNumber = this.txtPhoneNumber.Text.Trim(),
                ClassName = this.cboClassName.Text,
                StudentAddress = this.txtAddress.Text.Trim() == "" ? "地址不详" : this.txtAddress.Text.Trim(),
                CardNo = this.txtCardNo.Text.Trim(),
                ClassId = Convert.ToInt32(this.cboClassName.SelectedValue),//获取选择班级对应classId
                Age = DateTime.Now.Year - Convert.ToDateTime(this.dtpBirthday.Text).Year,
                StuImage = this.imgPic.Source != null ? objFileDialog.FileName : ""
            };
            #endregion

            #region 调用后台数据访问方法添加对象
            try
            {
                int studentId = objStudentService.AddStudent(objStudent);
                if (studentId > 1)
                {
                    //同步显示添加的学员
                    objStudent.StudentId = studentId;
                    this.stuList.Add(objStudent);
                    this.dgvStudentList.ItemsSource = null;
                    this.dgvStudentList.ItemsSource = this.stuList;
                    //询问是否继续添加
                    MessageBoxResult result = MessageBox.Show("新学员添加成功!是否继续添加？", "提示信息",
                           MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)//清除用户输入的信息
                    {
                        foreach (TextBox ctrls in Grid_InfoTxt.Children)
                        {
                           if (ctrls is TextBox) ctrls.Text = "";
                        }
                        this.cboClassName.SelectedIndex = -1;
                        this.rdoFemale.IsChecked = false;
                        this.rdoMale.IsChecked = false;
                        this.txtStudentName.Focus();
                        this.imgPic.Source = null;
                    }
                    else this.Visibility=Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion
        }

        //关闭窗体
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
        private void userAddStu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)//按下ESC键关闭窗体
            {
                this.Visibility = Visibility.Hidden;
            }
        }
        //选择新照片
        private void btnChoseImage_Click(object sender, EventArgs e)
        {         
            Nullable<bool> result = objFileDialog.ShowDialog();
            if (result == true)
                imgPic.Source = new BitmapImage(new Uri(objFileDialog.FileName, UriKind.RelativeOrAbsolute)); 
        }
        //启动摄像头
        private void btnStartVideo_Click(object sender, EventArgs e)
        {

        }
        //拍照
        private void btnTake_Click(object sender, EventArgs e)
        {

        }
        //清除照片
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.imgPic.Source = null;
        }
    }
}
