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
    /// ScoreManagePage.xaml 的交互逻辑
    /// </summary>
    public partial class ScoreManagePage : UserControl
    {
        private ScoreListService objScoreService = new ScoreListService();
        //private List<StudentClass> list = null;
        private DataSet ds = null;//保存全部查询结果的数据集
        public ScoreManagePage()
        {
            InitializeComponent();

            //断开事件
            this.cboClass.SelectionChanged -= cboClass_SelectionChanged;
            //初始化班级下拉框
            this.cboClass.ItemsSource = new StudentClassService().GetAllClasses();
            this.cboClass.DisplayMemberPath = "ClassName";//设置下拉框的显示文本
            this.cboClass.SelectedValuePath = "ClassId";//设置下拉框显示文本对应的value
            //this.cboClass.DataSource = new StudentClassService().GetAllClasses();
            //this.cboClass.DisplayMember = "ClassName";//设置下拉框的显示文本
            //this.cboClass.ValueMember = "ClassId";//设置下拉框显示文本对应的value 
            this.cboClass.SelectedIndex = -1;
            //加载全部考试成绩
            ds = objScoreService.GetAllScoreList();
            this.dgvScoreList.ItemsSource = ds.Tables[0].DefaultView;
            //禁止列排序，保证列标题居中
            foreach (DataGridColumn item in this.dgvScoreList.Columns)
            //挂接事件
            this.cboClass.SelectionChanged += cboClass_SelectionChanged;
        }

        //根据班级查询      
        void cboClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cboClass.SelectedIndex == -1)
            {
                MessageBox.Show("请首先选择要查询的班级", "查询提示");
                return;
            }
            var a = this.cboClass.SelectedItem as StudentClass;
            this.dgvScoreList.AutoGenerateColumns = false; 
            this.ds.Tables[0].DefaultView.RowFilter = "ClassName='" + a.ClassName + "'";
            //new Common.DataGridViewStyle().DgvStyle1(this.dgvScoreList);

            //同步显示班级考试信息
            this.gbStat.Header = "[" + a.ClassName + "]考试成绩统计";
            Dictionary<string, string> dic =
                objScoreService.GetScoreInfoByClassId(this.cboClass.SelectedValue.ToString());
            this.lblAttendCount.Content = dic["stuCount"];
            this.lblCSharpAvg.Content = dic["avgCSharp"];
            this.lblDBAvg.Content = dic["avgDB"];
            this.lblCount.Content = dic["absentCount"];
            //显示缺考人员姓名
            List<string> list =
                objScoreService.GetAbsentListByClassId(this.cboClass.SelectedValue.ToString());
            this.lblList.Items.Clear();
            if (list.Count == 0) this.lblList.Items.Add("没有缺考");
            else
            {
                for (int x = 1; x <= list.Count;x++ )
                {
                    lblList.Items.Add(list[x-1]);
                }
                    
                //lblList.ItemsSource = list;
            }

        }
        //关闭
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visibility=Visibility.Hidden;
        }
        //统计全校考试成绩
        private void btnStat_Click(object sender, EventArgs e)
        {
            this.gbStat.Header = "全校考试成绩统计";
            this.dgvScoreList.AutoGenerateColumns = false;
            ds = objScoreService.GetAllScoreList();
            this.dgvScoreList.ItemsSource = ds.Tables[0].DefaultView;
            //new Common.DataGridViewStyle().DgvStyle1(this.dgvScoreList);
            //查询并显示成绩统计
            Dictionary<string, string> dic = objScoreService.GetScoreInfo();
            this.lblAttendCount.Content = dic["stuCount"];
            this.lblCSharpAvg.Content = dic["avgCSharp"];
            this.lblDBAvg.Content = dic["avgDB"];
            this.lblCount.Content = dic["absectCount"];
            //显示缺考人员姓名
            List<string> list = objScoreService.GetAbsentList();
            lblList.Items.Clear();
            for (int x = 1; x <= list.Count; x++)
            {
                lblList.Items.Add(list[x - 1]);
            }
        }

        //private void dgvScoreList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        //{
        //    Common.DataGridViewStyle.DgvRowPostPaint(this.dgvScoreList, e);
        //}



        ////选择框选择改变处理
        //private void dgvScoreList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        //{
        //    if (this.dgvScoreList.IsCurrentCellDirty) //首先判断单元格的值是否有未提交的更改
        //    {
        //        this.dgvScoreList.CommitEdit(DataGridViewDataErrorContexts.Commit);//提交修改

        //        //显示修改后的值（true/false）可以将修改后的状态更新到数据库等...
        //        string ss = this.dgvScoreList.CurrentCell.EditedFormattedValue.ToString();
        //        MessageBox.Show(ss);
        //    }
        //}
        ////解析组合属性
        //private void dgvScoreList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    if (e.ColumnIndex == 0 && e.Value is Student)
        //    {
        //        e.Value = (e.Value as Student).StudentId;
        //    }
        //    if (e.ColumnIndex == 1 && e.Value is Student)
        //    {
        //        e.Value = (e.Value as Student).StudentName;
        //    }
        //    if (e.ColumnIndex == 2 && e.Value is Student)
        //    {
        //        e.Value = (e.Value as Student).Gender;
        //    }
        //    if (e.ColumnIndex == 4 && e.Value is ScoreList)
        //    {
        //        e.Value = (e.Value as ScoreList).CSharp;
        //    }
        //    if (e.ColumnIndex == 5 && e.Value is ScoreList)
        //    {
        //        e.Value = (e.Value as ScoreList).SQLServerDB;
        //    }
        //}
    }
}
