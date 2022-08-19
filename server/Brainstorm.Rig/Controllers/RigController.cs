using Brainstorm.Data;
using Microsoft.AspNetCore.Mvc;

namespace Brainstorm.Rig.Controllers;

[Route("api/[controller]")]
public class RigController : Controller
{
    private DbManager manager;

    public RigController(DbManager manager)
    {
        this.manager = manager;
    }

    [HttpGet("[action]")]
    public string GetConnectionString() => manager.Connection;
}