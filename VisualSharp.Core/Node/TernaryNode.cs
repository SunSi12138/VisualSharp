namespace VisualSharp;

/// <summary>
/// Node representing a ternary operation.
/// </summary>
public class TernaryNode : ExecNode
{
    public override bool CanSetPure => true;

    /// <summary>
    /// Pin for the object to choose when the condition was true.
    /// </summary>
    public InputDataPin TrueObjectPin => InputDataPins[0];

    /// <summary>
    /// Pin for the object to choose when the condition was false.
    /// </summary>
    public InputDataPin FalseObjectPin => InputDataPins[1];

    /// <summary>
    /// Pin for the selection condition.
    /// </summary>
    public InputDataPin ConditionPin => InputDataPins[2];

    /// <summary>
    /// Input type pin for the type to select.
    /// </summary>
    public InputTypePin TypePin => InputTypePins[0];

    /// <summary>
    /// Pin that holds the selected object.
    /// </summary>
    public OutputDataPin OutputObjectPin => OutputDataPins[0];

    /// <summary>
    /// Type to cast to. Inferred from input type pin.
    /// </summary>
    public BaseType Type => TypePin.InferredType?.Value ?? TypeSpecifier.FromType<object>();

    public TernaryNode(Graph graph)
        : base(graph)
    {
        AddInputTypePin(NameDB.Type);
        AddInputDataPin(NameDB.True, TypeSpecifier.FromType<object>());
        AddInputDataPin(NameDB.False, TypeSpecifier.FromType<object>());
        AddInputDataPin(NameDB.Condition, TypeSpecifier.FromType<bool>());
        AddOutputDataPin(NameDB.Output, Type);
    }

    protected override void OnInputTypeChanged(object sender)
    {
        base.OnInputTypeChanged(sender);

        TrueObjectPin.PinType.Value = Type;
        FalseObjectPin.PinType.Value = Type;
        OutputObjectPin.PinType.Value = Type;
    }

    public override string ToString() => $"Ternary {Type.ShortName}";
}