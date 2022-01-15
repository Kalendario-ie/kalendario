using System;
using System.Text.Json;

namespace Kalendario.Core.Entities;

public class AuditEntity : BaseEntity
{
    public string EntityState { get; set; }
    public string EntityTable { get; set; }
    public string EntityId { get; set; }
    public string OldValues { get; set; }
    public string NewValues { get; set; }

    public T Deserialize<T>() where T : BaseEntity
    {
        var entity = JsonSerializer.Deserialize<T>(this.NewValues);
        entity.Id = Guid.Parse(EntityId);
        return entity;
    }
}