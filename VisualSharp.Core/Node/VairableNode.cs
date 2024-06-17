
namespace VisualSharp;

/// <summary>
/// Abstract class for variable nodes.
/// </summary>
public abstract partial class VariableNode : Node
{
    /// <summary>
    /// Target object of this variable node.
    /// Can be null for local variables.
    /// </summary>
    public InputDataPin TargetPin => !IsLocalVariable && !IsStatic ? InputDataPins[0] : null;

    /// <summary>
    /// Pin that outputs the value of the variable.
    /// </summary>
    public OutputDataPin ValuePin => OutputDataPins[0];

    /// <summary>
    /// Whether the variable is a local variable.
    /// </summary>
    public bool IsLocalVariable => TargetType is null;

    /// <summary>
    /// Name of this variable.
    /// </summary>
    public string VariableName { get => Variable.Name; }

    /// <summary>
    /// Specifier for the type of the target object.
    /// </summary>
    public TypeSpecifier TargetType => Variable.DeclaringType;

    /// <summary>
    /// Whether the variable is static.
    /// </summary>
    public bool IsStatic => Variable.Modifiers.HasFlag(VariableModifier.Static);

    /// <summary>
    /// Whether this variable node is for an indexer (eg. dict["key"]).
    /// </summary>
    public bool IsIndexer => Variable.Name == "this[]";

    /// <summary>
    /// Specifier for the type of the index.
    /// TODO: Get indexer type
    /// </summary>
    public BaseType IndexType => IsIndexer ? TypeSpecifier.FromType<object>() : null;


    /// <summary>
    /// Data pin for the indexer.
    /// </summary>
    public InputDataPin IndexPin => IsIndexer ? InputDataPins[1] : null;

    /// <summary>
    /// Specifier for the underlying variable.
    /// </summary>
    public VariableSpecifier Variable { get; private set; }

    protected VariableNode(Graph graph, VariableSpecifier variable)
        : base(graph)
    {
        Variable = variable;

        // Add target input pin if not local or static
        if (!IsLocalVariable && !Variable.Modifiers.HasFlag(VariableModifier.Static))
        {
            AddInputDataPin(NameDB.Target, TargetType);
        }

        if (IsIndexer)
        {
            AddInputDataPin(NameDB.Index, IndexType);
        }

        AddOutputDataPin(Variable.Type.ShortName, Variable.Type);
    }
}
