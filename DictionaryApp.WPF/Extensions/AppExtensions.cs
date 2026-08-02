using System.IO;
using System.Windows;
using DictionaryApp.Application.Dtos;
using DictionaryApp.Domain.Models;
using DictionaryApp.Infrastructure.Data;
using Microsoft.Win32;

namespace DictionaryApp.WPF.Extensions;

public static class AppExtensions
{
    public static Entry TrimAll(this Entry entry)
    {
        return new Entry()
        {
            Id = entry.Id,
            Word = entry.Word.Trim(),
            Translate = entry.Translate.Select(x => x.Trim()).ToArray()
        };
    }
    
    public static EntryDto TrimAll(this EntryDto entryDto)
    {
        return new EntryDto()
        {
            Word = entryDto.Word.Trim(),
            Translate = entryDto.Translate.Select(x => x.Trim()).ToArray()
        };
    }
    
    public static string? GetFilePath(FileFilter filter)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = GetFilter(filter),
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        };
        return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
    }
    
    private static string GetFilter(this FileFilter filter) => filter switch
    {
        FileFilter.Txt => "Text files (*.txt)|*.txt|All files (*.*)|*.*",
        FileFilter.Json => "JSON files (*.json)|*.json|All files (*.*)|*.*",
        _ => throw new ArgumentOutOfRangeException(nameof(filter), filter, null)
    };

    public static void ExportJsonFile()
    {
        var saveFileDialog = new SaveFileDialog();
        // Configure dialog settings
        saveFileDialog.Filter = GetFilter(FileFilter.Json);
        saveFileDialog.FilterIndex = 1;
        saveFileDialog.DefaultExt = "json";
        saveFileDialog.Title = "Save Data as Json File";
        saveFileDialog.FileName = "words"; 

        // Show the dialog and check if the user clicked 'OK'
        if (!(saveFileDialog.ShowDialog() ?? false)) return;
        try
        {
            var sourceFile = Path.Combine(AppConfig.RoamingPath, AppConfig.JsonFileName);
            if (!File.Exists(saveFileDialog.FileName))
            {
                using (FileStream fs = File.Create(saveFileDialog.FileName))
                {
                    // File is created and stream is closed safely here
                }
            }
            File.Copy(sourceFile, saveFileDialog.FileName, true);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving file: {ex.Message}", "Error");
        }
    }
}

public enum FileFilter
{
    Txt,
    Json
}