namespace VisualSharp;

/// <summary>
/// Node representing a reroute node. Does nothing by itself.
/// Used for layouting in the editor.
/// </summary>
public class RerouteNode : Node
{
    public int ExecRerouteCount { get => InputExecPins.Count; }
    public int DataRerouteCount { get => InputDataPins.Count; }
    public int TypeRerouteCount { get => InputTypePins.Count; }

    private RerouteNode(Graph graph): base(graph)
    {
    }

    public static RerouteNode MakeExecution(Graph graph, int numExecs)
    {
        var node = new RerouteNode(graph);

        for (var i = 0; i < numExecs; i++)
        {
            node.AddInputExecPin($"{NameDB.Exec}{i}");
            node.AddOutputExecPin($"{NameDB.Exec}{i}");
        }

        return node;
    }

    public static RerouteNode MakeData(Graph graph, IEnumerable<Tuple<BaseType, BaseType>> dataTypes)
    {
        if (dataTypes is null)
        {
            throw new ArgumentException("dataTypes was null in RerouteNode.MakeData.");
        }

        var node = new RerouteNode(graph);

        var index = 0;
        foreach (var dataType in dataTypes)
        {
            node.AddInputDataPin($"Data{index}", dataType.Item1);
            node.AddOutputDataPin($"Data{index}", dataType.Item2);
            index++;
        }

        return node;
    }

    public static RerouteNode MakeType(Graph graph, int numTypes)
    {
        var node = new RerouteNode(graph);

        for (var i = 0; i < numTypes; i++)
        {
            node.AddInputTypePin($"Type{i}");
            node.AddOutputTypePin($"Type{i}", new ObservableValue<BaseType>(null));
        }

        return node;
    }

    private void UpdateOutputType()
    {
        if (OutputTypePins.Count > 0)
        {
            OutputTypePins[0].InferredType.Value = InputTypePins[0].InferredType?.Value;
        }
    }

    protected override void OnInputTypeChanged(object sender)
    {
        base.OnInputTypeChanged(sender);
        UpdateOutputType();
    }

    public override string ToString()=>"Reroute";
}
