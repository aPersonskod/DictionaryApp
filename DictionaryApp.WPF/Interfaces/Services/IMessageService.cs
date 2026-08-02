namespace DictionaryApp.WPF.Interfaces.Services;

public interface IMessageService
{
    void Info(string message);
    void Error(Exception ex, string title = "Error");
}