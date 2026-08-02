using System.IO;
using System.Text.Json;
using System.Windows;
using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Mappings;
using DictionaryApp.Domain.Models;
using DictionaryApp.WPF.Extensions;
using DictionaryApp.WPF.Interfaces.Services;

namespace DictionaryApp.WPF.Services;

public class FileService : IFileService
{
    public IEnumerable<EntryDto> ImportTxt()
    {
        try
        {
            var filePath = AppExtensions.GetFilePath(FileFilter.Txt);
            if(string.IsNullOrEmpty(filePath)) return new List<EntryDto>();
            var lines = File.ReadAllLines(filePath);
            var words = new List<EntryDto>();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var firstSplit = line.Split("-");
                var word = firstSplit[0].Trim();
                var translationSplit = firstSplit[1].Split(",");
                var translate = translationSplit.Length > 1 ? translationSplit : [translationSplit[0]];
                words.Add(new EntryDto(){Word = word, Translate = translate});
            }
            return words;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Import txt error");
            return new List<EntryDto>();
        }
    }

    public IEnumerable<EntryDto> ImportJson()
    {
        try
        {
            var filePath = AppExtensions.GetFilePath(FileFilter.Json);
            if(string.IsNullOrEmpty(filePath)) return new List<EntryDto>();
            var jsonData = File.ReadAllText(filePath);
            return string.IsNullOrEmpty(jsonData) 
                ? [] 
                : JsonSerializer.Deserialize<IEnumerable<Entry>>(jsonData)!.Select(x => x.ToDto());
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Import json error");
            return new List<EntryDto>();
        }
    }

    public void ExportJson() => AppExtensions.ExportJsonFile();
}