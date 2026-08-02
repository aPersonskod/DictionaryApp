using DictionaryApp.Application.Dtos;

namespace DictionaryApp.WPF.Interfaces.Services;

public interface IFileService
{
    IEnumerable<EntryDto> ImportTxt();
    IEnumerable<EntryDto> ImportJson();
    void ExportJson();
}