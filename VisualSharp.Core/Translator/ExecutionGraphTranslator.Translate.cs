namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    /// <summary>
    /// Translates a method to C#.
    /// </summary>
    /// <param name="graph">Execution graph to translate.</param>
    /// <param name="withSignature">Whether to translate the signature.</param>
    /// <returns>C# code for the method.</returns>
    public string Translate(ExecutionGraph graph, bool withSignature)
    {
        this.graph = graph;

        // Reset state
        variableNames.Clear();
        nodeStateIds.Clear();
        pinsJumpedTo.Clear();
        nextStateId = 0;
        builder.Clear();
        random = new Random(0);

        nodes = TranslatorUtil.GetAllNodesInExecGraph(graph);
        execNodes = TranslatorUtil.GetExecNodesInExecGraph(graph);

        // Assign a state id to every non-pure node
        CreateStates();

        // Assign jump stack state id
        // Write it later once we know which states get jumped to
        jumpStackStateId = GetNextStateId();

        // Create variables for all output pins for every node
        CreateVariables();

        // Write the signatures
        if (withSignature)
        {
            TranslateSignature();
        }

        builder.AppendLine("{"); // Method start

        // Write a placeholder for the jump stack declaration
        // Replaced later
        builder.Append("%JUMPSTACKPLACEHOLDER%");

        // Write the variable declarations
        TranslateVariables();
        builder.AppendLine();

        // Start at node after method entry if necessary (id!=0)
        if (graph.EntryNode.OutputExecPins[0].OutgoingPin != null && GetExecPinStateId(graph.EntryNode.OutputExecPins[0].OutgoingPin) != 0)
        {
            WriteGotoOutputPin(graph.EntryNode.OutputExecPins[0]);
        }

        // Translate every exec node
        foreach (Node node in execNodes)
        {
            if (!(node is MethodEntryNode))
            {
                for (int pinIndex = 0; pinIndex < node.InputExecPins.Count; pinIndex++)
                {
                    builder.AppendLine($"State{nodeStateIds[node][pinIndex]}:");
                    TranslateNode(node, pinIndex);
                    builder.AppendLine();
                }
            }
        }

        // Write the jump stack if it was ever used
        if (pinsJumpedTo.Count > 0)
        {
            TranslateJumpStack();

            builder.Replace("%JUMPSTACKPLACEHOLDER%", $"{JumpStackType} {JumpStackVarName} = new {JumpStackType}();{Environment.NewLine}");
        }
        else
        {
            builder.Replace("%JUMPSTACKPLACEHOLDER%", "");
        }

        builder.AppendLine("}"); // Method end

        string code = builder.ToString();

        // Remove unused labels
        return RemoveUnnecessaryLabels(code);
    }
}