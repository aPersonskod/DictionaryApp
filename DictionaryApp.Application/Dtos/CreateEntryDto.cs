namespace DictionaryApp.Application.Dtos;

public class CreateEntryDto
{
    public string Word { get; set; }
    public string[] Translate { get; set; }
}