namespace VisualSharp;

/// <summary>
/// Pin which can receive types.
/// </summary>
public class InputTypePin(Node node, string name) : TypePin(node, name)
{
    public event InputTypePinIncomingPinChangedDelegate IncomingPinChanged;

    OutputTypePin incomingPin;
    
    public OutputTypePin IncomingPin
    {
        get => incomingPin;
        set
        {
            if (incomingPin != value)
            {
                var oldPin = incomingPin;
                incomingPin = value;
                IncomingPinChanged?.Invoke(this, oldPin, value);
            }
        }
    }
    public override ObservableValue<BaseType> InferredType => incomingPin?.InferredType;
}