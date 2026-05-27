
namespace Domain.Abstractions
{
    public abstract class AggregateRoot<TEntityId> : Entity<TEntityId>
    {
        protected AggregateRoot(TEntityId id) : base(id) { }
        
        protected AggregateRoot() : base() { }
    }
}