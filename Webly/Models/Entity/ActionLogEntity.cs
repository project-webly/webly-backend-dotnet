using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webly.Models.Entity;

public enum ActionType
{
    CreateLink,
    DeleteLink,
    RenameLink,
    MoveLink,
    CreateFolder,
    DeleteFolder,
    RenameFolder,
    MoveFolder,
}

public class ActionLogEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public ActionType ActionType { get; set; }

    public ProjectEntity Project { get; set; }

    public long LinkId { get; set; }
    public string NewName { get; set; }
    public string OldName { get; set; }
    public string Url { get; set; }
    public long? ParentId { get; set; }
    public long? FromFolderId { get; set; }
    public long? ToFolderId { get; set; }
    public long FolderId { get; set; }
    public string Name { get; set; }
}
