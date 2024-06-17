namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    public void PureTranslateLiteralNode(LiteralNode node)
    {
        builder.AppendLine($"{GetOrCreatePinName(node.ValuePin)} = {GetPinIncomingValue(node.InputDataPins[0])};");
    }
}