using Playground.Models;

namespace Playground.Data;

public class ProjectDoesntExists : Exception;
public class ProjectHasInvalidData : Exception
{
    public ProjectHasInvalidData(string? message)
		: base(message)
    {
    }
}

public interface IProjectRepository
{
	ProjectData GetByHash(string hash);

	List<ProjectData> GetProjectsSince(DateTime dateSince);

	ProjectData Save(ProjectData dataToSave);
	void Update(ProjectData dataToUpdate);
	void Delete(string hash);
}
