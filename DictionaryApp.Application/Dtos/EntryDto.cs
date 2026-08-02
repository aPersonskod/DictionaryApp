namespace DictionaryApp.Application.Dtos;

public class EntryDto
{
    public int Id { get; set; }
    public string Word { get; set; }
    public string[] Translate { get; set; }
}