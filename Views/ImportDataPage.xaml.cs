﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentManagerWPF.Views
{
    /// <summary>
    /// ImportDataPage.xaml 的交互逻辑
    /// </summary>
    public partial class ImportDataPage : System.Windows.Controls.UserControl
    {
        private List<Student> list = null;
        public ImportDataPage()
        {
            InitializeComponent();
        }
        private void btnChoseExcel_Click(object sender, EventArgs e)
        {
            //打开文件
            OpenFileDialog openFile = new OpenFileDialog();
            DialogResult result = openFile.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = openFile.FileName;//获取文件路径
                list = new DAL.Helper.ImportDataFromExcel().GetStudentByExcel(path);
                //显示数据
                this.dgvStudentList.ItemsSource = null;
                this.dgvStudentList.AutoGenerateColumns = false;
                this.dgvStudentList.ItemsSource = list;
            }
        }
        //保存到数据库
        private void btnSaveToDB_Click(object sender, EventArgs e)
        {
            if (list == null || list.Count == 0)
            {
                System.Windows.MessageBox.Show("目前没有要导入的数据！", "导入提示");
                return;
            }
            try
            {
                if (new DAL.Helper.ImportDataFromExcel().Import(this.list))
                {
                    System.Windows.MessageBox.Show("数据导入成功！", "导入提示");
                    this.dgvStudentList.ItemsSource = null;
                    this.list.Clear();
                }
                else
                {
                    System.Windows.MessageBox.Show("数据导入失败！", "导入提示");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("数据导入失败！具体原因：" + ex.Message, "导入提示");
            }
        }
    }
}
