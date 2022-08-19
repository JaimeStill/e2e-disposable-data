using Brainstorm.Models.Entities;
using Brainstorm.Services;
using Microsoft.AspNetCore.Mvc;

namespace Brainstorm.Api.Controllers;

[Route("api/[controller]")]
public class TopicController : EntityBaseController<Topic>
{
    readonly TopicService topicSvc;

    public TopicController(TopicService svc) : base(svc)
    {
        topicSvc = svc;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> ValidateName([FromBody]Topic topic) =>
        Ok(await topicSvc.ValidateName(topic));
}