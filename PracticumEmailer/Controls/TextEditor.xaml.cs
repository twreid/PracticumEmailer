using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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

            _currentFile = _collectionView[0];

            cmbFiles.ItemsSource = _collectionView;
            cmbFiles.SelectedIndex = 0;

            Editor.DocumentReady += new RoutedEventHandler(OnDocumentReady);
            btnSave.Click += new RoutedEventHandler(OnSaveClick);
            cmbFiles.SelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            _currentFile = selectionChangedEventArgs.AddedItems[0].ToString();
            Editor.ContentHtml = File.ReadAllText(_currentFile);
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(_currentFile, Editor.ContentHtml);
        }

        private void OnDocumentReady(object sender, RoutedEventArgs routedEventArgs)
        {
            Editor.ContentHtml = File.ReadAllText(_currentFile);
        }
    }
}
