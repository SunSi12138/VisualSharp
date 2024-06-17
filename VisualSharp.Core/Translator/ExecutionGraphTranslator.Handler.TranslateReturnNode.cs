using System.Threading.Tasks;

namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    void TranslateReturnNode(ReturnNode node)
    {
        // Translate all the pure nodes this node depends on in
        // the correct order
        TranslateDependentPureNodes(node);

        if (node.InputDataPins.Count == 0)
        {
            // Only write return if the return node is not the last node
            if (GetExecPinStateId(node.InputExecPins[0]) != nodeStateIds.Count - 1)
            {
                builder.AppendLine("return;");
            }
        }
        else if (node.InputDataPins.Count == 1)
        {
            // Special case for async functions returning Task (no return value)
            if (node.InputDataPins[0].PinType == TypeSpecifier.FromType<Task>())
            {
                builder.AppendLine("return;");
            }
            else
            {
                builder.AppendLine($"return {GetPinIncomingValue(node.InputDataPins[0])};");
            }
        }
        else
        {
            var returnValues = node.InputDataPins.Select(pin => GetPinIncomingValue(pin));

            // Tuple<Types..> (won't be needed in the future)
            string returnType = typeof(Tuple).FullName + "<" + string.Join(", ", node.InputDataPins.Select(pin => pin.PinType.Value.FullCodeName)) + ">";
            builder.AppendLine($"return new {returnType}({string.Join(", ", returnValues)});");
        }
    }
}