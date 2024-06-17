namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    void TranslateVariableSetterNode(VariableSetterNode node)
    {
        // Translate all the pure nodes this node depends on in
        // the correct order
        TranslateDependentPureNodes(node);

        string valueName = GetPinIncomingValue(node.NewValuePin);

        // Add target name if there is a target (null for local and static variables)
        if (node.IsStatic)
        {
            if (!(node.TargetType is null))
            {
                builder.Append(node.TargetType.FullCodeName);
            }
            else
            {
                builder.Append(node.Graph.Class.Name);
            }
        }
        if (node.TargetPin != null)
        {
            if (node.TargetPin.IncomingPin != null)
            {
                string targetName = GetOrCreatePinName(node.TargetPin.IncomingPin);
                builder.Append(targetName);
            }
            else
            {
                builder.Append("this");
            }
        }

        // Add index if needed
        if (node.IsIndexer)
        {
            builder.Append($"[{GetPinIncomingValue(node.IndexPin)}]");
        }
        else
        {
            builder.Append($".{node.VariableName}");
        }

        builder.AppendLine($" = {valueName};");

        // Set output pin of this node to the same value
        builder.AppendLine($"{GetOrCreatePinName(node.OutputDataPins[0])} = {valueName};");

        // Go to the next state
        WriteGotoOutputPinIfNecessary(node.OutputExecPins[0], node.InputExecPins[0]);
    }
}