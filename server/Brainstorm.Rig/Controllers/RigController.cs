using Brainstorm.Models.Entities;
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
    public async Task<bool> DestroyDatabase() =>
        await rig.DestroyDatabase();

    [HttpGet("[action]")]
    public bool StartProcess() =>
        rig.StartProcess();

    [HttpGet("[action]")]
    public bool KillProcess() =>
        rig.KillProcess();

    [HttpPost("[action]")]
    public async Task<IActionResult> SeedTopic([FromBody]Topic topic) =>
        Ok(await rig.Seeder.Seed(topic));

    [HttpPost("[action]")]
    public async Task<IActionResult> SeedTopics([FromBody]List<Topic> topics) =>
        Ok(await rig.Seeder.SeedMany(topics));

    [HttpPost("[action]")]
    public async Task<IActionResult> SeedNote([FromBody]Note note) =>
        Ok(await rig.Seeder.Seed(note));

    [HttpPost("[action]")]
    public async Task<IActionResult> SeedNotes([FromBody]List<Note> notes) =>
        Ok(await rig.Seeder.SeedMany(notes));
}