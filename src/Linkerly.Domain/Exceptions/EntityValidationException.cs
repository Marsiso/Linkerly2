namespace Linkerly.Domain.Exceptions;

public class EntityValidationException : Exception
{
    public EntityValidationException(Dictionary<string, string[]> entityErrorsByProperty)
    {
        ArgumentNullException.ThrowIfNull(entityErrorsByProperty);

        EntityErrorsByProperty = entityErrorsByProperty;
    }


    public Dictionary<string, string[]> EntityErrorsByProperty { get; }
}
