namespace VisualSharp;

public class TypeNode : Node
{
    public BaseType Type { get; private set; }

    private ObservableValue<BaseType> constructedType;

    public TypeNode(Graph graph, BaseType type) : base(graph)
    {
        Type = type;

        // Add type pins for each generic argument of the literal type
        // and monitor them for changes to reconstruct the actual pin types.
        if (Type is TypeSpecifier typeSpecifier)
        {
            foreach (var genericArg in typeSpecifier.GenericArguments.OfType<GenericType>())
            {
                AddInputTypePin(genericArg.Name);
            }
        }

        constructedType = new ObservableValue<BaseType>(GetConstructedOutputType());
        AddOutputTypePin(NameDB.OutputType, constructedType);
    }

    protected override void OnInputTypeChanged(object sender)
    {
        base.OnInputTypeChanged(sender);

        // Set the type of the output type pin by constructing
        // the type of this node with the input type pins.
        constructedType.Value = GetConstructedOutputType();
    }

    private BaseType GetConstructedOutputType()=>GenericsUtil.ConstructWithTypePins(Type, InputTypePins);
    public override string ToString()=>Type.ShortName;
}