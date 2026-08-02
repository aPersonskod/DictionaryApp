using System.Reflection;
using DictionaryApp.Application.Interfaces;
using DictionaryApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IEntryService, EntryService>();
        return services;
    }
}