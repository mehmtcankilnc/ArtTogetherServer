using ArtTogether.Application.Features.Strokes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArtTogether.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StrokesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{roomId}")]
    public async Task<IActionResult> GetHistory(string roomId)
    {
        var query = new GetStrokesBySessionQuery(roomId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
