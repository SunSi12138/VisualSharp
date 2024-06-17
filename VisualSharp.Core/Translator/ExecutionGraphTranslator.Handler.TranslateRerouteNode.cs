namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    void TranslateRerouteNode(RerouteNode node)
    {
        if (node.ExecRerouteCount + node.TypeRerouteCount + node.DataRerouteCount != 1)
        {
            throw new NotImplementedException("Only implemented reroute nodes with exactly 1 type of pin.");
        }

        if (node.DataRerouteCount == 1)
        {
            builder.AppendLine($"{GetOrCreatePinName(node.OutputDataPins[0])} = {GetPinIncomingValue(node.InputDataPins[0])};");
        }
        else if (node.ExecRerouteCount == 1)
        {
            WriteGotoOutputPinIfNecessary(node.OutputExecPins[0], node.InputExecPins[0]);
        }
    }
}