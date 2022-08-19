using Brainstorm.Models;
using Brainstorm.Models.Interfaces;
using Brainstorm.Models.Query;
using Microsoft.AspNetCore.Mvc;

namespace Brainstorm.Api.Controllers;
public abstract class EntityBaseController<T> : ControllerBase where T : EntityBase
{
    protected readonly IService<T> svc;

    public EntityBaseController(IService<T> svc)
    {
        this.svc = svc;
    }

    [HttpGet("[action]")]
    public virtual async Task<IActionResult> Query([FromQuery]QueryParams queryParams) =>
        Ok(await svc.Query(queryParams));

    [HttpGet("[action]/{id}")]
    public virtual async Task<IActionResult> GetById([FromRoute]int id) =>
        Ok(await svc.GetById(id));

    [HttpGet("[action]/{url}")]
    public virtual async Task<IActionResult> GetByUrl([FromRoute]string url) =>
        Ok(await svc.GetByUrl(url));

    [HttpPost("[action]")]
    public virtual async Task<IActionResult> Validate([FromBody]T entity) =>
        Ok(await svc.Validate(entity));

    [HttpPost("[action]")]
    public virtual async Task<IActionResult> AddOrUpdate([FromBody]T entity) =>
        Ok(await svc.AddOrUpdate(entity));

    [HttpDelete("[action]")]
    public virtual async Task<IActionResult> Remove([FromBody]T entity) =>
        Ok(await svc.Remove(entity));
}