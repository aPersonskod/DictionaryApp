using DictionaryApp.Application.Dtos;

namespace DictionaryApp.WPF.Interfaces;

public interface IEntryModelObject
{
    EntryDto Entry { get; }
    Task SetWord(EntryDto entry);
}