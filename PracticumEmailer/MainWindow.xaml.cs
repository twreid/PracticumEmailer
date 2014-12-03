using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using PracticumEmailer.Properties;

namespace PracticumEmailer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isFileSelected;
        private OpenFileDialog _ofd;

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            _ofd = new OpenFileDialog
            {Filter = "Comma Separated Values (.csv)|*.csv", Title = "Select the Data File."};

            if (Settings.Default.DataFile == String.Empty)
            {
                txtFile.Text = string.Empty;
                _isFileSelected = false;
            }
            else
            {
                txtFile.Text = Settings.Default.DataFile;
                if (File.Exists(txtFile.Text))
                {
                    _isFileSelected = true;
                }
                else
                {
                    _isFileSelected = false;
                    txtFile.Text = String.Empty;
                }
            }

            if (Settings.Default.CutOffDate == null)
            {
                calCutOff.DisplayDate = DateTime.Now;
                calCutOff.SelectedDate = DateTime.Now;
            }
            else
            {
                calCutOff.DisplayDate = Settings.Default.CutOffDate;
                calCutOff.SelectedDate = Settings.Default.CutOffDate;
            }

            btnStart.IsEnabled = _isFileSelected;
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (true == _ofd.ShowDialog())
            {
                if (File.Exists(_ofd.FileName))
                {
                    txtFile.Text = _ofd.FileName;
                    Settings.Default.DataFile = _ofd.FileName;
                    Settings.Default.Save();
                    _isFileSelected = true;
                }
            }
            else
            {
                txtFile.Text = "No Valid File Selected.";
                _isFileSelected = false;
            }
            btnStart.IsEnabled = _isFileSelected;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var p = new Parser(txtFile.Text, chkTest.IsChecked, null,
                    new StudentManager(null, calCutOff.SelectedDate.Value));
                p.StartParse();
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Error Opening CSV File");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Must Select a Valid date.", "Invalid Date: " + ex.Message);
            }
        }

        private void editCourses_Click(object sender, RoutedEventArgs e)
        {
            var s = new EditClearances();
            s.ShowDialog();
        }

        private void editMessages_Click(object sender, RoutedEventArgs e)
        {
            var s = new EditMessages();
            s.ShowDialog();
        }
    }
}