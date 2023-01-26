using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webly.Model.Entity;

public class LinkEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Url { get; set; }

    [Required]
    public FolderEntity FolderEntity { get; set; }
}
