namespace RopDemo.SharedKernel.Domain;

public abstract class Entity<TId> : IEntity
    where TId : struct, IEntityId
{
    public TId Id { get; protected init; }
}