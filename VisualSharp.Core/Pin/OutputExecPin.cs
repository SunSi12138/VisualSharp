namespace VisualSharp;

public class OutputExecPin(Node node, string name) : ExecPin(node, name)
{

    /// <summary>
    /// Event that is triggered when the outgoing pin of the node changes.
    /// </summary>
    public event OutputExecPinOutgoingPinChangedDelegate OutgoingPinChanged;

    private InputExecPin outgoingPin;

    /// <summary>
    /// Gets or sets the outgoing pin of the node.
    /// </summary>
    public InputExecPin OutgoingPin
    {
        get => outgoingPin;
        set
        {
            if (outgoingPin != value)
            {
                var oldPin = outgoingPin;

                outgoingPin = value;

                OutgoingPinChanged?.Invoke(this, oldPin, outgoingPin);
            }
        }
    }
}