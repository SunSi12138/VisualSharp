namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    IEnumerable<string> GetOrCreatePinNames(IEnumerable<OutputDataPin> pins) => pins.Select(pin => GetOrCreatePinName(pin)).ToList();
    string GetOrCreatePinName(OutputDataPin pin)
    {
        // Return the default value of the pin type if nothing is connected
        if (pin == null)
        {
            return "null";
        }

        if (variableNames.TryGetValue(pin, out var value))
        {
            return value;
        }

        string pinName;

        // Special case for property setters, input name "value".
        // TODO: Don't rely on set_ prefix
        // TODO: Use PropertyGraph instead of MethodGraph
        if (pin.Node is MethodEntryNode && graph is MethodGraph methodGraph && methodGraph.Name.StartsWith("set_"))
        {
            pinName = "value";
        }
        else
        {
            pinName = TranslatorUtil.GetUniqueVariableName(pin.Name.Replace("<", "_").Replace(">", "_"), variableNames.Values.ToList());
        }

        variableNames.Add(pin, pinName);
        return pinName;
    }

    IEnumerable<string> GetPinIncomingValues(IEnumerable<InputDataPin> pins) => pins.Select(pin => GetPinIncomingValue(pin)).ToList();
    string GetPinIncomingValue(InputDataPin pin)
    {
        if (pin.IncomingPin == null)
        {
            if (pin.UsesUnconnectedValue && pin.UnconnectedValue != null)
            {
                return TranslatorUtil.ObjectToLiteral(pin.UnconnectedValue, (TypeSpecifier)pin.PinType.Value);
            }
            else if (pin.UsesExplicitDefaultValue)
            {
                return null;
            }
            else
            {
                throw new Exception($"Input data pin {pin} on {pin.Node} was unconnected without an explicit default or unconnected value.");
                //return $"default({pin.PinType.Value.FullCodeName})";
            }
        }
        else
        {
            return GetOrCreatePinName(pin.IncomingPin);
        }
    }

    void WriteGotoOutputPinIfNecessary(OutputExecPin pin, InputExecPin fromPin)
    {
        int fromId = GetExecPinStateId(fromPin);
        int nextId = fromId + 1;

        if (pin.OutgoingPin == null)
        {
            if (nextId != jumpStackStateId)
            {
                WriteGotoJumpStack();
            }
        }
        else
        {
            int toId = GetExecPinStateId(pin.OutgoingPin);

            // Only write the goto if the next state is not
            // the state we want to go to.
            if (nextId != toId)
            {
                WriteGotoInputPin(pin.OutgoingPin);
            }
        }
    }
    
    int GetNextStateId() => nextStateId++;
    void CreateStates()
    {
        foreach(Node node in execNodes)
        {
            if (!(node is MethodEntryNode))
            {
                nodeStateIds.Add(node, new List<int>());

                foreach (InputExecPin execPin in node.InputExecPins)
                {
                    nodeStateIds[node].Add(GetNextStateId());
                }
            }
        }
    }

    void CreateVariables()
    {
        foreach(Node node in nodes)
        {
            var v = GetOrCreatePinNames(node.OutputDataPins);
        }
    }

    

    IEnumerable<string> GetOrCreateTypedPinNames(IEnumerable<OutputDataPin> pins)
    {
        return pins.Select(pin => GetOrCreateTypedPinName(pin)).ToList();
    }

    string GetOrCreateTypedPinName(OutputDataPin pin)
    {
        string pinName = GetOrCreatePinName(pin);
        return $"{pin.PinType.Value.FullCodeName} {pinName}";
    }


     void WriteGotoOutputPin(OutputExecPin pin)
    {
        if (pin.OutgoingPin == null)
        {
            WriteGotoJumpStack();
        }
        else
        {
            WriteGotoInputPin(pin.OutgoingPin);
        }
    }

    string RemoveUnnecessaryLabels(string code)
    {
        foreach (int stateId in nodeStateIds.Values.SelectMany(i => i))
        {
            if (!code.Contains($"goto State{stateId};"))
            {
                code = code.Replace($"State{stateId}:", "");
            }
        }
        return code;
    }
}