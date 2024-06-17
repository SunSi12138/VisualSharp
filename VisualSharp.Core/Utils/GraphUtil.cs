namespace VisualSharp;

public static class GraphUtil
{
    /// <summary>
        /// Connects two  execution pins. Removes any previous connection.
        /// </summary>
        /// <param name="fromPin">Output execution pin to connect.</param>
        /// <param name="toPin">Input execution pin to connect.</param>
        public static void ConnectExecPins(OutputExecPin fromPin, InputExecPin toPin)
        {
            // Remove from old pin if any
            if (fromPin.OutgoingPin != null)
            {
                fromPin.OutgoingPin.IncomingPins.Remove(fromPin);
            }

            fromPin.OutgoingPin = toPin;
            toPin.IncomingPins.Add(fromPin);
        }

        /// <summary>
        /// Connects two  data pins. Removes any previous connection.
        /// </summary>
        /// <param name="fromPin">Output data pin to connect.</param>
        /// <param name="toPin">Input data pin to connect.</param>
        public static void ConnectDataPins(OutputDataPin fromPin, InputDataPin toPin)
        {
            // Remove from old pin if any
            if (toPin.IncomingPin != null)
            {
                toPin.IncomingPin.OutgoingPins.Remove(toPin);
            }

            fromPin.OutgoingPins.Add(toPin);
            toPin.IncomingPin = fromPin;
        }

        /// <summary>
        /// Connects two  type pins. Removes any previous connection.
        /// </summary>
        /// <param name="fromPin">Output type pin to connect.</param>
        /// <param name="toPin">Input type pin to connect.</param>
        public static void ConnectTypePins(OutputTypePin fromPin, InputTypePin toPin)
        {
            // Remove from old pin if any
            if (toPin.IncomingPin != null)
            {
                toPin.IncomingPin.OutgoingPins.Remove(toPin);
            }

            fromPin.OutgoingPins.Add(toPin);
            toPin.IncomingPin = fromPin;
        }
    public static void DisconnectInputDataPin(InputDataPin pin)
    {
        pin.IncomingPin?.OutgoingPins.Remove(pin);
        pin.IncomingPin = null;
    }

    public static void DisconnectOutputDataPin(OutputDataPin pin)
    {
        foreach(InputDataPin outgoingPin in pin.OutgoingPins)
        {
            outgoingPin.IncomingPin = null;
        }

        pin.OutgoingPins.Clear();
    }

    public static void DisconnectInputTypePin(InputTypePin pin)
    {
        pin.IncomingPin?.OutgoingPins.Remove(pin);
        pin.IncomingPin = null;
    }

    public static void DisconnectOutputTypePin(OutputTypePin pin)
    {
        foreach (InputTypePin outgoingPin in pin.OutgoingPins)
        {
            outgoingPin.IncomingPin = null;
        }

        pin.OutgoingPins.Clear();
    }

    public static void DisconnectOutputExecPin(OutputExecPin pin)
    {
        pin.OutgoingPin?.IncomingPins.Remove(pin);
        pin.OutgoingPin = null;
    }

    public static void DisconnectInputExecPin(InputExecPin pin)
    {
        foreach (OutputExecPin incomingPin in pin.IncomingPins)
        {
            incomingPin.OutgoingPin = null;
        }

        pin.IncomingPins.Clear();
    }

    
    /// <summary>
    /// Creates a type node for the given type. Also recursively
    /// creates any type nodes it takes as generic arguments and
    /// connects them.
    /// </summary>
    /// <param name="graph">Graph to add the type nodes to.</param>
    /// <param name="type">Specifier for the type the type node should output.</param>
    /// <param name="x">X position of the created type node.</param>
    /// <param name="y">Y position of the created type node.</param>
    /// <returns>Type node outputting the given type.</returns>
    public static TypeNode CreateNestedTypeNode(Graph graph, BaseType type, double x, double y)
    {
        const double offsetX = -308;
        const double offsetY = -112;

        var typeNode = new TypeNode(graph, type)
        {
            PositionX = x,
            PositionY = y,
        };

        // Create nodes for the type's generic arguments and connect
        // them to it.
        if (type is TypeSpecifier typeSpecifier)
        {
            IEnumerable<TypeNode> genericArgNodes = typeSpecifier.GenericArguments.Select(arg => CreateNestedTypeNode(graph, arg, x + offsetX, y + offsetY * (typeSpecifier.GenericArguments.IndexOf(arg) + 1)));

            foreach (TypeNode genericArgNode in genericArgNodes)
            {
                GraphUtil.ConnectTypePins(genericArgNode.OutputTypePins[0], typeNode.InputTypePins[0]);
            }
        }
        
        return typeNode;
    }
}