namespace VisualSharp;

/// <summary>
/// Node representing the creation of a delegate (method pointer).
/// </summary>
public class MakeDelegateNode : Node
{
    /// <summary>
    /// Specifier describing the method the delegate is created for.
    /// </summary>
    public MethodSpecifier MethodSpecifier { get; private set; }

    /// <summary>
    /// The target this delegate is for ("this").
    /// Accessing this for static methods (IsFromStaticMethod==true)
    /// will throw an exception.
    /// </summary>
    public InputDataPin TargetPin => InputDataPins[0];

    /// <summary>
    /// Whether the delegate is for a static method.
    /// </summary>
    public bool IsFromStaticMethod => MethodSpecifier.Modifiers.HasFlag(MethodModifier.Static);

    public MakeDelegateNode(Graph graph, MethodSpecifier methodSpecifier)
        : base(graph)
    {
        MethodSpecifier = methodSpecifier;

        if (!IsFromStaticMethod)
        {
            AddInputDataPin(NameDB.Target, methodSpecifier.DeclaringType);
        }

        TypeSpecifier delegateType;

        if (methodSpecifier.ReturnTypes.Count == 0)
        {
            delegateType = new TypeSpecifier("System.Action", false, false, methodSpecifier.ArgumentTypes);
        }
        else if (methodSpecifier.ReturnTypes.Count == 1)
        {
            delegateType = new TypeSpecifier("System.Func", false, false, methodSpecifier.ArgumentTypes.Concat(methodSpecifier.ReturnTypes).ToList());
        }
        else
        {
            throw new NotImplementedException("Only 0 and 1 return types are supported right now.");
        }

        AddOutputDataPin(delegateType.ShortName, delegateType);
    }

    public override string ToString()=> IsFromStaticMethod? $"Make Delegate from {MethodSpecifier.DeclaringType} {MethodSpecifier.Name}" : $"Make Delegate from {MethodSpecifier.Name}";
}
