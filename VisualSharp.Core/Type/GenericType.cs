namespace VisualSharp;


/// <summary>
/// An unbound generic type.
/// </summary>
public class GenericType : BaseType
{
    /// <summary>
    /// Constraints for this generic type.
    /// </summary>
    public ObservableRangeCollection<GenericTypeConstraint> Constraints { get; private set; }

    public GenericType(string name, IEnumerable<GenericTypeConstraint> constraints = null) : base(name)
    {
        if (constraints == null) Constraints = new ObservableRangeCollection<GenericTypeConstraint>();
        else Constraints = new ObservableRangeCollection<GenericTypeConstraint>(constraints);
    }

    /// <summary>
    /// Blank for generic types.
    /// </summary>
    public override string FullCodeNameUnbound=>"";

    /// <summary>
    /// Creates a GenericType from a type. Type must be a generic argument.
    /// </summary>
    /// <typeparam name="T">Type to generate GenericType for.</typeparam>
    /// <returns>GenericType for the passed type.</returns>
    public static GenericType FromType<T>()=>FromType(typeof(T));

    /// <summary>
    /// Creates a GenericType from a type. Type must be a generic argument.
    /// </summary>
    /// <param name="type">Type to generate GenericType for.</param>
    /// <returns>GenericType for the passed type.</returns>
    public static GenericType FromType(Type type)
    {
        if (!type.IsGenericParameter)
        {
            throw new ArgumentException(nameof(type));
        }

        // TODO: Convert constraints
        GenericType genericType = new GenericType(type.Name);

        return genericType;
    }

    public override bool Equals(object obj)
    {
        if (obj is TypeSpecifier t)
        {
            // TODO: Check constraints
            return true;
        }
        else if (obj is GenericType genType)
        {
            // TODO: Check constraints
            return Name == genType.Name;
        }

        return false;
    }

    public override int GetHashCode()=>Name.GetHashCode();

    public static bool operator ==(GenericType a, GenericType b)=>a.Equals(b);

    public static bool operator !=(GenericType a, GenericType b)=>!a.Equals(b);

    public static bool operator ==(GenericType a, TypeSpecifier b)=>a.Equals(b);

    public static bool operator !=(GenericType a, TypeSpecifier b)=>!a.Equals(b);
}
