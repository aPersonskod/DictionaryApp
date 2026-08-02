namespace DictionaryApp.Infrastructure.Data;

public class AppConfig
{
    public static readonly string RoamingPath = 
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyDictionary");
        //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyDictionaryTest");
    public const string JsonFileName = "words.json";
}