namespace VisualSharp;

public class MethodGraph:ExecutionGraph
{
    public IEnumerable<ReturnNode> returnNodes=>Nodes.OfType<ReturnNode>();

    public ReturnNode MainReturnNode=>Nodes.OfType<ReturnNode>()?.FirstOrDefault();

    public IEnumerable<BaseType> ReturnTypes=>MainReturnNode?.InputTypePins?.Select(pin => pin.InferredType?.Value ?? TypeSpecifier.FromType<object>())?.ToList() ?? new List<BaseType>();

    /// <summary>
    /// Generic type arguments of the method.
    /// </summary>
    public IEnumerable<GenericType> GenericArgumentTypes
    {
        get => EntryNode != null ? EntryNode.OutputTypePins.Select(pin => pin.InferredType.Value).Cast<GenericType>().ToList() : new List<GenericType>();
    }

    public string Name {get;set;}

    public MethodModifier Modifiers {get;set;} = MethodModifier.None;

    public MethodEntryNode methodEntryNode=> EntryNode as MethodEntryNode;

    public MethodGraph(string name)
    {
        Name = name;
        EntryNode = new MethodEntryNode(this);
        // 这里的配置在构造函数里进行
        new ReturnNode(this);
    }

    public override string ToString()=>Name;
}