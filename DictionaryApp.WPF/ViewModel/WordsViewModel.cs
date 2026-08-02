using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Interfaces;
using DictionaryApp.WPF.Api;
using DictionaryApp.WPF.Interfaces;
using DictionaryApp.WPF.Interfaces.Services;

namespace DictionaryApp.WPF.ViewModel;

public class WordsViewModel : ObservableObject, IInitingObject
{
    private readonly INavigationService _navigationService;
    private readonly EntryApi _entryApi;
    private readonly IFileService _fileService;

    public WordsViewModel(
        INavigationService navigationService,
        IFileService fileService,
        IEntryService entryService,
        IMessageService messageService)
    {
        _navigationService = navigationService;
        _fileService = fileService;
        _entryApi = new EntryApi(entryService, messageService);
        CmdGoAddWord = new AsyncRelayCommand(async () =>
        {
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var foundWord = await _entryApi.GetEntryByWordAsync(SearchText);
                if (foundWord != null)
                {
                    await _navigationService.NavigateTo<UpdateEntryViewModel>([foundWord]);
                    return;
                }
                var wordModel = new EntryDto()
                {
                    Word = SearchText
                };
                await _navigationService.NavigateTo<AddEntryViewModel>([wordModel]);
                return;
            }
            await _navigationService.NavigateTo<AddEntryViewModel>();
        });
        CmdGoUpdateWord = new AsyncRelayCommand(() => _navigationService.NavigateTo<UpdateEntryViewModel>([SelectedEntry]));
        CmdDeleteWord = new AsyncRelayCommand(async () =>
        {
            await _entryApi.DeleteEntryAsync(SelectedEntry.Id);
            await UpdateWords();
        });
        CmdSearch = new RelayCommand(SearchHandler);
        CmdClearSearch = new AsyncRelayCommand(async () =>
        {
            SearchText = "";
            await UpdateWords();
        });
        CmdExportToJson = new RelayCommand(() => { _fileService.ExportJson(); });
    }

    public async Task InitAsync()
    {
        await UpdateWords();
    }

    public ICommand CmdGoAddWord { get; }
    public ICommand CmdGoUpdateWord { get; }
    public ICommand CmdDeleteWord { get; }
    public ICommand CmdSearch { get; }
    public ICommand CmdExportToJson { get; }
    private void SearchHandler()
    {
        var translateSearchFunc = (string[] translates, string searchText) 
            => translates.Any(translate => translate.StartsWith(searchText, true, CultureInfo.InvariantCulture));
        FoundEntries = new ObservableCollection<EntryDto>(_entries.Where(x 
            => x.Word.StartsWith(SearchText, true, CultureInfo.InvariantCulture) || translateSearchFunc(x.Translate, SearchText)));
    }
    public ICommand CmdClearSearch { get; }

    private string _entriesCountText;
    public string EntriesCountText
    {
        get => _entriesCountText;
        set => SetProperty(ref _entriesCountText, value);
    }

    private EntryDto _selectedEntry;
    public EntryDto SelectedEntry
    {
        get => _selectedEntry; 
        set => SetProperty(ref _selectedEntry, value);
    }

    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            SearchHandler();
        }
    }

    private List<EntryDto> _entries;
    private ObservableCollection<EntryDto> _foundEntries;
    public ObservableCollection<EntryDto> FoundEntries
    {
        get => _foundEntries;
        private set => SetProperty(ref _foundEntries, value);
    }
    private async Task UpdateWords()
    {
        var entries = await _entryApi.GetEntriesAsync();
        _entries = entries.ToList();
        FoundEntries = new ObservableCollection<EntryDto>(_entries);
        EntriesCountText = $"Words count: {_entries.Count}";
    }
}