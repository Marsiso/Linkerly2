namespace Linkerly.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string? entityID, string? entityTypeName)
    {
        ArgumentException.ThrowIfNullOrEmpty(entityID);
        ArgumentException.ThrowIfNullOrEmpty(entityTypeName);

        EntityID = entityID;
        EntityTypeName = entityTypeName;
    }

    public string EntityID { get; }
    public string EntityTypeName { get; }
}