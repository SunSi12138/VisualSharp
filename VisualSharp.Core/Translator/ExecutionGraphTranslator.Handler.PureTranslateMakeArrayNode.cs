namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    public void PureTranslateMakeArrayNode(MakeArrayNode node)
    {
        builder.Append($"{GetOrCreatePinName(node.OutputDataPins[0])} = new {node.ArrayType.FullCodeName}");

        // Use predefined size or initializer list
        if (node.UsePredefinedSize)
        {
            // HACKish: Remove trailing "[]" contained in type
            builder.Remove(builder.Length - 2, 2);
            builder.AppendLine($"[{GetPinIncomingValue(node.SizePin)}];");
        }
        else
        {
            builder.AppendLine();
            builder.AppendLine("{");

            foreach (var inputDataPin in node.InputDataPins)
            {
                builder.AppendLine($"{GetPinIncomingValue(inputDataPin)},");
            }

            builder.AppendLine("};");
        }
    }
}