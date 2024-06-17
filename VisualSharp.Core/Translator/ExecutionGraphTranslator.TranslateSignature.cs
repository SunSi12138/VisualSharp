namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    void TranslateSignature()
    {
        builder.AppendLine($"// {graph}");

        // Write visibility
        builder.Append($"{TranslatorUtil.VisibilityTokens[graph.Visibility]} ");

        MethodGraph methodGraph = graph as MethodGraph;

        if (methodGraph != null)
        {
            // Write modifiers
            if (methodGraph.Modifiers.HasFlag(MethodModifier.Async))
            {
                builder.Append("async ");
            }

            if (methodGraph.Modifiers.HasFlag(MethodModifier.Static))
            {
                builder.Append("static ");
            }

            if (methodGraph.Modifiers.HasFlag(MethodModifier.Abstract))
            {
                builder.Append("abstract ");
            }

            if (methodGraph.Modifiers.HasFlag(MethodModifier.Sealed))
            {
                builder.Append("sealed ");
            }

            if (methodGraph.Modifiers.HasFlag(MethodModifier.Override))
            {
                builder.Append("override ");
            }
            else if (methodGraph.Modifiers.HasFlag(MethodModifier.Virtual))
            {
                builder.Append("virtual ");
            }

            // Write return type
            if (methodGraph.ReturnTypes.Count() > 1)
            {
                // Tuple<Types..> (won't be needed in the future)
                string returnType = typeof(Tuple).FullName + "<" + string.Join(", ", methodGraph.ReturnTypes.Select(t => t.FullCodeName)) + ">";
                builder.Append(returnType + " ");

                //builder.Append($"({string.Join(", ", method.ReturnTypes.Select(t => t.FullName))}) ");
            }
            else if (methodGraph.ReturnTypes.Count() == 1)
            {
                builder.Append($"{methodGraph.ReturnTypes.Single().FullCodeName} ");
            }
            else
            {
                builder.Append("void ");
            }
        }

        // Write name
        builder.Append(graph.ToString());

        if (methodGraph != null)
        {
            // Write generic arguments if any
            if (methodGraph.GenericArgumentTypes.Any())
            {
                builder.Append("<" + string.Join(", ", methodGraph.GenericArgumentTypes.Select(arg => arg.FullCodeName)) + ">");
            }
        }

        // Write parameters
        builder.AppendLine($"({string.Join(", ", GetOrCreateTypedPinNames(graph.EntryNode.OutputDataPins))})");
    }
}