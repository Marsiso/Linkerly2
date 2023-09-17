using Linkerly.Domain.Application.Models.Common;

namespace Linkerly.Domain.Application.Models;

public class CodeListItemEntity : ChangeTrackingEntity
{
    public int CodeListItemID { get; set; }
    public int CodeListID { get; set; }
    public string Value { get; set; } = string.Empty;

    public CodeListEntity? CodeList { get; set; }
}