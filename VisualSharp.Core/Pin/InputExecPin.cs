namespace VisualSharp;

/// <summary>
/// Represents an input execution pin of a node.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="InputExecPin"/> class.
/// </remarks>
/// <param name="node">The node that this input pin belongs to.</param>
/// <param name="name">The name of the input pin.</param>
public class InputExecPin(Node node, string name) : ExecPin(node, name)
{
    /// <summary>
    /// Gets the collection of incoming execution pins connected to this input pin.
    /// </summary>
    public ObservableRangeCollection<OutputExecPin> IncomingPins { get; private set; } = [];
}