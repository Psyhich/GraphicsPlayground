using Microsoft.AspNetCore.Mvc;

using Playground.Notebook;

namespace Playground.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class PlaygroundController : ControllerBase
{
    public PlaygroundController(IPlaygroundRepository playgroundRepository)
    {
        m_playgroundRepository = playgroundRepository;
    }

    // GET api/playground/{hash}
    [HttpGet("{hash}")]
    public ActionResult<PlaygroundData> Get(string hash)
    {
		try
		{
			var playgroundData = m_playgroundRepository.GetByHash(hash);
			return Ok(playgroundData);
		}
		catch (PlaygroundDoesntExists)
		{
			return NotFound();
		}
    }

    // POST api/playground
    [HttpPost]
    public ActionResult<string> Post([FromBody] PlaygroundData dataToSave)
    {
        var savedData = m_playgroundRepository.Save(dataToSave);
        return Ok(savedData.hash);
    }

    // PUT api/playground
    [HttpPut]
    public ActionResult Put([FromBody] PlaygroundData dataToUpdate)
    {
		try
		{
			m_playgroundRepository.Update(dataToUpdate);
			return Ok();
		}
		catch (PlaygroundDoesntExists)
		{
			return NotFound();
		}
    }

    // DELETE api/playground/{hash}
    [HttpDelete("{hash}")]
    public ActionResult Delete(string hash)
    {
		try
		{
			m_playgroundRepository.Delete(hash);
		}
		catch (PlaygroundDoesntExists)
		{
			return NotFound();
		}
        return Ok();
    }

    private readonly IPlaygroundRepository m_playgroundRepository;
}
