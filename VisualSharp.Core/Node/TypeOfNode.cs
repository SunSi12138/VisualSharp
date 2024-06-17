namespace VisualSharp;

public class TypeOfNode : Node
{
    /// <summary>
    /// Output data pin for the Type value.
    /// </summary>
    public OutputDataPin TypePin => OutputDataPins[0];

    /// <summary>
    /// Input type pin for the Type value.
    /// </summary>
    public InputTypePin InputTypePin => InputTypePins[0];

    public TypeOfNode(Graph graph)
        : base(graph)
    {
        AddInputTypePin(NameDB.Type);
        AddOutputDataPin(NameDB.Type, TypeSpecifier.FromType<Type>());
    }

    public override string ToString()=>$"Type Of";
}