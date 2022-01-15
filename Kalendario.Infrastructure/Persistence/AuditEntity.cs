using Kalendario.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Kalendario.Infrastructure.Persistence;

public class AuditEntry
{
    public AuditEntry(EntityEntry entry, string userId)
    {
        Entry = entry;
        UserId = userId;
        TableName = entry.Metadata.GetTableName();
    }

    public string UserId { get; }
    public EntityEntry Entry { get; }
    public string TableName { get; }
    public string EntityId { get; set; }
    public Dictionary<string, object> OldValues { get; } = new();
    public Dictionary<string, object> NewValues { get; } = new();
    public List<PropertyEntry> TemporaryProperties { get; } = new();

    public bool HasTemporaryProperties => TemporaryProperties.Any();

    public AuditEntity ToAudit()
    {
        return new AuditEntity
        {
            EntityState = Entry.State.ToString(),
            EntityTable = TableName,
            EntityId = EntityId,
            OldValues = OldValues.Count == 0 ? null : System.Text.Json.JsonSerializer.Serialize(OldValues),
            NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues)
        };
    }
}