using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Playground.Services;

public class PlaygroundAuthorizationCRUDHandler
	: AuthorizationHandler<OperationAuthorizationRequirement, string>
{

}
