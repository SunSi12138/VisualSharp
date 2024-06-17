namespace VisualSharp;

[Flags]
public enum CompilationOutput
{
    Nothing = 0,
    SourceCode = 1,
    Binaries = 2,
    Errors = 4,
    All = SourceCode | Binaries | Errors,
}