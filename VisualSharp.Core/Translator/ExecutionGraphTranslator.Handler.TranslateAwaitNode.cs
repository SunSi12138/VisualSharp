namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    public void TranslateAwaitNode(AwaitNode node)
    {
        if (!node.IsPure)
        {
            // Translate all the pure nodes this node depends on in
            // the correct order
            TranslateDependentPureNodes(node);
        }

        // Store result if task has a return value.
        if (node.ResultPin != null)
        {
            builder.Append($"{GetOrCreatePinName(node.ResultPin)} = ");
        }

        builder.AppendLine($"await {GetPinIncomingValue(node.TaskPin)};");
    }
}