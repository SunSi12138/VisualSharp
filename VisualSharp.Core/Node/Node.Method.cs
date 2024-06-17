namespace VisualSharp;


public abstract partial class Node
{
    protected void AddInputDataPin(string pinName, ObservableValue<BaseType> pinType)=>InputDataPins.Add(new InputDataPin(this, pinName, pinType));

    protected void AddOutputDataPin(string pinName, ObservableValue<BaseType> pinType)=>OutputDataPins.Add(new OutputDataPin(this,pinName, pinType));

    protected void AddInputExecPin(string pinName)=>InputExecPins.Add(new InputExecPin(this, pinName));

    protected void AddOutputExecPin(string pinName)=>OutputExecPins.Add(new OutputExecPin(this,pinName));

    protected void AddInputTypePin(string pinName)=>InputTypePins.Add(new InputTypePin(this, pinName));

    protected void AddOutputTypePin(string pinName,ObservableValue<BaseType> pinType)=>OutputTypePins.Add(new OutputTypePin(this,pinName,pinType));

}