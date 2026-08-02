using System.Windows;
using System.Windows.Controls;

namespace DictionaryDesktopApp.View;

public partial class UpdateWord : UserControl
{
    public UpdateWord()
    {
        InitializeComponent();
        Loaded += WindowLoaded;
    }

    private void WindowLoaded(object sender, RoutedEventArgs e)
    {
        WordTextBox.Focus();
        WordTextBox.CaretIndex = WordTextBox.Text.Length;
    }
}