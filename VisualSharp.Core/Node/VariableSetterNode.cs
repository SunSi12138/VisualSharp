namespace VisualSharp;

public class VariableSetterNode : VariableNode
{
    public InputDataPin NewValuePin=> IsStatic?InputDataPins[0]:InputDataPins[1];

    public VariableSetterNode(Graph graph, VariableSpecifier variable):base(graph,variable)
    {
        AddInputExecPin(NameDB.Exec);
        AddOutputExecPin(NameDB.Exec);

        AddInputDataPin(NameDB.NewValue,variable.Type);
    }

    public override string ToString()=> IsStatic? $"Set {TargetType.ShortName}.{VariableName}" : $"Set {VariableName}";
}