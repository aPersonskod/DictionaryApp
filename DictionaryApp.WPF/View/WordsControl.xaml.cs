using System.Windows;
using System.Windows.Controls;

namespace DictionaryDesktopApp.View;

public partial class WordsControl : UserControl
{
    public WordsControl()
    {
        InitializeComponent();
        Loaded += WindowLoaded;
    }

    private void WindowLoaded(object sender, RoutedEventArgs e)
    {
        SearchTextBox.Focus();
    }
}