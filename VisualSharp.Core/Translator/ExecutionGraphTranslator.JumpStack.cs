namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    const string JumpStackVarName = "jumpStack";
    const string JumpStackType = "System.Collections.Generic.Stack<int>";

    readonly HashSet<InputExecPin> pinsJumpedTo = [];

    int jumpStackStateId;

    int GetExecPinStateId(InputExecPin pin) => nodeStateIds[pin.Node][pin.Node.InputExecPins.IndexOf(pin)];
    void WriteGotoInputPin(InputExecPin pin) => builder.AppendLine($"goto State{GetExecPinStateId(pin)};");
    void WriteGotoJumpStack() => builder.AppendLine($"goto State{jumpStackStateId};");
    void WritePushJumpStack(InputExecPin pin)
    {
        if (!pinsJumpedTo.Contains(pin))
        {
            pinsJumpedTo.Add(pin);
        }

        builder.AppendLine($"{JumpStackVarName}.Push({GetExecPinStateId(pin)});");
    }

    void TranslateJumpStack()
    {
        builder.AppendLine("// Jump stack");

        builder.AppendLine($"State{jumpStackStateId}:");
        builder.AppendLine($"if ({JumpStackVarName}.Count == 0) throw new System.Exception();");
        builder.AppendLine($"switch ({JumpStackVarName}.Pop())");
        builder.AppendLine("{");

        foreach (InputExecPin pin in pinsJumpedTo)
        {
            builder.AppendLine($"case {GetExecPinStateId(pin)}:");
            WriteGotoInputPin(pin);
        }

        builder.AppendLine("default:");
        builder.AppendLine("throw new System.Exception();");

        builder.AppendLine("}"); // End switch
    }
}