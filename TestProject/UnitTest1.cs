using DictionaryApp.Infrastructure.Repositories;

namespace TestProject;

public class UnitTest1
{
    [Fact]
    public async Task Test_TranslateApi()
    {
        var translateRepo = new TranslateRepository();
        var word = "bearer";
        var translate = await translateRepo.GetTranslations(word);
        Assert.Equal("носитель", translate);
    }
}