namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    public void PureTranslateVariableGetterNode(VariableGetterNode node)
    {
        string valueName = GetOrCreatePinName(node.OutputDataPins[0]);

        builder.Append($"{valueName} = ");

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
        else
        {
            if (node.TargetPin?.IncomingPin != null)
            {
                string targetName = GetOrCreatePinName(node.TargetPin.IncomingPin);
                builder.Append(targetName);
            }
            else
            {
                // Default to this
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

        builder.AppendLine(";");
    }
}