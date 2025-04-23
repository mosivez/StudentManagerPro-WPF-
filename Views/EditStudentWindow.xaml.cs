using Common;
using DAL;
using Microsoft.Win32;
using Models;
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
using System.Windows.Shapes;

namespace StudentManagerWPF.Views
{
    /// <summary>
    /// EditStudentWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditStudentWindow : Window
    {
        private StudentClassService objClassService = new StudentClassService();
        private StudentService objStuService = new StudentService();
        OpenFileDialog objFileDialog = new OpenFileDialog();
        public EditStudentWindow(StudentExt objStudent)
        {
            InitializeComponent();
            dtpBirthday.SelectedDate = DateTime.Now;
            //初始化班级下拉框
            this.cboClassName.ItemsSource = objClassService.GetAllClasses();
            this.cboClassName.DisplayMemberPath = "ClassName";//设置下拉框的显示文本
            this.cboClassName.SelectedValuePath = "ClassId";//设置下拉框显示文本对应的value
            this.cboClassName.SelectedIndex = 0;
            //显示学生信息
            this.txtStudentId.Text = objStudent.StudentId.ToString();
            this.txtStudentIdNo.Text = objStudent.StudentIdNo;
            this.txtStudentName.Text = objStudent.StudentName;
            this.txtPhoneNumber.Text = objStudent.PhoneNumber;
            this.txtAddress.Text = objStudent.StudentAddress;
            if (objStudent.Gender == "男") this.rdoMale.IsChecked = true;
            else this.rdoFemale.IsChecked = true;
            this.cboClassName.Text = objStudent.ClassName;
            this.dtpBirthday.Text = objStudent.Birthday.ToShortDateString();
            this.txtCardNo.Text = objStudent.CardNo;
            //显示照片
        //    this.pbStu.Image = objStudent.StuImage.Length != 0 ?
        //      (Image)new SerializeObjectToString().DeserializeObject(objStudent.StuImage) : Image.FromFile("default.png"); ;
        }
        //提交修改
        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            #region  验证数据（身份证号的验证需要修改）

            if (this.txtStudentName.Text.Trim().Length == 0)
            {
                MessageBox.Show("学生姓名不能为空！", "提示信息");
                this.txtStudentName.Focus();
                return;
            }
            //验证性别
            if (this.rdoFemale.IsChecked == false && this.rdoMale.IsChecked == false)
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
            //验证身份证号是否符合要求
            if (!Common.DataValidate.IsIdentityCard(this.txtStudentIdNo.Text.Trim()))
            {
                MessageBox.Show("身份证号不符合要求！", "验证提示");
                this.txtStudentIdNo.Focus();
                return;
            }
            //验证身份证号是否重复
            if (objStuService.IsIdNoExisted(this.txtStudentIdNo.Text.Trim(), this.txtStudentId.Text.Trim()))
            {
                MessageBox.Show("身份证号不能和现有学员身份证号重复！", "验证提示");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            //验证身份证号是否和出生日期相吻合
            string month = string.Empty;
            string day = string.Empty;
            if (Convert.ToDateTime(this.dtpBirthday.Text).Month < 10)
                month = "0" + Convert.ToDateTime(this.dtpBirthday.Text).Month;
            if (Convert.ToDateTime(this.dtpBirthday.Text).Day < 10)
                day = "0" + Convert.ToDateTime(this.dtpBirthday.Text).Day;
            string birthday = Convert.ToDateTime(this.dtpBirthday.Text).Year.ToString() + month + day;

            if (!this.txtStudentIdNo.Text.Trim().Contains(birthday))
            {
                MessageBox.Show("身份证号和出生日期不匹配！", "验证提示");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            //验证出生日期
            int age = DateTime.Now.Year - Convert.ToDateTime(this.dtpBirthday.Text).Year;
            if (age < 18)
            {
                MessageBox.Show("学生年龄不能小于18岁！", "验证提示");
                return;
            }
            #endregion
            #region 封装学生对象
            Student objStudent = new Student()
            {
                StudentName = this.txtStudentName.Text.Trim(),
                Gender = this.rdoMale.IsChecked == true ? "男" : "女",
                Birthday = Convert.ToDateTime(this.dtpBirthday.Text),
                StudentIdNo = this.txtStudentIdNo.Text.Trim(),
                PhoneNumber = this.txtPhoneNumber.Text.Trim(),
                StudentAddress = this.txtAddress.Text.Trim() == "" ? "地址不详" : this.txtAddress.Text.Trim(),             
                CardNo = this.txtCardNo.Text.Trim(),
                ClassId = Convert.ToInt32(this.cboClassName.SelectedValue),//获取选择班级对应classId
                Age = DateTime.Now.Year - Convert.ToDateTime(this.dtpBirthday.Text).Year,
                StudentId = Convert.ToInt32(this.txtStudentId.Text.Trim()),
                StuImage = this.pbStu.Source != null ? objFileDialog.FileName : ""
            };
            #endregion
            #region 修改对象
            try
            {
                if (objStuService.ModifyStudent(objStudent) == 1)
                {
                    MessageBox.Show("学员信息修改成功!", "提示信息");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion
        }
        //选择照片
        private void btnChoseImage_Click(object sender, EventArgs e)
        {
            Nullable<bool> result = objFileDialog.ShowDialog();
            if (result == true)
                pbStu.Source = new BitmapImage(new Uri(objFileDialog.FileName, UriKind.Absolute)); 
        }
        //关闭窗口
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
