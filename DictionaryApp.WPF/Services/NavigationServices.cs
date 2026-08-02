using CommunityToolkit.Mvvm.ComponentModel;
using DictionaryApp.Application.Dtos;
using DictionaryApp.WPF.Interfaces;
using DictionaryApp.WPF.Interfaces.Services;
using DictionaryApp.WPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryApp.WPF.Services;

public class NavigationService(IServiceProvider serviceProvider) : INavigationService
{
    public async Task NavigateTo<TViewModel>() where TViewModel : ObservableObject
    {
        var vm = serviceProvider.GetRequiredService<TViewModel>();
        if (vm is IInitingObject initingObject) await initingObject.InitAsync();
        serviceProvider.GetService<MainViewModel>()!.CurrentViewModel = vm;
    }

    public async Task NavigateTo<TViewModel>(object[] parameters) where TViewModel : ObservableObject
    {
        var vm = serviceProvider.GetRequiredService<TViewModel>();
        if (vm is IInitingObject initingObject) await initingObject.InitAsync();
        foreach (var parameter in parameters)
        {
            switch (vm)
            {
                case IEntryModelObject wordModelObject:
                    wordModelObject.SetWord((parameter as EntryDto)!);
                    break;
            }
        }
        serviceProvider.GetService<MainViewModel>()!.CurrentViewModel = vm;
    }

    public async Task NavigateTo(ObservableObject viewModel)
    {
        serviceProvider.GetService<MainViewModel>()!.CurrentViewModel = viewModel;
    }
}