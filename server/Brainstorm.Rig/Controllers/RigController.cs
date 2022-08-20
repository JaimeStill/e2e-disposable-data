using Brainstorm.Data;
using Brainstorm.Models;
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

    [HttpGet("[action]")]
    public async Task<bool> Initialize() =>
        await manager.InitializeAsync();

    [HttpPost("[action]")]
    public async Task<T> SeedGraph<T>([FromBody]T entity)
        where T : EntityBase =>
        await manager.SeedGraph(entity);

    [HttpPost("[action]")]
    public async Task<List<T>> SeedGraphs<T>([FromBody]List<T> entities)
        where T : EntityBase =>
        await manager.SeedGraphs(entities);
}