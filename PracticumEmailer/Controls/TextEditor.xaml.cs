using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using WpfRichText.Ex.XamlToHtmlParser;

namespace PracticumEmailer.Controls
{
    public partial class TextEditor : UserControl
    {
        private readonly ObservableCollection<string> _collectionView;
        private string _currentFile;

        public TextEditor()
        {
            InitializeComponent();

            _collectionView = new ObservableCollection<string>(Directory.EnumerateFiles(@".\", "*.html"));
            editor.Text = HtmlToXamlConverter.ConvertHtmlToXaml(File.ReadAllText(_collectionView[0]), false);

            _currentFile = _collectionView[0];

            cmbFiles.ItemsSource = _collectionView;
        }

        private void cmbFiles_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            editor.Text = HtmlToXamlConverter.ConvertHtmlToXaml(File.ReadAllText(e.AddedItems[0].ToString()), false);

            _currentFile = e.AddedItems[0].ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string html = HtmlFromXamlConverter.ConvertXamlToHtml(editor.Text, false);
            File.WriteAllText(_currentFile, html);
        }
    }
}
