namespace VisualSharp;

public abstract partial class Node
{
    public event EventHandler InputTypeChanged;
    protected virtual void OnInputTypeChanged(object sender)
    {
    }

    void OnIncomingTypePinChanged(InputTypePin pin, OutputTypePin oldPin, OutputTypePin newPin)
    {
        if (oldPin?.InferredType != null)
            oldPin.InferredType.OnValueChanged -= EventInputTypeChanged;

        if (newPin?.InferredType != null)
            newPin.InferredType.OnValueChanged += EventInputTypeChanged;

        EventInputTypeChanged(this);
    }
    void EventInputTypeChanged(object sender)
    {
        OnInputTypeChanged(sender);

        // Notify others afterwards, since the above call might have updated something
        InputTypeChanged?.Invoke(sender,EventArgs.Empty);
    }
    // TODO:[ONDeserialized]
    // void OnDeserializing(StreamingContext context)
    // {
    //     foreach (var inputTypePin in InputTypePins)
    //     {
    //         if (inputTypePin.InferredType != null)
    //             inputTypePin.InferredType.OnValueChanged += EventInputTypeChanged;
    //         inputTypePin.IncomingPinChanged += OnIncomingTypePinChanged;
    //     }
    // }

    // TODO：why?
    /// <summary>
    /// Called when the containing method was deserialized.
    /// </summary>
    public virtual void OnMethodDeserialized()
    {
        // Call OnInputTypeChanged to update the types of all nodes correctly.
        OnInputTypeChanged(this);
    }
}   