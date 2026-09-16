namespace DictionaryApp.Application.Interfaces;

public interface ITranslateRepository
{
    Task<string> GetTranslations(string word);
}