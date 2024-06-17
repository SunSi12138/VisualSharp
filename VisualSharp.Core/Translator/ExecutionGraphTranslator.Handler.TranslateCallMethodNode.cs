

using System.Diagnostics;

namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    void TranslateCallMethodNode(CallMethodNode node)
    {
        // Wrap in try / catch
        if (node.HandlesExceptions)
        {
            builder.AppendLine("try");
            builder.AppendLine("{");
        }

        string temporaryReturnName = null;

        if (!node.IsPure)
        {
            // Translate all the pure nodes this node depends on in the correct order
            TranslateDependentPureNodes(node);
        }

        // Write assignment of return values
        if (node.ReturnValuePins.Count == 1)
        {
            string returnName = GetOrCreatePinName(node.ReturnValuePins[0]);

            builder.Append($"{returnName} = ");
        }
        else if (node.ReturnValuePins.Count > 1)
        {
            temporaryReturnName = TranslatorUtil.GetTemporaryVariableName(random);

            var returnTypeNames = string.Join(", ", node.ReturnValuePins.Select(pin => pin.PinType.Value.FullCodeName));

            builder.Append($"{typeof(Tuple).FullName}<{returnTypeNames}> {temporaryReturnName} = ");
        }

        // Get arguments for method call
        var argumentNames = GetPinIncomingValues(node.ArgumentPins);

        // Check whether the method is an operator and we need to translate its name
        // into operator symbols. Otherwise just call the method normally.
        if (OperatorUtil.TryGetOperatorInfo(node.MethodSpecifier, out OperatorInfo operatorInfo))
        {
            Debug.Assert(!argumentNames.Any(a => a is null));

            if (operatorInfo.Unary)
            {
                if (argumentNames.Count() != 1)
                {
                    throw new Exception($"Unary operator was found but did not have one argument: {node.MethodName}");
                }

                if (operatorInfo.UnaryRightPosition)
                {
                    builder.AppendLine($"{argumentNames.ElementAt(0)}{operatorInfo.Symbol};");
                }
                else
                {
                    builder.AppendLine($"{operatorInfo.Symbol}{argumentNames.ElementAt(0)};");
                }
            }
            else
            {
                if (argumentNames.Count() != 2)
                {
                    throw new Exception($"Binary operator was found but did not have two arguments: {node.MethodName}");
                }

                builder.AppendLine($"{argumentNames.ElementAt(0)}{operatorInfo.Symbol}{argumentNames.ElementAt(1)};");
            }
        }
        else
        {
            // Static: Write class name / target, default to own class name
            // Instance: Write target, default to this

            if (node.IsStatic)
            {
                builder.Append($"{node.DeclaringType.FullCodeName}.");
            }
            else
            {
                if (node.TargetPin.IncomingPin != null)
                {
                    string targetName = GetOrCreatePinName(node.TargetPin.IncomingPin);
                    builder.Append($"{targetName}.");
                }
                else
                {
                    // Default to this
                    builder.Append("this.");
                }
            }

            string[] argNameArray = argumentNames.ToArray();
            Debug.Assert(argNameArray.Length == node.MethodSpecifier.Parameters.Count);

            bool prependArgumentName = argNameArray.Any(a => a is null);

            List<string> arguments = new List<string>();

            foreach ((var argName, var methodParameter) in argNameArray.Zip(node.MethodSpecifier.Parameters, Tuple.Create))
            {
                // null means use default value
                if (!(argName is null))
                {
                    string argument = argName;

                    // Prepend with argument name if wanted
                    if (prependArgumentName)
                    {
                        argument = $"{methodParameter.Name}: {argument}";
                    }

                    // Prefix with "out" / "ref" / "in"
                    switch (methodParameter.PassType)
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
            builder.AppendLine($"{node.BoundMethodName}({string.Join(", ", arguments)});");
        }
        

        // Assign the real variables from the temporary tuple
        if (node.ReturnValuePins.Count > 1)
        {
            var returnNames = GetOrCreatePinNames(node.ReturnValuePins);
            for(int i = 0; i < returnNames.Count(); i++)
            {
                builder.AppendLine($"{returnNames.ElementAt(i)} = {temporaryReturnName}.Item{i+1};");
            }
        }

        // Set the exception to null on success if catch pin is connected
        if (node.HandlesExceptions)
        {
            builder.AppendLine($"{GetOrCreatePinName(node.ExceptionPin)} = null;");
        }

        // Go to the next state
        if (!node.IsPure)
        {
            WriteGotoOutputPinIfNecessary(node.OutputExecPins[0], node.InputExecPins[0]);
        }

        // Catch exceptions if catch pin is connected
        if (node.HandlesExceptions)
        {
            string exceptionVarName = TranslatorUtil.GetTemporaryVariableName(random);
            builder.AppendLine("}");
            builder.AppendLine($"catch (System.Exception {exceptionVarName})");
            builder.AppendLine("{");
            builder.AppendLine($"{GetOrCreatePinName(node.ExceptionPin)} = {exceptionVarName};");

            // Set all return values to default on exception
            foreach (var returnValuePin in node.ReturnValuePins)
            {
                string returnName = GetOrCreatePinName(returnValuePin);
                builder.AppendLine($"{returnName} = default({returnValuePin.PinType.Value.FullCodeName});");
            }

            if (!node.IsPure)
            {
                WriteGotoOutputPinIfNecessary(node.CatchPin, node.InputExecPins[0]);
            }

            builder.AppendLine("}");
        }
    }
}