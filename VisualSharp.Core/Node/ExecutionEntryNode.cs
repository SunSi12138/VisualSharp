namespace VisualSharp;

public abstract class ExecutionEntryNode(ExecutionGraph graph) : Node(graph) 
{
    public OutputExecPin InitialExecutionPin=>OutputExecPins[0];
}