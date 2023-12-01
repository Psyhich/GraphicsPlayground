using Microsoft.AspNetCore.Authorization;

namespace Playground.Services;

public class UserAuthorizationHandler :
    AuthorizationHandler<SameUserRequirement, string>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
		SameUserRequirement requirement, string checkedUsername)
    {
		if (context.User.Identity == null
			|| context.User.Identity.Name == null)
		{
			context.Fail();
			return Task.CompletedTask;
		}

		string username = context.User.Identity.Name;
		if (checkedUsername == username)
		{
			context.Succeed(requirement);
		}

		return Task.CompletedTask;
    }
}

public class SameUserRequirement : IAuthorizationRequirement
{}
