using Newtonsoft.Json;
using PEFile;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Navigation;

namespace MOSRK_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                LoadSettings();
                CheckInstall();
                if (_settings is { StartInstantly: true })
                {
                    RunGame();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Start not possible or Exception occurs {e}  ", "MOSRK Launcher Error");
            }
        }

        private string _mapTexturesChosen = "vanilla";

        private void LoadSettings()
        {
            if (File.Exists(Cwd + "/MOSRK_Config.json"))
            {
                _settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Cwd + "/MOSRK_Config.json"));
                if (_settings is { PermanentArrows: true })
                {
                    permArrowCheck.IsChecked = true;
                }
                saved.Text = "";
            }
            else
            {
                _settings = new Settings();
                var json = JsonConvert.SerializeObject(_settings, Formatting.Indented);
                File.WriteAllText(Cwd + "/MOSRK_Config.json", json);
            }
        }

        private string _exeMed = "";
        private string _exeKingdoms = "";
        private Settings? _settings = new();

        private void CheckInstall()
        {
            var gameDir = Path.GetFullPath(Path.Combine(Cwd, @"..\..\"));
            _exeMed = gameDir + "/medieval2.exe";
            _exeKingdoms = gameDir + "/kingdoms.exe";
            if (!File.Exists(_exeMed) && (!File.Exists(_exeKingdoms)))
            {
                const string messageBoxText = "You have installed MOS: Reunited Kingdom into the wrong location, no game executables were found. Check your path to the mod folder.";
                const string caption = "Wrong installation!";
                const MessageBoxButton button = MessageBoxButton.OK;
                const MessageBoxImage icon = MessageBoxImage.Warning;

                MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                Application.Current.Shutdown();
            }
            if (File.Exists(_exeKingdoms))
            {
                laaapplied.Text = LargeAddressAware.IsLargeAddressAware(_exeKingdoms) ? "LAA applied" : "LAA not applied";
            }
            else
            {
                laaapplied.Text = LargeAddressAware.IsLargeAddressAware(_exeMed) ? "LAA applied" : "LAA not applied";
            }
        }

        private bool LaaWarning()
        {
            const string messageBoxText = "You have not applied LAA, you will experience many crashes. Do you want to apply it now?";
            const string caption = "LAA has not been applied";
            const MessageBoxButton button = MessageBoxButton.YesNoCancel;
            const MessageBoxImage icon = MessageBoxImage.Warning;

            var result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    SetLaa();
                    return true;
                case MessageBoxResult.No:
                    return true;
                case MessageBoxResult.Cancel:
                    return false;
                case MessageBoxResult.None:
                    return false;
                case MessageBoxResult.OK:
                    return false;
                default:
                    return false;
            }
        }

        private void SetLaa()
        {
            if (File.Exists(_exeMed))
            {
                LargeAddressAware.SetLargeAddressAware(_exeMed);
            }
            if (File.Exists(_exeKingdoms))
            {
                LargeAddressAware.SetLargeAddressAware(_exeKingdoms);
            }
        }

        private void RunGame()
        {
            var argument = "@" + Cwd + "\\TATW.cfg";
            var startGame = true;
            if (File.Exists(_exeKingdoms))
            {
                if (!LargeAddressAware.IsLargeAddressAware(_exeKingdoms))
                {
                    startGame = LaaWarning();
                }
                if (!startGame) return;
                //Some disk and pirated versions need a cmd launch method for some reason, don't remove
                string strCmdText = "/C cd ..\\..&start kingdoms.exe " + '"' + argument + '"';
                Process.Start("CMD.exe", strCmdText);
                Application.Current.Shutdown();
            }
            else if (File.Exists(_exeMed))
            {
                if (!LargeAddressAware.IsLargeAddressAware(_exeMed))
                {
                    startGame = LaaWarning();
                }
                if (!startGame) return;
                string strCmdText = "/C cd ..\\..&start medieval2.exe " + '"' + argument + '"';
                Process.Start("CMD.exe", strCmdText);
                Application.Current.Shutdown();
            }
            else
            {
                Application.Current.Shutdown();
            }
            if (!startGame) return;
            Application.Current.Shutdown();
        }

        private bool _permArrow;

        private static readonly string Cwd = Directory.GetCurrentDirectory();




        private void permArrowCheck_Checked(object sender, RoutedEventArgs e)
        {
            _permArrow = true;
            saved.Text = "Unsaved settings!";
        }

        private void permArrowCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            _permArrow = false;
            saved.Text = "Unsaved settings!";

        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            string sourceDir;
            var destinationDir = Cwd + "/data";
            if (_permArrow)
            {
                sourceDir = Cwd + "/extra/permArrow";
            }
            else
            {
                sourceDir = Cwd + "/extra/permArrowVanilla";
            }
            CopyFiles(sourceDir, destinationDir);
            if (_settings != null) _settings.PermanentArrows = _permArrow;
            
            var json = JsonConvert.SerializeObject(_settings, Formatting.Indented);
            File.WriteAllText(Cwd + "/MOSRK_Config.json", json);
            saved.Text = "Settings saved.";
        }


        private void runButton_Click(object sender, RoutedEventArgs e)
        {
            RunGame();
        }

        private static void CopyFiles(string sourceDir, string destinationDir)
        {
            if (!Directory.Exists(sourceDir) || !Directory.Exists(destinationDir)) return;
            var allFiles = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);
            foreach (var newPath in allFiles)
            {
                const bool overwriteFiles = true;
                File.Copy(newPath, newPath.Replace(sourceDir, destinationDir), overwriteFiles);
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // for .NET Core you need to add UseShellExecute = true
            // see https://learn.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
