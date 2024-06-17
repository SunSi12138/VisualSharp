namespace VisualSharp;

/// <summary>
/// Node representing a constructor call.
/// </summary>
public class ConstructorNode : ExecNode
{
    public override bool CanSetPure => true;

    /// <summary>
    /// Specifier for the constructor.
    /// </summary>
    public ConstructorSpecifier ConstructorSpecifier { get; private set; }

    /// <summary>
    /// Specifier for the type this constructor creates.
    /// </summary>
    public BaseType ClassType => OutputDataPins[0].PinType.Value;

    /// <summary>
    /// List of type specifiers the constructor takes.
    /// </summary>
    public IReadOnlyList<BaseType> ArgumentTypes => ArgumentPins.Select(p => p.PinType.Value).ToList();

    /// <summary>
    /// List of node pins, one for each argument the constructor takes.
    /// </summary>
    public IList<InputDataPin> ArgumentPins => InputDataPins;

    public ConstructorNode(Graph graph, ConstructorSpecifier specifier)
        : base(graph)
    {
        ConstructorSpecifier = specifier;

        // Add type pins for each generic arguments of the type being constructed.
        foreach (var genericArg in ConstructorSpecifier.DeclaringType.GenericArguments.OfType<GenericType>())
        {
            AddInputTypePin(genericArg.Name);
        }

        foreach (Named<BaseType> argument in ConstructorSpecifier.Arguments)
        {
            AddInputDataPin(argument.Name, argument.Value);
        }

        AddOutputDataPin(ConstructorSpecifier.DeclaringType.ShortName, ConstructorSpecifier.DeclaringType);

        // TODO: Set the correct types to begin with.
        UpdateTypes();
    }

    protected override void OnInputTypeChanged(object sender)
    {
        base.OnInputTypeChanged(sender);
        UpdateTypes();
    }

    private void UpdateTypes()
    {
        // Construct data input
        for (int i = 0; i < ConstructorSpecifier.Arguments.Count; i++)
        {
            BaseType type = ConstructorSpecifier.Arguments[i];

            // Construct type with generic arguments replaced by our input type pins
            BaseType constructedType = GenericsUtil.ConstructWithTypePins(type, InputTypePins);

            if (InputDataPins[i].PinType.Value != constructedType)
            {
                InputDataPins[i].PinType.Value = constructedType;
            }
        }

        // Construct data output
        {
            BaseType constructedType = GenericsUtil.ConstructWithTypePins(ConstructorSpecifier.DeclaringType, InputTypePins);
            OutputDataPins[0].PinType.Value = constructedType;
        }
    }

    public override string ToString() => $"Construct {ClassType.ShortName}";
}