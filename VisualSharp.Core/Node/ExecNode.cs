namespace VisualSharp;

public abstract class ExecNode: Node
{
    protected ExecNode(Graph graph):base(graph)
    {
        AddExecPins();

    }

    void AddExecPins()
    {
        AddInputExecPin(NameDB.Exec);
        AddOutputExecPin(NameDB.Exec);
    }

    protected override void SetPurity(bool value)
    {
        if(value)
        {
            GraphUtil.DisconnectInputExecPin(InputExecPins[0]);
            InputExecPins.RemoveAt(0);

            GraphUtil.DisconnectOutputExecPin(OutputExecPins[0]);
            OutputExecPins.RemoveAt(0);
        }
        else
        {
            AddExecPins();
        }
    }
}