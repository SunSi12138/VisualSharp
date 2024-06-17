namespace VisualSharp;
/// <summary>
/// Node that returns the default value of a type.
/// </summary>
public class DefaultNode : Node
{
    /// <summary>
    /// Pin for the default value.
    /// </summary>
    public OutputDataPin DefaultValuePin => OutputDataPins[0];

    /// <summary>
    /// Input type pin for the type to cast to.
    /// </summary>
    public InputTypePin TypePin => InputTypePins[0];

    /// <summary>
    /// Type for the default value output. Inferred from input type pin.
    /// </summary>
    public BaseType Type => TypePin.InferredType?.Value ?? TypeSpecifier.FromType<object>();

    public DefaultNode(Graph graph)
        : base(graph)
    {
        AddInputTypePin(NameDB.Type);
        AddOutputDataPin(NameDB.Default, Type);
    }

    protected override void OnInputTypeChanged(object sender)
    {
        base.OnInputTypeChanged(sender);

        DefaultValuePin.PinType.Value = Type;
    }

    public override string ToString() => $"Default {Type.ShortName}";
}