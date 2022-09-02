using Brainstorm.Rig.Models;
using Microsoft.AspNetCore.SignalR;

namespace Brainstorm.Rig.Hubs;
public class RigHub : Hub
{
    public async Task TriggerOutput(RigOutput output) =>
        await Clients
            .All
            .SendAsync("output", output);
}