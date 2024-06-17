namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    void TranslateIfElseNode(IfElseNode node)
    {
        // Translate all the pure nodes this node depends on in
        // the correct order
        TranslateDependentPureNodes(node);

        string conditionVar = GetPinIncomingValue(node.ConditionPin);

        builder.AppendLine($"if ({conditionVar})");
        builder.AppendLine("{");

        if (node.TruePin.OutgoingPin != null)
        {
            WriteGotoOutputPinIfNecessary(node.TruePin, node.InputExecPins[0]);
        }
        else
        {
            builder.AppendLine("return;");
        }

        builder.AppendLine("}");

        builder.AppendLine("else");
        builder.AppendLine("{");

        if (node.FalsePin.OutgoingPin != null)
        {
            WriteGotoOutputPinIfNecessary(node.FalsePin, node.InputExecPins[0]);
        }
        else
        {
            builder.AppendLine("return;");
        }

        builder.AppendLine("}");
    }
}