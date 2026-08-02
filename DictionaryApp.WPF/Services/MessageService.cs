using System.Windows;
using DictionaryApp.WPF.Interfaces.Services;

namespace DictionaryApp.WPF.Services;

public class MessageService : IMessageService
{
    public void Info(string message)
    {
        MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public void Error(Exception ex, string title = "Error")
    {
        MessageBox.Show($"{ex.Message}", title, MessageBoxButton.OK, MessageBoxImage.Error);
    }
}