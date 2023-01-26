using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webly.Model.Entity;

public class FolderEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Required]
    public string Name { get; set; }

    public ProjectEntity Project { get; set; }

    public long ParentFolderId { get; set; }

    [ForeignKey(nameof(ParentFolderId))]
    public FolderEntity ParentFolder { get; set; }

    [InverseProperty(nameof(ParentFolder))]
    public ICollection<FolderEntity> Children { get; } = new List<FolderEntity>();
}