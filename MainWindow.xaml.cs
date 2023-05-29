﻿using Microsoft.Win32;
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
        private static string GetCscCompiler()
        {
            string frameworkPath = "C:\\Windows\\Microsoft.NET\\Framework\\";
            List<string> compilers = new List<string>();
            foreach (var entry in new DirectoryInfo(frameworkPath).EnumerateDirectories())
            {
                if (entry.Name.Contains("v"))
                {
                    compilers.Add(entry.FullName);
                }
            }

            Dictionary<int, string> temp = new Dictionary<int, string>();
            foreach (var compiler in compilers)
            {
                string[] parts = compiler.Split('\\');
                int version = int.Parse(parts[4].Substring(1, 3).Replace(".", ""));
                temp[version] = parts[4];
            }

            Console.WriteLine(temp);
            string selectedCompiler = temp[temp.Keys.Max()];

            return frameworkPath + selectedCompiler + "\\csc.exe";
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
            lbl_compiler.Content = GetCscCompiler();
        }

        private void lbl_compiler_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("explorer.exe", $"/select,\"{lbl_compiler.Content}\"");
        }
    }


}
