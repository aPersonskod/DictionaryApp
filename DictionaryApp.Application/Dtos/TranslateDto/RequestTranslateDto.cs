namespace DictionaryApp.Application.Dtos.TranslateDto;

public class RequestTranslateDto
{
    public RequestSource source { get; set; }
    public RequestTarget target { get; set; }

    public static RequestTranslateDto Create(string word) =>
        new()
        {
            source = new RequestSource() { text = word },
            target = new RequestTarget()
        };
}

public class RequestSource
{
    public string dialect { get; } = "en-US";
    public string text { get; set; }
    public List<string> with { get; } = ["synonyms"];
}

public class RequestTarget
{
    public string dialect { get; } = "ru";
}