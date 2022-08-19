using Brainstorm.Models.Entities;
using Brainstorm.Models.Query;
using Brainstorm.Services;
using Microsoft.AspNetCore.Mvc;

namespace Brainstorm.Api.Controllers;

[Route("api/[controller]")]
public class NoteController : EntityBaseController<Note>
{
    readonly NoteService noteSvc;

    public NoteController(NoteService svc) : base(svc)
    {
        noteSvc = svc;
    }

    [HttpGet("[action]/{topicId}")]
    public async Task<IActionResult> QueryByTopic(
        [FromRoute]int topicId,
        [FromQuery]QueryParams queryParams
    ) => Ok(await noteSvc.QueryByTopic(topicId, queryParams));

    [HttpPost("[action]")]
    public async Task<IActionResult> ValidateTitle([FromBody]Note note) =>
        Ok(await noteSvc.ValidateTitle(note));
}