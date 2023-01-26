using Microsoft.EntityFrameworkCore;
using Webly.Dtos;
using Webly.Exceptions;
using Webly.Models;
using Webly.Models.Entity;

namespace Webly.Services;

public class ProjectService
{
    private readonly ApplicationDbContext _db;

    public ProjectService(ApplicationDbContext db)
    {
        _db = db;
    }

    public IEnumerable<ProjectDto> GetProjects(AccountEntity account)
    {
        return _db.ProjectAccounts
            .Include(a => a.Project)
            .Where(a => a.Account == account)
            .Select(a => new ProjectDto
            {
                Name = a.Project.Name,
                Id = a.Project.Id,
            });
    }

    public async Task<ProjectDto> GetProjectById(AccountEntity account, long id)
    {
        var project = await _db.Projects.SingleOrDefaultAsync(x => x.Id == id) 
                      ?? throw new ProjectNotFoundException();

        _ = await _db.ProjectAccounts
                .SingleOrDefaultAsync(x => x.Account == account && x.Project == project)
            ?? throw new UnAuthorizedProjectException();

        return new ProjectDto()
        {
            Id = project.Id,
            Name = project.Name,
        };
    }

    public async Task DeleteProject(AccountEntity account, long id)
    {
        var project = await _db.Projects.SingleOrDefaultAsync(x => x.Id == id)
                      ?? throw new ProjectNotFoundException();

        var projectAccount = await _db.ProjectAccounts
                .SingleOrDefaultAsync(x => x.Account == account && x.Project == project)
            ?? throw new UnAuthorizedProjectException();

        if (projectAccount.ProjectAccountType != ProjectAccountType.Owner)
        {
            throw new UnAuthorizedProjectException();
        }

        _db.ProjectAccounts.Remove(projectAccount);
        _db.Projects.Remove(project);
        await _db.SaveChangesAsync();
    }

    public async Task RenameProject(AccountEntity account, long id, string newName)
    {
        var project = await _db.Projects.SingleOrDefaultAsync(x => x.Id == id)
                      ?? throw new ProjectNotFoundException();

        var projectAccount = await _db.ProjectAccounts
                                 .SingleOrDefaultAsync(x => x.Account == account && x.Project == project)
                             ?? throw new UnAuthorizedProjectException();

        if (projectAccount.ProjectAccountType != ProjectAccountType.Owner)
        {
            throw new UnAuthorizedProjectException();
        }

        project.Name = newName;
        await _db.SaveChangesAsync();
    }

    public async Task<long> CreateProject(AccountEntity account, CreateProjectDto dto)
    {
        var project = new ProjectEntity()
        {
            Name = dto.Name,
        };
        _db.Projects.Add(project);
        _db.ProjectAccounts.Add(new ProjectAccountEntity()
        {
            Project = project,
            Account = account,
            ProjectAccountType = ProjectAccountType.Owner
        });

        await _db.SaveChangesAsync();

        return project.Id;
    }
}