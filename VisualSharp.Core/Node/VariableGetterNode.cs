namespace VisualSharp;

public class VariableGetterNode(Graph graph, VariableSpecifier variable) : VariableNode(graph, variable)
{
    public override string ToString()=> IsStatic? $"Get {TargetType.ShortName}.{VariableName}" : $"Get {VariableName}";
}