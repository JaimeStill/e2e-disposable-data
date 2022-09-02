namespace Brainstorm.Rig.Hubs;

public static class HubExtensions
{
    public static void MapHubs(this IEndpointRouteBuilder app)
    {
        app.MapHub<RigHub>("/rig-socket");
    }
}