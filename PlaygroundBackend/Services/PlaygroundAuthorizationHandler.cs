using Microsoft.AspNetCore.Authorization;

using Playground.Data;

namespace Playground.Services;

public class ProjectAuthorizationHandler
    : AuthorizationHandler<SameAuthorRequirement, string>
{
	public ProjectAuthorizationHandler(IPlaygroundUsersRepository usersRepository)
	{
		m_usersRepository = usersRepository;
	}

	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
		SameAuthorRequirement requirement, string resource)
	{
		if (context.User.Identity == null
			|| context.User.Identity.Name == null)
		{
			context.Fail();
			return Task.CompletedTask;
		}

		string username = context.User.Identity.Name;
		if (m_usersRepository.IsOwnedByUser(resource, username))
		{
			context.Succeed(requirement);
		}

		return Task.CompletedTask;
	}

	private readonly IPlaygroundUsersRepository m_usersRepository;
}

public class SameAuthorRequirement : IAuthorizationRequirement
{}
