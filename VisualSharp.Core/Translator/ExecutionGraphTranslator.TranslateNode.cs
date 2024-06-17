using System.Diagnostics;
using System.Text;

namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    void TranslateNode(Node node, int pinIndex)
    {
        if (!(node is RerouteNode))
        {
            builder.AppendLine($"// {node}");
        }

        if (nodeTypeHandlers.ContainsKey(node.GetType()))
        {
            nodeTypeHandlers[node.GetType()][pinIndex](this, node);
        }
        else
        {
            Debug.WriteLine($"Unhandled type {node.GetType()} in TranslateNode");
        }
    }
}