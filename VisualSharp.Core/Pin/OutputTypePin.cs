namespace VisualSharp;

public class OutputTypePin(Node node, string name, ObservableValue<BaseType> outputType) : TypePin(node, name)
{
    public ObservableRangeCollection<InputTypePin> OutgoingPins { get; private set; } = [];

    public override ObservableValue<BaseType> InferredType => outputType;

    public override string ToString()=> outputType.Value?.ShortName?? "None";
}