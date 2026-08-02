using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Interfaces;

namespace DictionaryApp.WPF.Api;

public class EntryApi(IEntryService entryService)
{
    public async Task<IEnumerable<EntryDto>> GetEntriesAsync(string? search = null)
    {
        return await entryService.GetEntries(search);
    }

    public async Task<EntryDto?> GetEntryByWordAsync(string word)
    {
        return await entryService.GetEntry(word);
    }

    public async Task CreateEntriesAsync(IEnumerable<CreateEntryDto> entryDtos)
    {
        await entryService.CreateEntries(entryDtos);
    }

    public async Task<EntryDto> CreateEntryAsync(CreateEntryDto entryDto)
    {
        return await entryService.CreateEntry(entryDto);
    }

    public async Task DeleteEntryAsync(int entryId)
    {
        await entryService.DeleteEntry(entryId);
    }

    public async Task UpdateEntryAsync(EntryDto entryDto)
    {
        await entryService.UpdateEntry(entryDto);
    }
}