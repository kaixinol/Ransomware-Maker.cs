using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;


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
        private static string GetCscCompiler(string folder)
        {
            List<string> compilers = new List<string>();
            foreach (var entry in new DirectoryInfo(folder).EnumerateDirectories())
            {
                if (entry.Exists)
                {
                    compilers.Add(entry.FullName);
                }
            }

            Dictionary<int, string> temp = new Dictionary<int, string>();
            foreach (var i in compilers)
            {
                string[] parts = i.Split('\\');
                string version = parts[4].Substring(1).Replace(".", "");
                if (parts[4].Contains("v"))
                {
                    temp[Convert.ToInt32(version)] = parts[4];
                }
            }

            int maxKey = temp.Keys.Max();
            string cscPath = "C:\\Windows\\Microsoft.NET\\Framework\\" + temp[maxKey] + "\\csc.exe";
            return cscPath;
        }
        private void rdb_cpp_Checked(object sender, RoutedEventArgs e)
        {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Executable Files|gcc.exe";
                openFileDialog.Title = "Compiler for MinGW-w64 GCC that supports c++17";

            if (openFileDialog.ShowDialog()==true)
            {
                string selectedFileName = openFileDialog.FileName;
                lbl_compiler.Content = selectedFileName;
            }
            else
            {
                MessageBox.Show("Please select the correct compiler file!", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                rdb_cpp.IsChecked = false;
                rdb_cs.IsChecked = true;
            }
        }
        private void lbl_open_source_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //调用系统默认的浏览器 
            Process.Start(new ProcessStartInfo { FileName = "https://github.com/kaixinol/Ransomware-Maker.cs", UseShellExecute = true });
        }


        private void rdb_cs_Loaded(object sender, RoutedEventArgs e)
        {
            rdb_cs.IsChecked = true;
        }
        private void rdb_cs_Checked(object sender, RoutedEventArgs e)
        {
            lbl_compiler.Content = GetCscCompiler("C:\\Windows\\Microsoft.NET\\Framework");
        }

        private void lbl_compiler_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("explorer.exe", $"/select,\"{lbl_compiler.Content}\"");
        }
    }


}
