namespace VisualSharp;


/// <summary>
/// Node representing an exception throw.
/// </summary>
public class ThrowNode : Node
{
    /// <summary>
    /// Pin for the exception to throw.
    /// </summary>
    public InputDataPin ExceptionPin => InputDataPins[0];

    public ThrowNode(Graph graph)
        : base(graph)
    {
        AddInputExecPin(NameDB.Exec);
        AddInputDataPin(NameDB.Exception, TypeSpecifier.FromType<Exception>());
    }

    public override string ToString() => $"Throw Exception";
}