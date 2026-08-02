using DictionaryApp.Application.Dtos;

namespace DictionaryApp.WPF.Interfaces;

public interface IEntryModelObject
{
    EntryDto Entry { get; }
    void SetWord(EntryDto entry);
}

public interface IInitingObject
{
    Task InitAsync();
}
