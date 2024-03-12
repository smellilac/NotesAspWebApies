using Microsoft.EntityFrameworkCore;
using Notes.Application;

namespace Notes.WebApi.PersistanceLevel;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistance(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration["DbConnection"];
        services.AddDbContext<NotesDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });

        services.AddScoped<INotesDbContext>(provider =>
            provider.GetService<NotesDbContext>());
        
        return services;
    }
}
