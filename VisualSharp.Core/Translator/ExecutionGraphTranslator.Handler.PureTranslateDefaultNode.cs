namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    public void PureTranslateDefaultNode(DefaultNode node)
    {
        builder.AppendLine($"{GetOrCreatePinName(node.DefaultValuePin)} = default({node.Type.FullCodeName});");
    }
}