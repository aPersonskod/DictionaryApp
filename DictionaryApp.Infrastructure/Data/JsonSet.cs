using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace DictionaryApp.Infrastructure.Data;

public class JsonSet<T>(string jsonPath)
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
        WriteIndented = true
    };
    
    public async Task<IEnumerable<T>> GetDataAsync()
    {
        await using var jsonStream = new FileStream(jsonPath, FileMode.OpenOrCreate, FileAccess.Read);
        return await JsonSerializer.DeserializeAsync<IEnumerable<T>>(jsonStream, _jsonSerializerOptions) 
               ?? throw new ApplicationException("Data from file is null");
    }

    public async Task SetDataAsync(IEnumerable<T> data)
    {
        await using var jsonStream = new FileStream(jsonPath, FileMode.OpenOrCreate, FileAccess.Write);
        await JsonSerializer.SerializeAsync(jsonStream, data, _jsonSerializerOptions);
    }
}