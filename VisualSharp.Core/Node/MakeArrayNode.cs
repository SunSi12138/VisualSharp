namespace VisualSharp;

public class MakeArrayNode : Node
{
    /// <summary>
    /// Whether this node is in predefined-size
    /// or in initializer mode.
    /// </summary>
    public bool UsePredefinedSize
    {
        get => usePredefinedSize;
        set
        {
            if (usePredefinedSize != value)
            {
                usePredefinedSize = value;
                UpdateInputDataPins();
            }
        }
    }

    private bool usePredefinedSize = false;

    /// <summary>
    /// Pin that specifies the size of the array.
    /// Only used when UsePredefinedSize is true.
    /// </summary>
    public InputDataPin SizePin => InputDataPins[0];

    /// <summary>
    /// Specifier for the type of the elements of the array.
    /// </summary>
    public BaseType ElementType => ElementTypePin.InferredType?.Value ?? TypeSpecifier.FromType<object>();

    /// <summary>
    /// Input type pin for the element type of the array to create.
    /// </summary>
    public InputTypePin ElementTypePin => InputTypePins[0];

    /// <summary>
    /// Output data pin for the created array.
    /// </summary>
    public OutputDataPin ArrayPin => OutputDataPins[0];

    /// <summary>
    /// Specifier for the type of the array.
    /// </summary>
    public TypeSpecifier ArrayType
    {
        get
        {
            if (ElementType is TypeSpecifier typeSpec)
            {
                return new TypeSpecifier($"{typeSpec.Name}[]", typeSpec.IsEnum, typeSpec.IsInterface, typeSpec.GenericArguments);
            }
            else
            {
                return new TypeSpecifier($"{ElementType.Name}[]");
            }
        }
    }

    /// <summary>
    /// Creates a new node representing the creation of an array.
    /// </summary>
    /// <param name="graph">Graph the node is part of.</param>
    /// <param name="elementType">Type specifier for the elements of the array.</param>
    public MakeArrayNode(Graph graph)
        : base(graph)
    {
        AddInputTypePin(NameDB.ElementType);
        AddOutputDataPin(NameDB.Array, ArrayType);
    }

    private void UpdateInputDataPins()
    {
        if (UsePredefinedSize)
        {
            // Remove element pins
            while (InputDataPins.Count > 0)
            {
                RemoveElementPin();
            }

            AddInputDataPin(NameDB.Size, TypeSpecifier.FromType<int>());
        }
        else
        {
            // Remove size pin
            GraphUtil.DisconnectInputDataPin(InputDataPins[0]);
            InputDataPins.RemoveAt(0);
        }
    }

    private void UpdateOutputType() => ArrayPin.PinType.Value = ArrayType;

    protected override void OnInputTypeChanged(object sender)
    {
        base.OnInputTypeChanged(sender);
        UpdateOutputType();
    }

    /// <summary>
    /// Adds an input data pin for an array element.
    /// </summary>
    public void AddElementPin() => AddInputDataPin($"Element{InputDataPins.Count}", ElementType);

    /// <summary>
    /// Removes the last input data pin for an array element.
    /// Returns whether one was actually removed.
    /// </summary>
    /// <returns>Whether a pin was removed.</returns>
    public bool RemoveElementPin()
    {
        if (InputDataPins.Count > 0)
        {
            // TODO: Add method for removing pins on Node
            InputDataPin inputDataPin = InputDataPins[InputDataPins.Count - 1];
            GraphUtil.DisconnectInputDataPin(inputDataPin);
            InputDataPins.Remove(inputDataPin);

            return true;
        }

        return false;
    }

    public override string ToString() => $"Make {ElementType.Name} Array";
}