using System.IO;
using DictionaryApp.Infrastructure.Data;
using Microsoft.Win32;

namespace DictionaryApp.WPF.Extensions;

public static class AppExtensions
{
    public static string GetFilter(this FileFilter filter) => filter switch
    {
        FileFilter.Txt => "Text files (*.txt)|*.txt|All files (*.*)|*.*",
        FileFilter.Json => "JSON files (*.json)|*.json|All files (*.*)|*.*",
        _ => throw new ArgumentOutOfRangeException(nameof(filter), filter, null)
    };
    
    public static string? GetFileDialogPath(this FileFilter filter)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = filter.GetFilter(),
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        };
        return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
    }

    public static void ExportToJson()
    {
        var saveFileDialog = new SaveFileDialog();
        // Configure dialog settings
        saveFileDialog.Filter = FileFilter.Json.GetFilter();
        saveFileDialog.FilterIndex = 1;
        saveFileDialog.DefaultExt = "json";
        saveFileDialog.Title = "Save Data as Json File";
        saveFileDialog.FileName = "words";
        
        // Show the dialog and check if the user clicked 'OK'
        if (!(saveFileDialog.ShowDialog() ?? false)) return;
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
}

public enum FileFilter
{
    Txt,
    Json
}