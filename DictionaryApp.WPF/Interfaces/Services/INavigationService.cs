using CommunityToolkit.Mvvm.ComponentModel;

namespace DictionaryApp.WPF.Interfaces.Services;

public interface INavigationService
{
    Task NavigateTo<TViewModel>() where TViewModel : ObservableObject;
    Task NavigateTo<TViewModel>(object[] parameters) where TViewModel : ObservableObject;
    Task NavigateTo(ObservableObject viewModel);
}