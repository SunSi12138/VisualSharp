namespace VisualSharp;

public class LiteralNode : Node
{
    /// <summary>
    /// Output data pin for the value of this literal.
    /// </summary>
    public OutputDataPin ValuePin => OutputDataPins[0];

    /// <summary>
    /// Input data pin for the value of this literal.
    /// </summary>
    public InputDataPin InputValuePin => InputDataPins[0];

    /// <summary>
    /// Specifier for the type of this literal.
    /// </summary>
    public TypeSpecifier LiteralType { get; private set; }

    public LiteralNode(Graph graph, TypeSpecifier literalType)
        : base(graph)
    {
        LiteralType = literalType;

        // Add type pins for each generic argument of the literal type
        // and monitor them for changes to reconstruct the actual pin types.
        foreach (var genericArg in literalType.GenericArguments.OfType<GenericType>())
        {
            AddInputTypePin(genericArg.Name);
        }

        AddInputDataPin(NameDB.Value, literalType);
        AddOutputDataPin(NameDB.Value, literalType);

        UpdatePinTypes();
    }

    protected override void OnInputTypeChanged(object sender)
    {
        base.OnInputTypeChanged(sender);

        UpdatePinTypes();
    }

    private void UpdatePinTypes()
    {
        // Construct type with generic arguments replaced by our input type pins
        BaseType constructedType = GenericsUtil.ConstructWithTypePins(LiteralType, InputTypePins);

        // Set pin types
        // TODO: Check if we can leave the pins connected

        if (constructedType != InputValuePin.PinType.Value)
        {
            GraphUtil.DisconnectInputDataPin(InputValuePin);
            InputValuePin.PinType.Value = constructedType;
        }

        if (constructedType != ValuePin.PinType.Value)
        {
            GraphUtil.DisconnectOutputDataPin(ValuePin);
            ValuePin.PinType.Value = constructedType;
        }
    }

    /// <summary>
    /// Creates a literal node and gives it an unconnected value.
    /// </summary>
    /// <typeparam name="T">Type of the unconnected value.</typeparam>
    /// <param name="graph">Graph to add the node to.</param>
    /// <param name="val">Value when the input pin is unconnected.</param>
    /// <returns>Literal node with the specified unconnected value.</returns>
    public static LiteralNode WithValue<T>(Graph graph, T val)
    {
        LiteralNode node = new LiteralNode(graph, TypeSpecifier.FromType<T>());
        node.InputValuePin.UnconnectedValue = val;
        return node;
    }

    public override string ToString() => $"Literal - {ValuePin.PinType.Value.ShortName}";
}