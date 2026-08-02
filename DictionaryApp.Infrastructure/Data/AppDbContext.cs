using System.Text.Json;
using DictionaryApp.Domain.Models;

namespace DictionaryApp.Infrastructure.Data;

public class AppDbContext
{
    private const string JsonFile = AppConfig.JsonFileName;
    private readonly string _jsonPath;
    public AppDbContext()
    {
        if(!Directory.Exists(AppConfig.RoamingPath)) Directory.CreateDirectory(AppConfig.RoamingPath);
        _jsonPath = Path.Combine(AppConfig.RoamingPath, JsonFile);
        Entries = new JsonDbSet<Entry>(_jsonPath);
        var words = GetWords();
        foreach (var wordModel in words)
        {
            Entries.Add(wordModel);
        }
    }
    
    public JsonDbSet<Entry> Entries { get; }

    private IEnumerable<Entry> GetWords()
    {
        if (!File.Exists(_jsonPath))
        {
            using (var fs = File.Create(_jsonPath))
            {
            }
        }
        var jsonData = File.ReadAllText(_jsonPath);
        return string.IsNullOrEmpty(jsonData) ? [] : JsonSerializer.Deserialize<IEnumerable<Entry>>(jsonData)!;
    }
}