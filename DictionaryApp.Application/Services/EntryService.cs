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
        foreach (var requestEntryDto in entryDtos)
        {
            await entryRepository.CreateEntry(requestEntryDto);
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