using System.Diagnostics;

namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    void TranslateConstructorNode(ConstructorNode node)
    {
        if (!node.IsPure)
        {
            // Translate all the pure nodes this node depends on in
            // the correct order
            TranslateDependentPureNodes(node);
        }

        // Write assignment and constructor
        string returnName = GetOrCreatePinName(node.OutputDataPins[0]);
        builder.Append($"{returnName} = new {node.ClassType}");

        // Write constructor arguments
        var argumentNames = GetPinIncomingValues(node.ArgumentPins);
        //builder.AppendLine($"({string.Join(", ", argumentNames)});");

        string[] argNameArray = argumentNames.ToArray();
        Debug.Assert(argNameArray.Length == node.ConstructorSpecifier.Arguments.Count);

        bool prependArgumentName = argNameArray.Any(a => a is null);

        List<string> arguments = new List<string>();

        foreach ((var argName, var constructorParameter) in argNameArray.Zip(node.ConstructorSpecifier.Arguments, Tuple.Create))
        {
            // null means use default value
            if (!(argName is null))
            {
                string argument = argName;

                // Prepend with argument name if wanted
                if (prependArgumentName)
                {
                    argument = $"{constructorParameter.Name}: {argument}";
                }

                // Prefix with "out" / "ref" / "in"
                switch (constructorParameter.PassType)
                {
                    case MethodParameterPassType.Out:
                        argument = "out " + argument;
                        break;
                    case MethodParameterPassType.Reference:
                        argument = "ref " + argument;
                        break;
                    case MethodParameterPassType.In:
                        // Don't pass with in as it could break implicit casts.
                        // argument = "in " + argument;
                        break;
                    default:
                        break;
                }

                arguments.Add(argument);
            }
        }

        // Write the method call
        builder.AppendLine($"({string.Join(", ", arguments)});");

        if (!node.IsPure)
        {
            // Go to the next state
            WriteGotoOutputPinIfNecessary(node.OutputExecPins[0], node.InputExecPins[0]);
        }
    }
}