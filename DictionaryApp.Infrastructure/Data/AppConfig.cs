namespace DictionaryApp.Infrastructure.Data;

public class AppConfig
{
    public static readonly string RoamingPath = 
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyDictionaryTest");
        //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyDictionary");
    public const string JsonFileName = "words.json";
}