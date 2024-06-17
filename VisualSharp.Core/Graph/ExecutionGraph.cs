namespace VisualSharp;

public abstract class ExecutionGraph:Graph
{
    public ExecutionEntryNode EntryNode {get; protected set;}
    
    public IEnumerable<BaseType> ArgumentTypes=>EntryNode is null ?[]:EntryNode.InputTypePins.Select(pin=>pin.InferredType?.Value?? TypeSpecifier.FromType<object[]>()).ToList();

    public IEnumerable<Named<BaseType>> NamedArgumentTypes=>EntryNode is null?[]:EntryNode.InputTypePins.Zip(EntryNode.OutputDataPins,(type,data)=>(type,data)).Select(pair => new Named<BaseType>(pair.data.Name, pair.type.InferredType?.Value ?? TypeSpecifier.FromType<object>())).ToList();

    public MemberVisibility Visibility {get;set;} = MemberVisibility.Private;
}