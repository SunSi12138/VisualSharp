namespace VisualSharp;

/// <summary>
/// Node representing an if / else expression.
/// </summary>
public class IfElseNode : Node
{
    /// <summary>
    /// Pin that gets executed if the statement was true.
    /// </summary>
    public OutputExecPin TruePin => OutputExecPins[0];

    /// <summary>
    /// Pin that gets executed if the statement was false.
    /// </summary>
    public OutputExecPin FalsePin => OutputExecPins[1];

    /// <summary>
    /// Input execution pin that executes this node.
    /// </summary>
    public InputExecPin ExecutionPin => InputExecPins[0];

    /// <summary>
    /// Input data pin for the condition of this node.
    /// Expects a boolean value.
    /// </summary>
    public InputDataPin ConditionPin => InputDataPins[0];

    public IfElseNode(Graph graph)
        : base(graph)
    {
        AddInputExecPin(NameDB.Exec);

        AddInputDataPin(NameDB.Condition, TypeSpecifier.FromType<bool>());

        AddOutputExecPin(NameDB.True);
        AddOutputExecPin(NameDB.False);
    }

    public override string ToString() => "If Else";
}