namespace VisualSharp;

public class ConstructorEntryNode : ExecutionEntryNode
{
    public ConstructorGraph ConstructorGraph => (ConstructorGraph)Graph;

    public ConstructorEntryNode(ConstructorGraph constructor): base(constructor) => AddOutputExecPin(NameDB.Exec);// TODO: Add output data and type pins for constructor graph

    public override string ToString() => $"{ConstructorGraph.Class.Name} Constructor Entry";
}