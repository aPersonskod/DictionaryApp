using System.IO;
using System.Text.Json;
using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Mappings;
using DictionaryApp.Domain.Models;
using DictionaryApp.Infrastructure.Data;
using DictionaryApp.WPF.Extensions;
using DictionaryApp.WPF.Interfaces.Services;
//using Microsoft.Win32;

namespace DictionaryApp.WPF.Services;

public class FileService(IMessageService messageService) : IFileService
{
    public IEnumerable<EntryDto> ImportTxt()
    {
        try
        {
            var filePath = FileFilter.Txt.GetFileDialogPath();
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
            messageService.Error(e, "Import txt error");
            return new List<EntryDto>();
        }
    }

    public IEnumerable<EntryDto> ImportJson()
    {
        try
        {
            var filePath = FileFilter.Json.GetFileDialogPath();
            if(string.IsNullOrEmpty(filePath)) return new List<EntryDto>();
            var jsonData = File.ReadAllText(filePath);
            return string.IsNullOrEmpty(jsonData) 
                ? [] 
                : JsonSerializer.Deserialize<IEnumerable<Entry>>(jsonData)!.Select(x => x.ToDto()!);
        }
        catch (Exception e)
        {
            messageService.Error(e, "Import json error");
            return new List<EntryDto>();
        }
    }

    public void ExportJson()
    {
        try
        {
            AppExtensions.ExportToJson();
        }
        catch (Exception ex)
        {
            messageService.Error(ex, "Error saving file");
        }
    }
}