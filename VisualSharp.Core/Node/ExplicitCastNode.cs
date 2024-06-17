namespace VisualSharp;
/// <summary>
/// Node representing an explicit type cast.
/// </summary>
public class ExplicitCastNode : Node
{
    public override bool CanSetPure => true;

    /// <summary>
    /// Pin for the object to cast to another type.
    /// </summary>
    public InputDataPin ObjectToCast => InputDataPins[0];

    /// <summary>
    /// Input type pin for the type to cast to.
    /// </summary>
    public InputTypePin CastTypePin => InputTypePins[0];

    /// <summary>
    /// Pin that holds the cast object.
    /// </summary>
    public OutputDataPin CastPin => OutputDataPins[0];

    /// <summary>
    /// Pin that gets executed when the cast succeeded.
    /// </summary>
    public OutputExecPin CastSuccessPin => OutputExecPins[0];

    /// <summary>
    /// Pin that gets executed when the cast failed.
    /// </summary>
    public OutputExecPin CastFailedPin => OutputExecPins[1];

    /// <summary>
    /// Type to cast to. Inferred from input type pin.
    /// </summary>
    public BaseType CastType => CastTypePin.InferredType?.Value ?? TypeSpecifier.FromType<object>();

    public ExplicitCastNode(Graph graph)
        : base(graph)
    {
        AddInputTypePin(NameDB.Type);
        AddInputDataPin(NameDB.Object, TypeSpecifier.FromType<object>());
        AddOutputDataPin(NameDB.Cast, CastType);
        AddExecPins();
    }

    private void AddExecPins()
    {
        AddInputExecPin(NameDB.Exec);
        AddOutputExecPin(NameDB.Success);
        AddOutputExecPin(NameDB.Failure);
    }

    protected override void SetPurity(bool pure)
    {
        base.SetPurity(pure);

        if (pure)
        {
            var outExecPins = new OutputExecPin[]
            {
                OutputExecPins.Single(p => p.Name == "Success"),
                OutputExecPins.Single(p => p.Name == "Failure"),
            };

            foreach (var execPin in outExecPins)
            {
                GraphUtil.DisconnectOutputExecPin(execPin);
                OutputExecPins.Remove(execPin);
            }

            var inExecPin = InputExecPins.Single(p => p.Name == "Exec");
            GraphUtil.DisconnectInputExecPin(inExecPin);
            InputExecPins.Remove(inExecPin);
        }
        else
        {
            AddExecPins();
        }
    }

    protected override void OnInputTypeChanged(object sender)
    {
        base.OnInputTypeChanged(sender);

        CastPin.PinType.Value = CastType;
    }

    public override string ToString() => $"Explicit Cast to {CastType.ShortName}";
}