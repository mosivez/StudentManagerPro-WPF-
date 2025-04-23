using Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace StudentManagerWPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static Models.Admin objCurentAdmin = null;

        //声明用户信息的全局变量
        public static Admin currentAdmin = null;
    }
}
