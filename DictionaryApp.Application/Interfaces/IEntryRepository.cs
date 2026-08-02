using DictionaryApp.Application.Dtos;
using DictionaryApp.Domain.Models;

namespace DictionaryApp.Application.Interfaces;

public interface IEntryRepository
{
    public Task<IEnumerable<Entry>> GetEntries();
    public Task<IEnumerable<Entry>> GetEntries(string search);
    public Task<Entry?> GetEntry(string word);
    public Task<Entry> CreateEntry(CreateEntryDto entryDto);
    public Task<Entry> UpdateEntry(EntryDto entryDto);
    public Task DeleteEntry(int id);
}