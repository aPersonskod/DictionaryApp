using System.Windows;
using System.Windows.Controls;

namespace DictionaryDesktopApp.View;

public partial class AddWordControl : UserControl
{
    public AddWordControl()
    {
        InitializeComponent();
        Loaded += WindowLoaded;
    }

    private void WindowLoaded(object sender, RoutedEventArgs e) {
        if (TranslateTextBox.Text.Length == 0)
        {
            TranslateTextBox.Focus();
        }

        if (WordTextBox.Text.Length == 0)
        {
            WordTextBox.Focus();
        }
    }
}