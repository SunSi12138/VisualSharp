namespace VisualSharp;

public class AssemblyReference:CompilationReference
{
    public string AssemblyPath { get; set; }

    public AssemblyReference(string assemblyPath)=>AssemblyPath = assemblyPath;

    public override string ToString()=>$"{Path.GetFileNameWithoutExtension(AssemblyPath)} at {AssemblyPath}";

    public static AssemblyReference FromPath(string path)=>new AssemblyReference(path);
    
    static readonly string RuntimePath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
    /// <summary>
    /// 从dotnet内置库加载程序集
    /// 例如System.dll, System.Core.dll, mscorelib.dll
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static AssemblyReference FromName(string name)=>new AssemblyReference($"{RuntimePath}{name}.dll");
}