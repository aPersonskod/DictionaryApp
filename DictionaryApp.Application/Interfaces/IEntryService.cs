using DictionaryApp.Application.Dtos;

namespace DictionaryApp.Application.Interfaces;

public interface IEntryService
{
    Task<IEnumerable<EntryDto>> GetEntries(string? search);
    Task<EntryDto?> GetEntry(string word);
    
    Task CreateEntries(IEnumerable<CreateEntryDto> entryDtos);
    Task<EntryDto> CreateEntry(CreateEntryDto entryDto);
    Task DeleteEntry(int entryId);
    Task UpdateEntry(EntryDto entryDto);
}