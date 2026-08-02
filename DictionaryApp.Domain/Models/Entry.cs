using System.Text.Json.Serialization;

namespace DictionaryApp.Domain.Models;

public class Entry
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("word")]
    public string Word { get; set; }
    [JsonPropertyName("translate")]
    public string[] Translate { get; set; }
}