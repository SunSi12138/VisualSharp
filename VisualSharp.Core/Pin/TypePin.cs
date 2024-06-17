namespace VisualSharp;

public abstract class TypePin(Node node, string name) : Pin(node, name)
{
    public abstract ObservableValue<BaseType> InferredType { get; }
}