namespace VisualSharp;

public class ConstructorGraph:ExecutionGraph
{
    public ConstructorGraph() => EntryNode = new ConstructorEntryNode(this);

    public override string ToString()=>Class.ToString();
}