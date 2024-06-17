namespace VisualSharp;

public partial class CallMethodNode
{
    void AddExceptionPins()
    {
        AddOutputExecPin(NameDB.Catch);
        AddCathPinChangeEvent();
    }

    void AddCathPinChangeEvent()
    {
        if (CatchPin != null)
        {
            // Add / remove exception pin when catch is connected / unconnected
            CatchPin.OutgoingPinChanged += (pin, oldPin, newPin) => UpdateExceptionPin();
        }
    }
    
    /// <summary>
    /// Adds or removes the exception output data pin depending on
    /// whether the catch pin is connected.
    /// </summary>
    void UpdateExceptionPin()
    {
        if (CatchPin?.OutgoingPin == null && ExceptionPin != null)
        {
            GraphUtil.DisconnectOutputDataPin(ExceptionPin);
            OutputDataPins.Remove(ExceptionPin);
        }
        else if (CatchPin?.OutgoingPin != null && ExceptionPin == null)
        {
            AddOutputDataPin(NameDB.Exception, TypeSpecifier.FromType<Exception>());
        }
    }

    protected override void SetPurity(bool value)
    {
        base.SetPurity(value);

        if(value)
        {
            // Exception pin will automatically be removed if catch pin is disconnected
            GraphUtil.DisconnectOutputExecPin(CatchPin);
            OutputExecPins.Remove(CatchPin);
        }
        else
        {
            AddExceptionPins();
        }
    }

    void UpdateTypes()
    {
        for(int i = 0; i < MethodSpecifier.Parameters.Count;i++)
        {
            var type = MethodSpecifier.Parameters[i];

            var constructedType = GenericsUtil.ConstructWithTypePins(type, InputTypePins);

            if(ArgumentPins[i].PinType.Value != constructedType)
            {
                ArgumentPins[i].PinType.Value = constructedType;
            }
        }

        for(int i = 0; i< MethodSpecifier.ReturnTypes.Count;i++)
        {
            var type = MethodSpecifier.ReturnTypes[i];

            var constructedType = GenericsUtil.ConstructWithTypePins(type, InputTypePins);

            if(ReturnValuePins[i].PinType.Value != constructedType)
            {
                ReturnValuePins[i].PinType.Value = constructedType;
            }
        }
    }

    public override string ToString()
    {
        if (OperatorUtil.TryGetOperatorInfo(MethodSpecifier, out OperatorInfo operatorInfo))
        {
            return $"Operator {operatorInfo.DisplayName}";
        }
        else
        {
            string s = "";

            if (IsStatic)
            {
                s += $"{MethodSpecifier.DeclaringType.ShortName}.";
            }

            return s + MethodSpecifier.Name;
        }
    }
}