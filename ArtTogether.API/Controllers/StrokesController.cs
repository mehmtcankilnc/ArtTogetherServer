using ArtTogether.Application.Features.Strokes.Queries;
using ArtTogether.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArtTogether.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StrokesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{projectId}")]
    public async Task<IActionResult> GetHistory(string projectId)
    {
        if (!Guid.TryParse(projectId, out Guid projectGuid))
        {
            return BadRequest();
        }
        var query = new GetStrokesBySessionQuery(projectGuid);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
