using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Playground.Project;

namespace Playground.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class PlaygroundController : ControllerBase
{
    public PlaygroundController(IAuthorizationService authorizationService, IProjectRepository playgroundRepository, IPlaygroundUsersRepository usersRepository)
    {
        m_playgroundRepository = playgroundRepository;
		m_usersRepository = usersRepository;
		m_authorizationService = authorizationService;
    }

    // GET api/playground/{hash}
    [HttpGet("{hash}")]
    public ActionResult<ProjectData> Get(string hash)
    {
		try
		{
			var playgroundData = m_playgroundRepository.GetByHash(hash);
			return Ok(playgroundData);
		}
		catch (ProjectDoesntExists)
		{
			return NotFound();
		}
    }

    // POST api/playground
    [HttpPost]
    public ActionResult<string> Save([FromBody] ProjectData dataToSave)
    {
        var savedData = m_playgroundRepository.Save(dataToSave);
        return Ok(savedData.hash);
    }

    // PUT api/playground
	[Authorize]
    [HttpPut]
    public ActionResult Update([FromBody] ProjectData dataToUpdate)
    {
		try
		{
			m_authorizationService.AuthorizeAsync(User, dataToUpdate.hash, );
			m_playgroundRepository.Update(dataToUpdate);
			return Ok();
		}
		catch (ProjectDoesntExists)
		{
			return NotFound();
		}
    }

    // DELETE api/playground/{hash}
	[Authorize]
    [HttpDelete("{hash}")]
    public ActionResult Delete(string hash)
    {
		try
		{
			m_playgroundRepository.Delete(hash);
		}
		catch (ProjectDoesntExists)
		{
			return NotFound();
		}
        return Ok();
    }

    private readonly IProjectRepository m_playgroundRepository;
    private readonly IPlaygroundUsersRepository m_usersRepository;
    private readonly IAuthorizationService m_authorizationService;
}
