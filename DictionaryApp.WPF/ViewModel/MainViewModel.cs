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

public class MainViewModel : ObservableObject
{
    private ObservableObject _currentViewModel;
    public ObservableObject CurrentViewModel
    {
        get => _currentViewModel;
        set => SetProperty(ref _currentViewModel, value);
    }
}

public class WordsViewModel : ObservableObject, IInitingObject
{
    private readonly INavigationService _navigationService;
    private readonly EntryApi _entryApi;
    private readonly IFileService _fileService;

    public WordsViewModel(INavigationService navigationService, IFileService fileService, IEntryService entryService)
    {
        _navigationService = navigationService;
        _fileService = fileService;
        _entryApi = new EntryApi(entryService);
        CmdGoAddWord = new RelayCommand(async () =>
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
        /*CmdGoUpdateWord = new RelayCommand(() 
            => _navigationService.NavigateTo(new UpdateWordViewModel(navigationService, wordService, SelectedWord)));*/
        CmdGoUpdateWord = new RelayCommand(() => _navigationService.NavigateTo<UpdateEntryViewModel>([SelectedEntry]));
        CmdDeleteWord = new RelayCommand(async () =>
        {
            await _entryApi.DeleteEntryAsync(SelectedEntry.Id);
            await UpdateWords();
        });
        CmdSearch = new RelayCommand(SearchHandler);
        CmdClearSearch = new RelayCommand(async () =>
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
        CmdGoWords = new RelayCommand(async () => await _navigationService.NavigateTo<WordsViewModel>());
        CmdAddWord = new RelayCommand(async () => await AddWordHandler());
        CmdImportTxt = new RelayCommand(async () => await ImportTxtHandler());
        CmdImportJson = new RelayCommand(async () => await ImportJsonHandler());
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

public class UpdateEntryViewModel : ObservableObject, IEntryModelObject, IInitingObject
{
    private readonly INavigationService _navigationService;
    private readonly EntryApi _entryApi;

    public UpdateEntryViewModel(INavigationService navigationService, IEntryService entryService)
    {
        _navigationService = navigationService;
        _entryApi = new EntryApi(entryService);
        CmdGoWords = new RelayCommand(async () => await GoWordsHandler());
        CmdUpdateWord = new RelayCommand(async () => await UpdateWordHandler());
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