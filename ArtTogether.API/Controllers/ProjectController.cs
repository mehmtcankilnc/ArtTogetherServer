using ArtTogether.Application.DTOs;
using ArtTogether.Application.DTOs.Requests;
using ArtTogether.Application.Features.Projects.Commands;
using ArtTogether.Application.Features.Projects.Queries;
using ArtTogether.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ArtTogether.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out Guid userId))
            {
                var projects = await _mediator.Send(new GetProjectsByUserIdQuery(userId));
                return Ok(projects);
            }

            return Unauthorized("Kullanıcı kimliği doğrulanamadı.");
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProjectDetailsById([FromRoute] string projectId)
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out Guid userId))
            {
                if (Guid.TryParse(projectId, out Guid projectIdAsGuid))
                {
                    var project = await _mediator.Send(new GetProjectDetailsByIdQuery(projectIdAsGuid, userId));
                    return Ok(project);
                }
            }

            return Unauthorized("Kullanıcı kimliği doğrulanamadı.");
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out Guid userId))
            {
                var projectId = await _mediator.Send(new CreateProjectCommand(request, userId));

                var baseUrl = "https://arttogether.app";

                var response = new CreatedProjectDto(
                    ProjectId: projectId,
                    ProjectName: request.ProjectName,
                    InvitationUrl: $"{baseUrl}/join/{projectId}",
                    DeepLinkUrl: $"arttogether://project/{projectId}"
                );

                return Ok(response);
            }

            return Unauthorized("Kullanıcı kimliği doğrulanamadı.");
        }

        [HttpPut("{projectId}")]
        public async Task<IActionResult> UpdateProject([FromRoute] string projectId, [FromBody] ProjectUpdateDto dto)
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out Guid userId))
            {
                if (Guid.TryParse(projectId, out Guid projectIdAsGuid))
                {
                    var project = await _mediator.Send(new UpdateProjectCommand(projectIdAsGuid, userId, dto));
                    return Ok(project);
                }
            }

            return Unauthorized("Kullanıcı kimliği doğrulanamadı.");
        }
    }
}
