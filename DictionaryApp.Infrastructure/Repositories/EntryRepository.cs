using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Interfaces;
using DictionaryApp.Domain.Exceptions;
using DictionaryApp.Domain.Models;
using DictionaryApp.Infrastructure.Data;

namespace DictionaryApp.Infrastructure.Repositories;

public class EntryRepository : IEntryRepository
{
    private readonly JsonSet<Entry> _entrySet;

    public EntryRepository()
    {
        var jsonPath = Path.Combine(AppConfig.RoamingPath, AppConfig.JsonFileName);
        _entrySet = new JsonSet<Entry>(jsonPath);
    }

    public async Task<IEnumerable<Entry>> GetEntries()
    {
        var allEntries = await _entrySet.GetDataAsync();
        return allEntries.OrderBy(e => e.Word);
    }

    public async Task<IEnumerable<Entry>> GetEntries(string search)
    {
        var trimSearch = search.Trim().ToLower();
        var allEntries = await _entrySet.GetDataAsync();
        var entries = allEntries.Where(x =>
        {
            var wordEquality = x.Word.Contains(trimSearch, StringComparison.OrdinalIgnoreCase);
            var translateEquality = x.Translate.Any(t => t.Contains(trimSearch, StringComparison.OrdinalIgnoreCase));
            return wordEquality || translateEquality;
        });
        return entries;
    }

    public async Task<Entry?> GetEntry(string word)
    {
        var trimWord = word.Trim().ToLower();
        var allEntries = await _entrySet.GetDataAsync();
        var foundEntry = allEntries.FirstOrDefault(x =>
        {
            var wordEquality = x.Word.Equals(trimWord, StringComparison.OrdinalIgnoreCase);
            var translateEquality = x.Translate.Any(t => t.Equals(trimWord, StringComparison.OrdinalIgnoreCase));
            return wordEquality || translateEquality;
        });
        return foundEntry;
    }

    public async Task<Entry> CreateEntry(CreateEntryDto entryDto)
    {
        var allEntries = await _entrySet.GetDataAsync();
        var entries = allEntries.ToList();
        var newId = entries.Count == 0 ? 1 : entries.Max(x => x.Id) + 1;
        var createdEntry = new Entry()
        {
            Id = newId,
            Word = entryDto.Word,
            Translate = entryDto.Translate
        };
        entries.Add(createdEntry);
        await _entrySet.SetDataAsync(entries);
        return createdEntry;
    }

    public async Task<Entry> UpdateEntry(EntryDto entryDto)
    {
        var allEntries = await _entrySet.GetDataAsync();
        var entries = allEntries.ToList();
        var foundEntryIndex = entries.FindIndex(x => x.Id == entryDto.Id);
        if (foundEntryIndex == -1) throw new NotFoundException("Entry not found");
        var updatedEntry = new Entry()
        {
            Id = entryDto.Id,
            Word = entryDto.Word.ToLower(),
            Translate = entryDto.Translate.Select(x => x.ToLower()).ToArray()
        };
        entries[foundEntryIndex] = updatedEntry;
        await _entrySet.SetDataAsync(entries);
        return updatedEntry;
    }

    public async Task DeleteEntry(int id)
    {
        var allEntries = await _entrySet.GetDataAsync();
        var entries = allEntries.ToList();
        var foundEntryIndex = entries.FindIndex(x => x.Id == id);
        if (foundEntryIndex == -1) throw new NotFoundException("Entry not found");
        entries.RemoveAt(foundEntryIndex);
        await _entrySet.SetDataAsync(entries);
    }
}