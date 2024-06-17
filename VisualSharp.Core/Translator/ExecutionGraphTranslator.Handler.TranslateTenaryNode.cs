namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    void TranslateTernaryNode(TernaryNode node)
    {
        if (!node.IsPure)
        {
            // Translate all the pure nodes this node depends on in
            // the correct order
            TranslateDependentPureNodes(node);
        }

        builder.Append($"{GetOrCreatePinName(node.OutputObjectPin)} = ");
        builder.Append($"{GetPinIncomingValue(node.ConditionPin)} ? ");
        builder.Append($"{GetPinIncomingValue(node.TrueObjectPin)} : ");
        builder.AppendLine($"{GetPinIncomingValue(node.FalseObjectPin)};");

        if (!node.IsPure)
        {
            WriteGotoOutputPinIfNecessary(node.OutputExecPins.Single(), node.InputExecPins.Single());
        }
    }
}