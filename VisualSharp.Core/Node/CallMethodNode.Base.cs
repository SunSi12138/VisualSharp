namespace VisualSharp;

public partial class CallMethodNode:ExecNode
{

    public override bool CanSetPure => true;

    public MethodSpecifier MethodSpecifier {get; private set;}

    public string MethodName => MethodSpecifier.Name;

    public string BoundMethodName
    {
        get
        {
            string boundName = MethodSpecifier.Name;

            if(InputTypePins.Count>0)
            {
                boundName += $"<{string.Join(",",InputTypePins.Select(p=>p.InferredType?.Value?.FullCodeName??p.Name))}>";
            }

            return boundName;
        }
    }

    public bool IsStatic=>MethodSpecifier.Modifiers.HasFlag(MethodModifier.Static);

    public TypeSpecifier DeclaringType => MethodSpecifier.DeclaringType;

    public IReadOnlyList<BaseType> ArgumentTypes=>InputDataPins.Select(p => p.PinType.Value).ToList();

    public IReadOnlyList<BaseType> ReturnTypes => OutputDataPins.Select(p => p.PinType.Value).ToList();

    public InputDataPin TargetPin=>InputDataPins[0];

    public OutputDataPin ExceptionPin => OutputDataPins.SingleOrDefault(p => p.Name == NameDB.Exception);

    public OutputExecPin CatchPin => OutputExecPins.SingleOrDefault(p=>p.Name ==NameDB.Catch);

    public bool HandlesExceptions => !IsPure && OutputExecPins.Any(p=>p.Name==NameDB.Catch) && CatchPin.OutgoingPin != null;

    /// <summary>
    /// List of node pins, one for each argument the method takes.
    /// if is not static, the first pin is the target object. So skip it.
    /// </summary>
    public IList<InputDataPin> ArgumentPins => IsStatic? InputDataPins:InputDataPins.Skip(1).ToList();

    public IList<OutputDataPin> ReturnValuePins => OutputDataPins.Where(p=>p.Name!=NameDB.Exception).ToList();

    public CallMethodNode(Graph graph, MethodSpecifier methodSpecifier,IList<BaseType> genericArgumentTyps = null):base(graph)
    {
        MethodSpecifier = methodSpecifier;

        foreach(var genericArg in MethodSpecifier.GenericArguments.OfType<GenericType>())
        {
            AddInputTypePin(genericArg.Name);
        }

        if(!IsStatic) AddInputDataPin(NameDB.Target,DeclaringType);

        AddExceptionPins();

        foreach(var argument in MethodSpecifier.Parameters)
        {
            AddInputDataPin(argument.Name,argument.Value);

            if(argument.HasExplicitDefaultValue)
            {
                var newPin = InputDataPins.Last();
                newPin.UsesExplicitDefaultValue = true;
                newPin.ExplicitDefaultValue = argument.ExplicitDefaultValue;
            }

        }

        foreach(var returnType in MethodSpecifier.ReturnTypes)
        {
            AddOutputDataPin(returnType.ShortName,returnType);
        }

        UpdateTypes();
    }
}