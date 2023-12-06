using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Playground.Data;
using Playground.Models;
using Playground.Services;

namespace Playground.Controllers;

[ApiController]
[Route("/api/projects/")]
public class ProjectController : ControllerBase
{
    public ProjectController(IAuthorizationService authorizationService, IProjectRepository projectRepository)
    {
        m_projectRepository = projectRepository;
		m_authorizationService = authorizationService;
    }

    // GET api/project/{hash}
    [HttpGet("{hash}")]
    public ActionResult<ProjectData> Get(string hash)
    {
		try
		{
			var projectData = m_projectRepository.GetByHash(hash);
			return Ok(projectData);
		}
		catch (ProjectDoesntExists)
		{
			return NotFound();
		}
    }

    // POST api/project
    [HttpPost]
    public ActionResult<string> Save([FromBody] ProjectData dataToSave)
    {
		if (dataToSave.thumbnail != null
			&& dataToSave.thumbnail.Length != 0)
		{
			if (!dataToSave.thumbnail.StartsWith("data:image/jpeg;base64"))
			{
				return BadRequest();
			}
		}
        var savedData = m_projectRepository.Save(dataToSave);
        return Ok(savedData.hash);
    }

    // PUT api/project
    [HttpPut]
	async public Task<ActionResult> Update([FromBody] ProjectData dataToUpdate)
    {
		try
		{
			var authorizationRes = await m_authorizationService.AuthorizeAsync(User, dataToUpdate.hash, new SameAuthorRequirement());
			if (authorizationRes.Succeeded)
			{
				m_projectRepository.Update(dataToUpdate);
				return Ok();
			}
			return Forbid();
		}
		catch (ProjectDoesntExists)
		{
			return NotFound();
		}
    }

    // DELETE api/project/{hash}
    [HttpDelete("{hash}")]
    async public Task<ActionResult> Delete(string hash)
    {
		try
		{
			var authorizationRes = await m_authorizationService.AuthorizeAsync(User, hash, new SameAuthorRequirement());
			if (authorizationRes.Succeeded)
			{
				m_projectRepository.Delete(hash);
				return Ok();
			}
			return Forbid();
		}
		catch (ProjectDoesntExists)
		{
			return NotFound();
		}
    }

    private readonly IProjectRepository m_projectRepository;
    private readonly IAuthorizationService m_authorizationService;
}
