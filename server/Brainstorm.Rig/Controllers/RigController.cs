using Brainstorm.Models.Entities;
using Brainstorm.Rig.Models;
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
    public RigState GetState() => rig.State;

    [HttpGet("[action]")]
    public async Task<RigState> InitializeDatabase() =>
        await rig.InitializeDatabase();

    [HttpGet("[action]")]
    public async Task<RigState> DestroyDatabase() =>
        await rig.DestroyDatabase();

    [HttpGet("[action]")]
    public RigState StartProcess() =>
        rig.StartProcess();

    [HttpGet("[action]")]
    public RigState KillProcess() =>
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