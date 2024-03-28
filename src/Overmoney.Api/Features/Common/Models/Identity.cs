namespace Overmoney.Api.Features.Common.Models;

public abstract class Identity<T> where T : notnull
{

    public T? Id { get; protected set; }

    public Identity(T id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var identity = obj as Identity<T>;

        return Id.Equals(identity!.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Identity<T> left, Identity<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Identity<T> left, Identity<T> right)
    {
        return !(left == right);
    }
}
