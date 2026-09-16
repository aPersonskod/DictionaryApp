using DictionaryApp.Application.Dtos.TranslateDto;
using DictionaryApp.Application.Interfaces;
using QueryHelper;

namespace DictionaryApp.Infrastructure.Repositories;

public class TranslateRepository : ITranslateRepository
{
    public async Task<string> GetTranslations(string word)
    {
        try
        {
            var requestDto = RequestTranslateDto.Create(word);
            var query = "https://web-api.itranslateapp.com/v3/texts/translate";
            var response = await query.PostQuery<ResponseTranslateDto, RequestTranslateDto>(requestDto);
            return response is { Success: true, Result: not null } 
                ? response.Result.target.text
                : string.Empty;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }
}