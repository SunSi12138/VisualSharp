namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    void TranslateExplicitCastNode(ExplicitCastNode node)
    {
        if (!node.IsPure)
        {
            // Translate all the pure nodes this node depends on in
            // the correct order
            TranslateDependentPureNodes(node);
        }

        // Try to cast the incoming object and go to next states.
        if (node.ObjectToCast.IncomingPin != null)
        {
            string pinToCastName = GetPinIncomingValue(node.ObjectToCast);
            string outputName = GetOrCreatePinName(node.CastPin);

            // If failure pin is not connected write explicit cast that throws.
            // Otherwise check if cast object is null and execute failure
            // path if it is.
            if (node.IsPure || node.CastFailedPin.OutgoingPin == null)
            {
                builder.AppendLine($"{outputName} = ({node.CastType.FullCodeNameUnbound}){pinToCastName};");

                if (!node.IsPure)
                {
                    WriteGotoOutputPinIfNecessary(node.CastSuccessPin, node.InputExecPins[0]);
                }
            }
            else
            {
                builder.AppendLine($"{outputName} = {pinToCastName} as {node.CastType.FullCodeNameUnbound};");

                if (!node.IsPure)
                {
                    builder.AppendLine($"if ({outputName} is null)");
                    builder.AppendLine("{");
                    WriteGotoOutputPinIfNecessary(node.CastFailedPin, node.InputExecPins[0]);
                    builder.AppendLine("}");
                    builder.AppendLine("else");
                    builder.AppendLine("{");
                    WriteGotoOutputPinIfNecessary(node.CastSuccessPin, node.InputExecPins[0]);
                    builder.AppendLine("}");
                }
            }
        }
    }
}