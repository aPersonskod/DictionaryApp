using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Interfaces;
using DictionaryApp.WPF.Api;
using DictionaryApp.WPF.Interfaces;
using DictionaryApp.WPF.Interfaces.Services;

namespace DictionaryApp.WPF.ViewModel;

public class AddEntryViewModel : ObservableObject, IEntryModelObject, IInitingObject
{
    private readonly INavigationService _navigationService;
    private readonly IFileService _fileService;
    private readonly EntryApi _entryApi;

    public AddEntryViewModel(INavigationService navigationService, IFileService fileService, IEntryService entryService)
    {
        _navigationService = navigationService;
        _fileService = fileService;
        _entryApi = new EntryApi(entryService);
        CmdGoWords = new AsyncRelayCommand(async () => await _navigationService.NavigateTo<WordsViewModel>());
        CmdAddWord = new AsyncRelayCommand(async () => await AddWordHandler());
        CmdImportTxt = new AsyncRelayCommand(async () => await ImportTxtHandler());
        CmdImportJson = new AsyncRelayCommand(async () => await ImportJsonHandler());
    }
    
    public ICommand CmdGoWords { get; }
    public ICommand CmdAddWord { get; }
    private async Task AddWordHandler()
    {
        if(string.IsNullOrEmpty(EntryWord) || string.IsNullOrWhiteSpace(EntryTranslate)) return;
        var createEntryDto = new CreateEntryDto()
        {
            Word = Entry.Word,
            Translate = Entry.Translate
        };
        await _entryApi.CreateEntryAsync(createEntryDto);
        CmdGoWords.Execute(null);
    }
    public ICommand CmdImportTxt { get; }
    private async Task ImportTxtHandler()
    {
        var words = _fileService.ImportTxt();
        await _entryApi.CreateEntriesAsync(words.Select(x => new CreateEntryDto()
        {
            Word = x.Word,
            Translate = x.Translate
        }));
        CmdGoWords.Execute(null);
    }
    public ICommand CmdImportJson { get; }
    private async Task ImportJsonHandler()
    {
        var words = _fileService.ImportJson();
        await _entryApi.CreateEntriesAsync(words.Select(x => new CreateEntryDto()
        {
            Word = x.Word,
            Translate = x.Translate
        }));
        CmdGoWords.Execute(null);
    }
    public EntryDto Entry { get; } = new EntryDto();
    private string _entryWord;
    public string EntryWord
    {
        get => _entryWord;
        set
        {
            Entry.Word = value;
            SetProperty(ref _entryWord, value);
        }
    }

    private string _entryTranslate;
    public string EntryTranslate
    {
        get => _entryTranslate;
        set
        {
            Entry.Translate = value.Split(", ").Select(x => x.Trim()).ToArray();
            SetProperty(ref _entryTranslate, value);
        }
    }
    
    public void SetWord(EntryDto entry)
    {
        EntryWord = entry.Word;
    }

    public Task InitAsync()
    {
        return Task.CompletedTask;
    }
}