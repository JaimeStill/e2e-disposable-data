using Brainstorm.Rig.Services;
using Microsoft.AspNetCore.Mvc;

namespace Brainstorm.Rig.Controllers;

[Route("api/[controller]")]
public class RigController : Controller
{
    readonly ApiRig rig;

    public RigController(ApiRig rig)
    {
        this.rig = rig;
    }

    [HttpGet("[action]")]
    public string GetConnectionString() => rig.Connection;

    [HttpGet("[action]")]
    public async Task<bool> InitializeDatabase() =>
        await rig.InitializeDatabase();

    [HttpGet("[action]")]
    public bool StartProcess() =>
        rig.StartProcess();
}