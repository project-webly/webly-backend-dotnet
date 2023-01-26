using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Webly.Dtos;
using Webly.Exceptions;
using Webly.Models.Entity;
using Webly.Services;

namespace Webly.Controllers;

[Authorize]
[Route("/api/v1/project")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly ProjectService _projectService;
    private readonly UserManager<AccountEntity> _userManager;
    private readonly ILogger _logger;

    public ProjectController(ProjectService projectService, UserManager<AccountEntity> userManager, ILogger<ProjectController> logger)
    {
        _projectService = projectService;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<long>> CreateProject(CreateProjectDto dto)
    {
        var account = await _userManager.GetUserAsync(HttpContext.User);

        var id = await _projectService.CreateProject(account, dto);
        return Created($"/api/project/{id}", id);
    }

    [HttpGet]
    public async Task<ActionResult<ProjectDto>> GetProjects()
    {
        var account = await _userManager.GetUserAsync(HttpContext.User);
        
        return Ok(_projectService.GetProjects(account));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetProject(long id)
    {
        var account = await _userManager.GetUserAsync(HttpContext.User);

        try
        {
            return Ok(await _projectService.GetProjectById(account, id));
        }
        catch (ProjectNotFoundException)
        {
            return NotFound();
        }
        catch (UnAuthorizedProjectException)
        {
            return Unauthorized();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProject(long id)
    {
        var account = await _userManager.GetUserAsync(HttpContext.User);

        try
        {
            await _projectService.DeleteProject(account, id);
            return NoContent();
        }
        catch (ProjectNotFoundException)
        {
            return NotFound();
        }
        catch (UnAuthorizedProjectException)
        {
            return Unauthorized();
        }
    }

    [HttpPost("{id}/rename")]
    public async Task<ActionResult> RenameProject(long id, string newName)
    {
        var account = await _userManager.GetUserAsync(HttpContext.User);
        try
        {
            await _projectService.RenameProject(account, id, newName);
            return Ok();
        }
        catch (ProjectNotFoundException)
        {
            return NotFound();
        }
        catch (UnAuthorizedProjectException)
        {
            return Unauthorized();
        }
    }
}