using Playground.Models;

namespace Playground.Data;

public class ProjectDoesntExists : Exception
{}

public interface IProjectRepository
{
	ProjectData GetByHash(string hash);

	ProjectData Save(ProjectData dataToSave);
	void Update(ProjectData dataToUpdate);
	void Delete(string hash);
}
