namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    public void PureTranslateTypeOfNode(TypeOfNode node)
    {
        builder.AppendLine($"{GetOrCreatePinName(node.TypePin)} = typeof({node.InputTypePin.InferredType?.Value?.FullCodeNameUnbound ?? "System.Object"});");
    }
}