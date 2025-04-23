using DAL;
using Models;
using Models.Ext;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// StuManagePage.xaml 的交互逻辑
    /// </summary>
    public partial class StuManagePage : UserControl
    {
        private StudentClassService objClassService = new StudentClassService();
        private StudentService objStuService = new StudentService();
        private List<StudentExt> list = null;
        public StuManagePage()
        {
            InitializeComponent();
            //初始化班级下拉框
            this.cboClass.ItemsSource = objClassService.GetAllClasses();
            this.cboClass.DisplayMemberPath = "ClassName";//设置下拉框的显示文本
            this.cboClass.SelectedValuePath = "ClassId";//设置下拉框显示文本对应的value
            this.cboClass.SelectedIndex = 0;
        }
        //按照班级查询
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.cboClass.SelectedIndex == -1)
            {
                MessageBox.Show("请选则班级！", "提示");
                return;
            }
            this.dgvStudentList.AutoGenerateColumns = false;    //不显示未封装的属性
            //执行查询并绑定数据
            list = objStuService.GetStudentByClass(this.cboClass.Text);
            this.dgvStudentList.ItemsSource = list;
            //new Common.DataGridViewStyle().DgvStyle1(this.dgvStudentList);

        }

        //根据学号查询
        private void btnQueryById_Click(object sender, EventArgs e)
        {
            if (this.txtStudentId.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入学号！", "提示信息");
                this.txtStudentId.Focus();
                return;
            }
            if (!Common.DataValidate.IsInteger(this.txtStudentId.Text.Trim()))
            {
                MessageBox.Show("学号必须是正整数！", "提示信息");
                this.txtStudentId.SelectAll();
                this.txtStudentId.Focus();
                return;
            }
            StudentExt objStudent = objStuService.GetStudentById(this.txtStudentId.Text.Trim());
            if (objStudent == null)
            {
                MessageBox.Show("学员信息不存在！", "提示信息");
                this.txtStudentId.Focus();
            }
            else
            {
                StudentInfoWindow objStuInfo = new StudentInfoWindow(objStudent);
                objStuInfo.Show();
            }
        }
        private void txtStudentId_KeyDown(object sender, KeyEventArgs e)
        {
            //if (this.txtStudentId.Text.Trim().Length != 0 )
            //    btnQueryById_Click(null, null);
        }

        //双击选中的学员对象并显示详细信息
        private void dgvStudentList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var a = this.dgvStudentList.SelectedItem as StudentExt;
            txtStudentId.Text = a.StudentId.ToString();
            StudentExt objStudent = objStuService.GetStudentById(this.txtStudentId.Text.Trim());
            StudentInfoWindow objStuInfo = new StudentInfoWindow(objStudent);
            objStuInfo.Show();

        }

        //打印当前学员信息
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            //如果没有列表显示则不显示详细信息
            var a = this.dgvStudentList.SelectedItem as StudentExt;
            if (this.dgvStudentList.ItemsSource==null||a.StudentId==null)
                return;
            //获取当前行的学号
            string stuId = a.StudentId.ToString();
            //根据学号获取学生对象
            StudentExt objStuExt = objStuService.GetStudentById(stuId);
            //调用Excel模块实现打印预览
            //PrintStudent objPrint = new PrintStudent();
            //objPrint.ExecutePrint(objStuExt);
        }
        //修改学员对象
        private void btnEidt_Click(object sender, RoutedEventArgs e)
        {
            var a = this.dgvStudentList.SelectedItem as StudentExt;
            if(a==null)
            {
                MessageBox.Show("没有任何需要修改的学员信息！", "提示");
                return;
            }

            //获取学号
            
            string studentId = a.StudentId.ToString();
            StudentExt objStudent = objStuService.GetStudentById(studentId); //根据学号获取学员对象
            //显示修改学员信息窗口
            EditStudentWindow objEditStudent = new EditStudentWindow(objStudent);
            objEditStudent.ShowDialog();
            btnQuery_Click(null, null);   //同步刷新显示
        }

        //删除学员对象
        private void btnDel_Click(object sender, EventArgs e)
        {
            var a = this.dgvStudentList.SelectedItem as StudentExt;
            if (a == null)
            {
                MessageBox.Show("没有任何需要删除的学员！", "提示");
                return;
            }
            //删除确认
            MessageBoxResult result = MessageBox.Show("确实要删除吗？", "删除确认", MessageBoxButton.OKCancel,
                 MessageBoxImage.Question);
            if (result == MessageBoxResult.Cancel) return;
            //获取学号并调用后台执行删除
            string studentId = a.StudentId.ToString();
            try
            {
                if (objStuService.DeleteStudentById(studentId) == 1)
                    btnQuery_Click(null, null);  //同步刷新显示
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示信息");
            }
        }

        //姓名降序
        private void btnNameDESC_Click(object sender, EventArgs e)
        {
            if (dgvStudentList.ItemsSource == null)
            {
                return;
            }
            list.Sort(new NameDESC());
            this.dgvStudentList.ItemsSource = null;
            this.dgvStudentList.ItemsSource = list;  //同步刷新显示
        }

        //学号降序
        private void btnStuIdDESC_Click(object sender, EventArgs e)
        {
            if (dgvStudentList.ItemsSource == null)
            {
                return;
            }
            list.Sort(new StuIdDESC());
            this.dgvStudentList.ItemsSource = null;
            dgvStudentList.ItemsSource = list;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        #region 实现排序
        class NameDESC : IComparer<Student>
        {
            public int Compare(Student x, Student y)
            {
                return y.StudentName.CompareTo(x.StudentName);
            }
        }
        class StuIdDESC : IComparer<Student>
        {
            public int Compare(Student x, Student y)
            {
                return y.StudentId.CompareTo(x.StudentId);
            }
        }
        #endregion

        
    }
}
