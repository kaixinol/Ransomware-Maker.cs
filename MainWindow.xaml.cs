using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
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

namespace Ransomware_Maker.cs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void lbl_open_source_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //调用系统默认的浏览器 
            Process.Start(new ProcessStartInfo { FileName = "https://github.com/kaixinol/Ransomware-Maker.cs", UseShellExecute = true });
        }
    }
}
