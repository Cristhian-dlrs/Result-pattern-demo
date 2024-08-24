namespace TechDemo.Domain.Shared.Models;

public abstract class Entity
{
    public Guid Id { get; protected set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Entity)) return false;

        if (ReferenceEquals(this, obj)) return true;

        var entity = (Entity)obj;

        return Id == entity.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity a, Entity b)
    {
        if (a is null && b is null) return true;

        if (a is null || b is null) return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b) => !(a == b);
}