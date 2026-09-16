namespace DictionaryApp.Application.Interfaces;

public interface ITranslateService
{
    Task<string> GetTranslations(string word);
}