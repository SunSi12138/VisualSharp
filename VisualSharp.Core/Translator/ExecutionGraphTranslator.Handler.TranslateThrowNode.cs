namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    void TranslateThrowNode(ThrowNode node)
    {
        TranslateDependentPureNodes(node);
        builder.AppendLine($"throw {GetPinIncomingValue(node.ExceptionPin)};");
    }
}