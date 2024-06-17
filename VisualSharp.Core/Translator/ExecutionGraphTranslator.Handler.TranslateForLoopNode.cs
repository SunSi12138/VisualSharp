namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    void TranslateStartForLoopNode(ForLoopNode node)
    {
        // Translate all the pure nodes this node depends on in
        // the correct order
        TranslateDependentPureNodes(node);

        builder.AppendLine($"{GetOrCreatePinName(node.IndexPin)} = {GetPinIncomingValue(node.InitialIndexPin)};");
        builder.AppendLine($"if ({GetOrCreatePinName(node.IndexPin)} < {GetPinIncomingValue(node.MaxIndexPin)})");
        builder.AppendLine("{");
        WritePushJumpStack(node.ContinuePin);
        WriteGotoOutputPinIfNecessary(node.LoopPin, node.ExecutionPin);
        builder.AppendLine("}");
    }

    void TranslateContinueForLoopNode(ForLoopNode node)
    {
        // Translate all the pure nodes this node depends on in
        // the correct order
        TranslateDependentPureNodes(node);

        builder.AppendLine($"{GetOrCreatePinName(node.IndexPin)}++;");
        builder.AppendLine($"if ({GetOrCreatePinName(node.IndexPin)} < {GetPinIncomingValue(node.MaxIndexPin)})");
        builder.AppendLine("{");
        WritePushJumpStack(node.ContinuePin);
        WriteGotoOutputPinIfNecessary(node.LoopPin, node.ContinuePin);
        builder.AppendLine("}");

        WriteGotoOutputPinIfNecessary(node.CompletedPin, node.ContinuePin);
    }
}