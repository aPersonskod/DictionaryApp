using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Interfaces;
using DictionaryApp.Domain.Exceptions;
using DictionaryApp.Domain.Models;
using DictionaryApp.Infrastructure.Data;

namespace DictionaryApp.Infrastructure.Repositories;

public class EntryRepository(AppDbContext context) : IEntryRepository
{
    public Task<IEnumerable<Entry>> GetEntries() 
        => Task.FromResult<IEnumerable<Entry>>(context.Entries);
    
    public Task<IEnumerable<Entry>> GetEntries(string search)
    {
        var trimSearch = search.Trim().ToLower();
        var entries = context.Entries.Where(x =>
        {
            var wordEquality = x.Word.Contains(trimSearch, StringComparison.OrdinalIgnoreCase);
            var translateEquality = x.Translate.Any(t => t.Contains(trimSearch, StringComparison.OrdinalIgnoreCase));
            return wordEquality || translateEquality;
        });
        return Task.FromResult(entries);
    }

    public Task<Entry?> GetEntry(string word)
    {
        var trimWord = word.Trim().ToLower();
        var foundEntry = context.Entries.FirstOrDefault(x =>
        {
            var wordEquality = x.Word.Equals(trimWord, StringComparison.OrdinalIgnoreCase);
            var translateEquality = x.Translate.Any(t => t.Equals(trimWord, StringComparison.OrdinalIgnoreCase));
            return wordEquality || translateEquality;
        });
        return Task.FromResult(foundEntry);
    }

    public async Task<Entry> CreateEntry(CreateEntryDto entryDto)
    {
        var entries = await GetEntries("");
        var entriesList = entries.ToList();
        var newId = entriesList.Count == 0 ? 1 : entriesList.Max(x => x.Id) + 1;
        var createdEntry = new Entry()
        {
            Id = newId,
            Word = entryDto.Word,
            Translate = entryDto.Translate
        };
        context.Entries.Add(createdEntry);
        return createdEntry;
    }

    public Task<Entry> UpdateEntry(EntryDto entryDto)
    {
        var allEntries = context.Entries.ToList();
        var foundEntryIndex = allEntries.FindIndex(x => x.Id == entryDto.Id);
        if (foundEntryIndex == -1) throw new NotFoundException("Entry not found");
        var updatedEntry = new Entry()
        {
            Id = entryDto.Id,
            Word = entryDto.Word,
            Translate = entryDto.Translate
        };
        context.Entries[foundEntryIndex] = updatedEntry;
        return Task.FromResult(updatedEntry);
    }

    public Task DeleteEntry(int id)
    {
        var allEntries = context.Entries.ToList();
        var foundEntryIndex = allEntries.FindIndex(x => x.Id == id);
        if (foundEntryIndex == -1) throw new NotFoundException("Entry not found");
        context.Entries.RemoveAt(foundEntryIndex);
        return Task.CompletedTask;
    }
}