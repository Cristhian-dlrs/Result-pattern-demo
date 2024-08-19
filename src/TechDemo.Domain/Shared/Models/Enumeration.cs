namespace TechDemo.Domain.Shared.Models;

public abstract class Enumeration : IComparable
{
    public int Id { get; }
    public string Description { get; }

    protected Enumeration(int id, string description)
    {
        Id = id;
        Description = description;
    }

    public override string ToString() => Description;

    public override int GetHashCode() => (Id, Description).GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (Enumeration)obj;
        return Id == other.Id && Description == other.Description;
    }


    public int CompareTo(object? other)
    {
        if (other is null) return 1;
        if (ReferenceEquals(this, other)) return 0;

        return Id.CompareTo(((Enumeration)other).Id);
    }
}