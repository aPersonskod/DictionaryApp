using DictionaryApp.Application.Interfaces;
using DictionaryApp.Infrastructure.Data;
using DictionaryApp.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<AppDbContext>();
        services.AddScoped<IEntryRepository, EntryRepository>();
        return services;
    }
}