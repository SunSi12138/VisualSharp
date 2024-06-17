namespace VisualSharp;

public class InputDataPin(Node node, string name, ObservableValue<BaseType> pinType) : NodeDataPin(node, name,pinType)
{
    public event InputDataPinIncomingPinChangedDelegate IncomingPinChanged;
    OutputDataPin incomingPin;
    public OutputDataPin IncomingPin
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
    /// <summary>
    /// Whether this pin uses its unconnected value to output a value
    /// when no pin is connected to it.
    public bool UsesUnconnectedValue => PinType.Value is TypeSpecifier t && t.IsPrimitive;

    private object unconnectedValue;
    /// <summary>
    /// Unconnected value of this pin when no pin is connected to it.
    /// Setting this for types that don't support unconnected values will throw
    /// an exception.
    /// </summary>
    public object UnconnectedValue
    {
        get => unconnectedValue;
        set
        {
            // Check that:
            // this pin uses the unconnected value
            // the value is of the same type or string if enum
            if(value != null && (!UsesUnconnectedValue || (PinType.Value is TypeSpecifier t && ((!t.IsEnum && TypeSpecifier.FromType(value.GetType()) != t)|| (t.IsEnum && value.GetType() != typeof(string))))))
            {
                throw new ArgumentException();
            }
            unconnectedValue = value;
        }
    }

    public object ExplicitDefaultValue { get; set; }
    // TODO： 这个是什么意思？
    public bool UsesExplicitDefaultValue { get; set; }
}