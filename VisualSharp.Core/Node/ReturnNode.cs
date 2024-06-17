namespace VisualSharp;

public class ReturnNode:Node
{
    public InputExecPin ReturnPin=>InputExecPins[0];

    public ReturnNode(MethodGraph graph):base(graph)
    {
        AddInputExecPin(NameDB.Exec);
        SetupSecondaryNodeEvents();
    }

    /// <summary>
    /// Sets the data pin types to the same as the main nodes' data pin types.
    /// </summary>
    private void ReplicateMainInputTypes()
    {
        if (this == MethodGraph.MainReturnNode)
        {
            return;
        }

        // Get new return types
        InputDataPin[] mainInputPins = MethodGraph.MainReturnNode.InputDataPins.ToArray();

        var oldConnections = new Dictionary<int, OutputDataPin>();

        // Remember pins with same type as before
        foreach (InputDataPin pin in InputDataPins)
        {
            int i = InputDataPins.IndexOf(pin);
            if (i < mainInputPins.Length && pin.IncomingPin != null)
            {
                oldConnections.Add(i, pin.IncomingPin);
            }

            GraphUtil.DisconnectInputDataPin(pin);
        }

        InputDataPins.Clear();

        foreach (InputDataPin mainInputPin in mainInputPins)
        {
            AddInputDataPin(mainInputPin.Name, mainInputPin.PinType.Value);
        }

        // Restore old connections
        foreach (var oldConn in oldConnections)
        {
            GraphUtil.ConnectDataPins(oldConn.Value, InputDataPins[oldConn.Key]);
        }
    }

    /// <summary>
    /// Sets the data pin types to the input type pin types.
    /// </summary>
    private void UpdateMainInputTypes()
    {
        if (this != MethodGraph.MainReturnNode)
        {
            return;
        }

        for (int i = 0; i < InputTypePins.Count; i++)
        {
            InputDataPins[i].PinType.Value = InputTypePins[i].InferredType?.Value ?? TypeSpecifier.FromType<object>();
        }
    }

    protected override void OnInputTypeChanged(object sender)
    {
        base.OnInputTypeChanged(sender);
        UpdateMainInputTypes();
    }

    public void AddReturnType()
    {
        if (this != MethodGraph.MainReturnNode)
        {
            throw new InvalidOperationException("Can only add return types on the main return node.");
        }

        int returnIndex = InputDataPins.Count;

        AddInputDataPin($"Output{returnIndex}", new ObservableValue<BaseType>(TypeSpecifier.FromType<object>()));
        AddInputTypePin($"Output{returnIndex}Type");
    }

    public void RemoveReturnType()
    {
        if (this != MethodGraph.MainReturnNode)
        {
            throw new InvalidOperationException("Can only remove return types on the main return node.");
        }

        if (InputDataPins.Count > 0)
        {
            InputDataPin idpToRemove = InputDataPins.Last();
            InputTypePin itpToRemove = InputTypePins.Last();

            GraphUtil.DisconnectInputDataPin(idpToRemove);
            GraphUtil.DisconnectInputTypePin(itpToRemove);

            InputDataPins.Remove(idpToRemove);
            InputTypePins.Remove(itpToRemove);
        }
    }

    private void SetupSecondaryNodeEvents()
    {
        if (MethodGraph.MainReturnNode != null)
        {
            if (this == MethodGraph.MainReturnNode)
            {
                UpdateMainInputTypes();
            }
            else
            {
                MethodGraph.MainReturnNode.InputDataPins.CollectionChanged += (sender, e) => ReplicateMainInputTypes();
                MethodGraph.MainReturnNode.InputTypeChanged += (sender, e) => ReplicateMainInputTypes();
                ReplicateMainInputTypes();
            }
        }
    }

    public override string ToString()
    {
        return "Return";
    }
}