namespace DictionaryApp.Application.Dtos.TranslateDto;

public class ResponseTranslateDto
{
    public ResponseSource source { get; set; }
    public ResponseSource target { get; set; }
    public List<string> from_engines { get; set; }
}

public class ResponseSource
{
    public string dialect { get; set; }
    public string text { get; set; }
    public object transliteration { get; set; }
    public List<object> entities { get; set; }
    public List<ResponseToken> tokens { get; set; }
    public List<object> verbs { get; set; }
    public object meanings { get; set; }
}

public class ResponseToken
{
    public string text { get; set; }
    public string pos { get; set; }
    public string morphology { get; set; }
    public ResponseRangeInText range_in_text { get; set; }
}

public class ResponseRangeInText
{
    public int start { get; set; }
    public int end { get; set; }
    public object context { get; set; }
}