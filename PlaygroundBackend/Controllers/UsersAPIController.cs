using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Playground.Data;
using Playground.Models;
using Playground.Services;

namespace Playground.Controllers;

[ApiController]
[Route("/api/users")]
class UsersAPIController : Controller
{
	public UsersAPIController(IAuthorizationService authorizationService, IPlaygroundUsersRepository usersRepository)
	{
		m_usersRepository = usersRepository;
		m_authorizationService = authorizationService;
	}

    // GET api/users/{hash}
    [HttpGet("{username}")]
    public ActionResult<PlaygroundUser> GetUser(string username)
    {
		try
		{
			return m_usersRepository.GetUser(username);
		} catch(UserDoesntExist)
		{
			return NotFound();
		}
	}

    // GET api/users/
	[HttpGet]
	public ActionResult<List<PlaygroundUser>> GetUsers()
	{
		return m_usersRepository.GetAllUsers();
	}

    // GET api/users/{hash}
    [HttpDelete("{username}")]
	async public Task<ActionResult> DeleteUser(string username)
	{
		var authorizationResult = await m_authorizationService.AuthorizeAsync(User, username, new SameUserRequirement());
		try
		{
			if (authorizationResult.Succeeded)
			{
				m_usersRepository.DeleteUser(username);
				return Ok();
			}
			return Forbid();
		} catch (UserDoesntExist)
		{
			return NotFound();
		}
	}


	private readonly IAuthorizationService m_authorizationService;
	private readonly IPlaygroundUsersRepository m_usersRepository;
}
