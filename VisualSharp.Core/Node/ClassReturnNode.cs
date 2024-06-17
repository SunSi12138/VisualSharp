namespace VisualSharp;

public class ClassReturnNode:Node
{
    public InputTypePin SuperTypePin=>InputTypePins[0];

    public IEnumerable<InputTypePin> InterfacePins=>InputTypePins.Skip(1);

    public ClassReturnNode(ClassGraph graph): base(graph)
    {
        AddInputTypePin(NameDB.BaseType);
    }

    public void AddInterfacePin()
    {
        AddInputTypePin($"Interface{InputTypePins.Count}");
    }

    public void RemoveInterfacePin()
    {
        var interfacePin = InterfacePins.LastOrDefault();

        if(interfacePin is not null)
        {
            GraphUtil.DisconnectInputTypePin(interfacePin);
            InputTypePins.Remove(interfacePin);
        }
    }
}