namespace VisualSharp;
public delegate void InputTypePinIncomingPinChangedDelegate(InputTypePin pin,   OutputTypePin oldPin, OutputTypePin newPin);
public delegate void InputDataPinIncomingPinChangedDelegate(InputDataPin pin,   OutputDataPin oldPin, OutputDataPin newPin);
public delegate void OutputExecPinOutgoingPinChangedDelegate(OutputExecPin pin, InputExecPin oldPin,  InputExecPin newPin);