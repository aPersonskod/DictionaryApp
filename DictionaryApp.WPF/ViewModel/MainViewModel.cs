using CommunityToolkit.Mvvm.ComponentModel;

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