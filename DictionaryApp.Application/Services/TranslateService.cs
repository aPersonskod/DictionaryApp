using DictionaryApp.Application.Interfaces;

namespace DictionaryApp.Application.Services;

public class TranslateService(ITranslateRepository translateRepository) : ITranslateService
{
    public async Task<string> GetTranslations(string word) => await translateRepository.GetTranslations(word);
}