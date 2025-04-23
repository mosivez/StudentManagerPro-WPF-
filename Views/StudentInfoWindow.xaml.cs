using Common;
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
    /// StudentInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StudentInfoWindow : Window
    {
        public StudentInfoWindow()
        {
            InitializeComponent();
        }
        public StudentInfoWindow(StudentExt objStudent)
            : this()
        {
            //显示学员信息
            this.lblStudentName.Content = objStudent.StudentName;
            this.lblStudentIdNo.Content = objStudent.StudentIdNo.ToString();
            this.lblPhoneNumber.Content = objStudent.PhoneNumber;
            this.lblBirthday.Content = objStudent.Birthday.ToShortDateString();
            this.lblAddress.Content = objStudent.StudentAddress;
            this.lblClass.Content = objStudent.ClassName;
            this.lblGender.Content = objStudent.Gender;
            this.lblCardNo.Content = objStudent.CardNo;

            //显示照片
            if (objStudent.StuImage.Length != 0)
            {
                this.pbStu.Source = new BitmapImage(new Uri(objStudent.StuImage, UriKind.Absolute));
                    //(ImageSource)new SerializeObjectToString().DeserializeObject(objStudent.StuImage);
            }
            else
            {
                this.pbStu.Source = new BitmapImage(new Uri("default.png", UriKind.Relative));
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
