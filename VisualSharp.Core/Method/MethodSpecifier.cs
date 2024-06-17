namespace VisualSharp;

public class MethodSpecifier(string name, IEnumerable<MethodParameter> parameters ,IEnumerable<BaseType> genericArguments, IEnumerable<BaseType> returnTypes, MethodModifier modifiers, MemberVisibility visibility, TypeSpecifier declaringType)
{
    public string Name { get; private set; } = name;
    public TypeSpecifier DeclaringType { get; private set; } = declaringType;

    public IList<MethodParameter> Parameters { get; private set; } = parameters.ToList();

    public IReadOnlyList<BaseType> ArgumentTypes =>Parameters.Select(p=>(BaseType)p).ToArray();

    // TODO: 也许可以使用命名元组的形式返回值
    public IList<BaseType> ReturnTypes { get; private set; } = returnTypes.ToList();

    public MethodModifier Modifiers { get; private set; } = modifiers;

    public MemberVisibility Visibility { get; private set; } = visibility;

    public IList<BaseType> GenericArguments { get; private set;} = genericArguments.ToList();

    public override string ToString()
    {
        string methodString = "";

        if (Modifiers.HasFlag(MethodModifier.Static))
        {
            methodString += $"{DeclaringType.ShortName}.";
        }

        methodString += Name;

        string argTypeString = string.Join(", ", Parameters.Select(a => a.Value.ShortName));

        methodString += $"({argTypeString})";

        if (GenericArguments.Count > 0)
        {
            string genArgTypeString = string.Join(", ", GenericArguments.Select(s => s.ShortName));
            methodString += $"<{genArgTypeString}>";
        }

        if (ReturnTypes.Count > 0)
        {
            string returnTypeString = string.Join(", ", ReturnTypes.Select(s => s.ShortName));
            methodString += $" : {returnTypeString}";
        }

        return methodString;
    }

    public override bool Equals(object obj)
    {
        if(obj is MethodSpecifier other)
        {
            return other.Name == Name
                && other.DeclaringType == DeclaringType
                && other.ArgumentTypes.SequenceEqual(ArgumentTypes)
                && other.ReturnTypes.SequenceEqual(ReturnTypes)
                && other.Modifiers == Modifiers
                && other.GenericArguments.SequenceEqual(GenericArguments);
        }
        else
        {
            return false;
        }
    }

    public override int GetHashCode()=>HashCode.Combine(Name, Modifiers, string.Join(",", GenericArguments), string.Join(",", ReturnTypes), string.Join(",", Parameters), Visibility, DeclaringType);

    public static bool operator ==(MethodSpecifier left, MethodSpecifier right)
    {
        if(left is null) return right is null;
        return left.Equals(right);
    }
    public static bool operator !=(MethodSpecifier left, MethodSpecifier right)=>!(left == right);
}