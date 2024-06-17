namespace VisualSharp;
[Flags]
public enum VariableModifier
{
    None = 0,
    ReadOnly = 1,
    Const = 2,
    Static = 4,
    New = 8,
}