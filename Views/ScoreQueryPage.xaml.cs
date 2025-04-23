using DAL;
using Models;
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
    /// ScoreQueryPage.xaml 的交互逻辑
    /// </summary>
    public partial class ScoreQueryPage : UserControl
    {
        private ScoreListService objListService = new ScoreListService();
        private DataSet ds = null;//保存全部查询结果的数据集
        private List<StudentClass> list = null;
        public ScoreQueryPage()
        {
            InitializeComponent();

            DataTable dt = new StudentClassService().GetAllClass().Tables[0];
            //初始化班级下拉框
            this.cboClass.ItemsSource = new StudentClassService().GetAllClasses();
            this.cboClass.DisplayMemberPath = "ClassName";//设置下拉框的显示文本
            this.cboClass.SelectedValuePath = "ClassId";//设置下拉框显示文本对应的value
            //this.cboClass.SelectedIndex = 0;
            //显示全部考试成绩
            ds = objListService.GetAllScoreList();
            this.dgvScoreList.ItemsSource = ds.Tables[0].DefaultView;
            //禁止列排序，保证列标题居中
            foreach (DataGridColumn item in this.dgvScoreList.Columns)
            {
                item.CanUserSort = false;
            }
        }

        //根据班级名称动态筛选
        private void cboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ds == null) return;
            var a = this.cboClass.SelectedItem as StudentClass;
            this.ds.Tables[0].DefaultView.RowFilter = "ClassName='" + a.ClassName + "'";
        }
        //显示全部成绩
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            this.ds.Tables[0].DefaultView.RowFilter = "ClassName like '%%'";
        }
        //根据C#成绩动态筛选
        private void txtScore_TextChanged(object sender, EventArgs e)
        {
            if (this.txtScore.Text.Trim().Length == 0) return;
            if (!Common.DataValidate.IsInteger(this.txtScore.Text.Trim())) return;
            else
            {
                this.ds.Tables[0].DefaultView.RowFilter = "CSharp>" + this.txtScore.Text.Trim();
            }
        }

        //private void dgvScoreList_RowPostPaint(object sender, DataGridRowPostPaintEventArgs e)
        //{
        //    Common.DataGridViewStyle.DgvRowPostPaint(this.dgvScoreList, e);
        //}

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visibility=Visibility.Hidden;
        }
    }
}
