using DAL;
using Models;
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

namespace StudentManagerWPF
{
    /// <summary>
    /// UserLoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UserLoginWindow : Window
    {
        //创建数据访问类对象
        private AdminService objAdminService = new AdminService();
        public UserLoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //【1】数据验证
            if (this.txtLoginId.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入登陆账号！", "登录提示");
                this.txtLoginId.Focus();
                return;
            }
            if (this.txtLoginPwd.Password.Length == 0)
            {
                MessageBox.Show("请输入登陆密码！", "登录提示");
                this.txtLoginPwd.Focus();
                return;
            }
            //【2】封装对象(封装的是登陆账号和密码)
            Admin objAdmin = new Admin()
            {
                LoginId = Convert.ToInt32(txtLoginId.Text.Trim()),
                LoginPwd = this.txtLoginPwd.Password.ToString(),
            };
            //【3】和后台交互，判断登陆信息是否正确

            try
            {
                App.currentAdmin = new AdminService().AdminLogin(objAdmin);
                if (App.currentAdmin != null)
                {
                    //保存登陆信息
                    App.objCurentAdmin = objAdmin;
                    //设置登陆窗体的返回值
                    this.DialogResult = Convert.ToBoolean(1);
                    //关闭窗体
                    this.Close();

                }
                else
                {
                    MessageBox.Show("用户名或密码错误！", "提示信息");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据访问出现异常，登录失败！具体原因：" + ex.Message);
            }

        }

        //关闭
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 改善用户体验
        private void txtLoginId_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtLoginId.Text.Length == 13)
            {
                if (this.txtLoginId.Text.Length != 0)
                {
                    this.txtLoginPwd.Focus();//将当前焦点跳转到密码框
                }
            }
        }

        private void pswLoginPwd_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyValue == 13)
            //{
            //    btnLogin_Click(null, null);
            //}
        }
        #endregion

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
