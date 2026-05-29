
namespace Domain.Abstractions
{
    public abstract class Entity<TEntityId>
    {
        public TEntityId Id { get; protected set; } = default!;

        protected Entity(TEntityId id)
        {
            Id = id;
        }

        protected Entity() { }
    }
}