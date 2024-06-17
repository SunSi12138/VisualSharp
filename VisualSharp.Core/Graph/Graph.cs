namespace VisualSharp;

public abstract class Graph
{
    /// <summary>
    /// Collection of nodes in this graph.
    /// </summary>
    public ObservableRangeCollection<Node> Nodes { get; private set; } = [];
    /// <summary>
    /// Class this graph is contained in.
    /// </summary>
    public ClassGraph Class { get; set; }
    /// <summary>
    /// Project the graph is part of.
    /// </summary>
    public Project Project { get; set; }
}
