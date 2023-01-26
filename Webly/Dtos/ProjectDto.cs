namespace Webly.Dtos;

public class ProjectDto
{
    public long Id { get; init; }
    public string Name { get; init; }
}

public class CreateProjectDto
{
    public string Name { get; init; }
}