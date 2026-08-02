using System.Collections.ObjectModel;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace DictionaryApp.Infrastructure.Data;

public class JsonDbSet<T>(string jsonPath) : Collection<T>
{
    protected override void InsertItem(int index, T item)
    {
        base.InsertItem(index, item);
        UpdateJsonFile();
    }

    protected override void RemoveItem(int index)
    {
        base.RemoveItem(index);
        UpdateJsonFile();
    }

    protected override void SetItem(int index, T item)
    {
        base.SetItem(index, item);
        UpdateJsonFile();
    }
    
    private void UpdateJsonFile()
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true
        };
        var jsonString = JsonSerializer.Serialize(Items, options);
        File.WriteAllText(jsonPath, jsonString);
    }
}