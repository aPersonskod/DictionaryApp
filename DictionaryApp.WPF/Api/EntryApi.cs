using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Interfaces;
using DictionaryApp.WPF.Interfaces.Services;

namespace DictionaryApp.WPF.Api;

public class EntryApi(IEntryService entryService, IMessageService messageService)
{
    public async Task<IEnumerable<EntryDto>> GetEntriesAsync(string? search = null)
    {
        try
        {
            return await entryService.GetEntries(search);
        }
        catch (Exception e)
        {
            messageService.Error(e, "GetEntries error");
            return new List<EntryDto>();
        }
    }

    public async Task<EntryDto?> GetEntryByWordAsync(string word)
    {
        try
        {
            return await entryService.GetEntry(word);
        }
        catch (Exception e)
        {
            messageService.Error(e, "GetEntry error");
            return null;
        }
    }

    public async Task CreateEntriesAsync(IEnumerable<CreateEntryDto> entryDtos)
    {
        try
        {
            await entryService.CreateEntries(entryDtos);
        }
        catch (Exception e)
        {
            messageService.Error(e, "CreateEntries error");
        }
    }

    public async Task CreateEntryAsync(CreateEntryDto entryDto)
    {
        try
        {
            await entryService.CreateEntry(entryDto);
        }
        catch (Exception e)
        {
            messageService.Error(e, "CreateEntry error");
        }
    }

    public async Task DeleteEntryAsync(int entryId)
    {
        try
        {
            await entryService.DeleteEntry(entryId);
        }
        catch (Exception e)
        {
            messageService.Error(e, "CreateEntry error");
        }
    }

    public async Task UpdateEntryAsync(EntryDto entryDto)
    {
        try
        {
            await entryService.UpdateEntry(entryDto);
        }
        catch (Exception e)
        {
            messageService.Error(e, "CreateEntry error");
        }
    }
}