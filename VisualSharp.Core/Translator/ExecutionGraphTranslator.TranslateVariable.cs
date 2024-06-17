namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    void TranslateVariables()
    {
        builder.AppendLine("// Variables");

        foreach (var v in variableNames)
        {
            OutputDataPin pin = v.Key;
            string variableName = v.Value;

            if (!(pin.Node is MethodEntryNode))
            {
                builder.AppendLine($"{pin.PinType.Value.FullCodeName} {variableName} = default({pin.PinType.Value.FullCodeName});");
            }
        }
    }
}