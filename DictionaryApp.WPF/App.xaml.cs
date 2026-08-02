using System.Windows;
using DictionaryApp.Application;
using DictionaryApp.Infrastructure;
using DictionaryApp.Infrastructure.Data;
using DictionaryApp.WPF.Api;
using DictionaryApp.WPF.Interfaces.Services;
using DictionaryApp.WPF.Services;
using DictionaryApp.WPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryApp.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    private readonly IServiceProvider _serviceProvider;
    
    public App()
    {
        var services = new ServiceCollection();
        services.AddApplication();
        services.AddInfrastructure();
        // add wpf services
        services.AddSingleton<MainViewModel>();
        services.AddTransient<WordsViewModel>();
        services.AddTransient<AddEntryViewModel>();
        services.AddTransient<UpdateEntryViewModel>();
        services.AddSingleton<MainWindow>(s => new MainWindow()
        {
            DataContext = s.GetRequiredService<MainViewModel>()
        });
        services.AddScoped<INavigationService, NavigationService>();
        services.AddScoped<IFileService, FileService>();
        services.AddTransient<IMessageService, MessageService>();
        services.AddScoped<AppDbContext>();
        _serviceProvider = services.BuildServiceProvider();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        var navigationService = _serviceProvider.GetRequiredService<INavigationService>();
        await navigationService.NavigateTo<WordsViewModel>();
        MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        MainWindow?.Show();
        base.OnStartup(e);
    }
}