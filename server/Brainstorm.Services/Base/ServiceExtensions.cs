using Microsoft.Extensions.DependencyInjection;

namespace Brainstorm.Services;
public static class ServiceExtensions
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<NoteService>();
        services.AddScoped<TopicService>();
    }
}
