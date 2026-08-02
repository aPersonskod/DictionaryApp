using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Interfaces;
using DictionaryApp.Application.Mappings;

namespace DictionaryApp.Application.Services;

public class EntryService(IEntryRepository entryRepository) : IEntryService
{
    public async Task<IEnumerable<EntryDto>> GetEntries(string? search)
    {
        var entries = string.IsNullOrWhiteSpace(search)
            ? await entryRepository.GetEntries()
            : await entryRepository.GetEntries(search);
        return entries.Select(x => x.ToDto());
    }

    public async Task<EntryDto?> GetEntry(string word)
    {
        var foundWord = await entryRepository.GetEntry(word);
        return foundWord.ToDto();
    }

    public async Task CreateEntries(IEnumerable<CreateEntryDto> entryDtos)
    {
        foreach (var entryDto in entryDtos)
        {
            var foundWord = await entryRepository.GetEntry(entryDto.Word.Trim());
            if (foundWord == null)
            {
                await entryRepository.CreateEntry(entryDto);
            }
            else
            {
                var translates = foundWord.Translate.ToList();
                translates.AddRange(entryDto.Translate);
                foundWord.Translate = translates.Select(x => x.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
                await entryRepository.UpdateEntry(new EntryDto()
                {
                    Id = foundWord.Id,
                    Word = foundWord.Word,
                    Translate = foundWord.Translate
                });
            }
        }
    }

    public async Task<EntryDto> CreateEntry(CreateEntryDto entryDto)
    {
        var createdEntry = await entryRepository.CreateEntry(entryDto);
        return createdEntry.ToDto();
    }

    public async Task DeleteEntry(int entryId)
    {
        await entryRepository.DeleteEntry(entryId);
    }

    public async Task UpdateEntry(EntryDto entryDto)
    {
        await entryRepository.UpdateEntry(entryDto);
    }
}