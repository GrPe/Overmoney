namespace Overmoney.Domain.Features.Common.Models;

public abstract class Identity<T> where T : notnull
{

    public T Value { get; private set; }

    public Identity(T id)
    {
        Value = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var identity = obj as Identity<T>;

        return Value.Equals(identity!.Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
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
