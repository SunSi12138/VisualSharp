namespace VisualSharp;


/// <summary>
/// Node representing an integer-based for-loop.
/// </summary>
public class ForLoopNode : Node
{
    /// <summary>
    /// Execution pin that gets executed for each loop.
    /// </summary>
    public OutputExecPin LoopPin => OutputExecPins[0];

    /// <summary>
    /// Execution pin that gets executed when the loop is over.
    /// </summary>
    public OutputExecPin CompletedPin => OutputExecPins[1];

    /// <summary>
    /// Input execution pin that executes the loop.
    /// </summary>
    public InputExecPin ExecutionPin => InputExecPins[0];

    /// <summary>
    /// Input execution pin that skips the current loop step.
    /// Should only be executed from within this loop.
    /// </summary>
    public InputExecPin ContinuePin => InputExecPins[1];

    /// <summary>
    /// Input data pin for the initial inclusive index value of the loop.
    /// </summary>
    public InputDataPin InitialIndexPin => InputDataPins[0];

    /// <summary>
    /// Input data pin for the maximum exclusive index value of the loop.
    /// </summary>
    public InputDataPin MaxIndexPin => InputDataPins[1];

    /// <summary>
    /// Output data pin for the current index value of the loop.
    /// Starts at the value of InitialIndexPin and increases up to,
    /// but not including, MaxIndexPin.
    /// </summary>
    public OutputDataPin IndexPin => OutputDataPins[0];

    public ForLoopNode(Graph graph)
        : base(graph)
    {
        AddInputExecPin(NameDB.Exec);
        AddInputExecPin(NameDB.Continue);

        AddOutputExecPin(NameDB.Loop);
        AddOutputExecPin(NameDB.Completed);

        AddInputDataPin(NameDB.InitialIndex, TypeSpecifier.FromType<int>());
        AddInputDataPin(NameDB.MaxIndex, TypeSpecifier.FromType<int>());

        AddOutputDataPin(NameDB.Index, TypeSpecifier.FromType<int>());

        InitialIndexPin.UsesExplicitDefaultValue = true;
        InitialIndexPin.ExplicitDefaultValue = 0;
    }

    public override string ToString() => "For Loop";
}