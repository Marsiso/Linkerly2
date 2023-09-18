using Linkerly.Domain.Application.Models.Common;

namespace Linkerly.Domain.Application.Models;

public class CodeListEntity : ChangeTrackingEntity
{
    public int CodeListID { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<CodeListItemEntity>? Items { get; set; }
}
