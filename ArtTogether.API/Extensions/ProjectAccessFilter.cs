using Microsoft.AspNetCore.SignalR;
using ArtTogether.Domain.Interfaces;

namespace ArtTogether.API.Extensions;

public class ProjectAccessFilter(IProjectRepository repository) : IHubFilter
{
    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext invocationContext,
        Func<HubInvocationContext, ValueTask<object?>> next)
    {
        var projectIdArg = invocationContext.HubMethodArguments.FirstOrDefault(a => a is string || a is Guid);

        if (projectIdArg != null && Guid.TryParse(projectIdArg.ToString(), out Guid projectId))
        {
            var userIdClaim = invocationContext.Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out Guid userId))
            {
                var isMember = await repository.IsMemberAsync(projectId, userId);

                if (!isMember)
                {
                    throw new HubException("You cannot access this project");
                }
            }
        }

        return await next(invocationContext);
    }
}
