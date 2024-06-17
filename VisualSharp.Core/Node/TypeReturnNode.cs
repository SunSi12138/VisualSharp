namespace VisualSharp;


public class TypeReturnNode : Node
{
    public InputTypePin TypePin => InputTypePins[0];

    public TypeReturnNode(TypeGraph graph): base(graph)=>AddInputTypePin(NameDB.Type);
}