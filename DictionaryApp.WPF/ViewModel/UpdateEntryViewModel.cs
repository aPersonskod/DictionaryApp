using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Interfaces;
using DictionaryApp.WPF.Api;
using DictionaryApp.WPF.Interfaces;
using DictionaryApp.WPF.Interfaces.Services;

namespace DictionaryApp.WPF.ViewModel;

public class UpdateEntryViewModel : ObservableObject, IEntryModelObject, IInitingObject
{
    private readonly INavigationService _navigationService;
    private readonly EntryApi _entryApi;

    public UpdateEntryViewModel(INavigationService navigationService, IEntryService entryService)
    {
        _navigationService = navigationService;
        _entryApi = new EntryApi(entryService);
        CmdGoWords = new AsyncRelayCommand(async () => await GoWordsHandler());
        CmdUpdateWord = new AsyncRelayCommand(async () => await UpdateWordHandler());
    }
    
    public ICommand CmdGoWords { get; }
    private async Task GoWordsHandler()
    {
        await _navigationService.NavigateTo<WordsViewModel>();
    }

    public ICommand CmdUpdateWord { get; }
    private async Task UpdateWordHandler()
    {
        await _entryApi.UpdateEntryAsync(Entry);
        CmdGoWords.Execute(null);
    }

    private string _wordText;
    public string WordText
    {
        get => _wordText;
        set
        {
            Entry.Word = value;
            SetProperty(ref _wordText, value);
        }
    }

    private string _translateText;
    public string TranslateText
    {
        get => _translateText;
        set
        {
            Entry.Translate = value.Split(", ").Select(x => x.Trim()).ToArray();
            SetProperty(ref _translateText, value);
        }
    }

    public EntryDto Entry { get; } = new EntryDto();

    public void SetWord(EntryDto entry)
    {
        Entry.Id = entry.Id;
        WordText = entry.Word;
        TranslateText = string.Join(", ", entry.Translate);
    }

    public Task InitAsync()
    {
        return Task.CompletedTask;
    }
}