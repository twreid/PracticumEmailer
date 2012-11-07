using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace PracticumEmailer
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Microsoft.Win32.OpenFileDialog ofd;
        private bool isFileSelected = false;
        
        public MainWindow()
        {
            InitializeComponent();
            init();
            
        }

        private void init()
        {   //Setup Open File Dialog options
            ofd = new Microsoft.Win32.OpenFileDialog
                {Filter = "Comma Separated Values (.csv)|*.csv", Title = "Select the Data File."};

            //Check and set default values from last run
            if (Properties.Settings.Default.DataFile == String.Empty)
            {
                txtFile.Text = string.Empty;
                this.isFileSelected = false;
            }
            else
            {
                txtFile.Text = Properties.Settings.Default.DataFile;
                if (File.Exists(txtFile.Text))
                {
                    this.isFileSelected = true;
                }
                else
                {
                    this.isFileSelected = false;
                    txtFile.Text = String.Empty;
                }
                
            }

            if (Properties.Settings.Default.CutOffDate == null)
            {
                calCutOff.DisplayDate = DateTime.Now;
                calCutOff.SelectedDate = DateTime.Now;
            }
            else
            {
                calCutOff.DisplayDate = Properties.Settings.Default.CutOffDate;
                calCutOff.SelectedDate = Properties.Settings.Default.CutOffDate;
            }

            btnStart.IsEnabled = isFileSelected;
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e){
            
            if (true == ofd.ShowDialog())
            {
                if (File.Exists(ofd.FileName))
                {
                    txtFile.Text = ofd.FileName;
                    Properties.Settings.Default.DataFile = ofd.FileName;
                    Properties.Settings.Default.Save();
                    isFileSelected = true;
                }
            }
            else
            {
                txtFile.Text = "No Valid File Selected.";
                isFileSelected = false;
            }
            btnStart.IsEnabled = isFileSelected;
            
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Parser p = new Parser(txtFile.Text, calCutOff.SelectedDate.Value, chkTest.IsChecked);
                p.StartParse();
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(ex.Message, "Error Opening CSV File");
                return;
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBox.Show("Must Select a Valid date.", "Invalid Date: " + ex.Message);
                return;
            }
            
            
        }

        private void editCourses_Click(object sender, RoutedEventArgs e)
        {
            EditClearances s = new EditClearances();
            s.ShowDialog();
        }

        private void editMessages_Click(object sender, RoutedEventArgs e)
        {
            EditMessages s = new EditMessages();
            s.ShowDialog();
        }



    }
}
