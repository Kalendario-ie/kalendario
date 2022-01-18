namespace Kalendario.Core.Common;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
}