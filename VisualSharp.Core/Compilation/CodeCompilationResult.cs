namespace VisualSharp;

public class CodeCompilationResult(bool success, IEnumerable<string> errors,string pathToAssembly)
{
    public bool Success=>success;

    public IEnumerable<string> Errors=>errors;

    public string PathToAssembly=>pathToAssembly;
}