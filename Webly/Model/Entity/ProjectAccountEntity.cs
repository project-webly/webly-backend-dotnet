using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webly.Model.Entity;

public enum ProjectAccountType
{
    Owner,
    Admin,
    User,
}

public class ProjectAccountEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public AccountEntity Account { get; set; }

    [Required]
    public ProjectEntity Project { get; set; }

    [Required]
    public ProjectAccountType ProjectAccountType { get; set; }
}
