using System.Runtime.InteropServices;

namespace VisualSharp;
public partial class Project
{
    public string Name { get; set; }
    /// <summary>
    /// 项目所在的路径，序列化时忽略，反序列化时赋值
    /// </summary>
    /// <value></value>
    public string Path { get; set; }
    /// <summary>
    /// Determines what gets output during compilation.
    /// </summary>
    /// <value></value>
    public CompilationOutput CompilationOutput { get; set; }
    /// <summary>
    /// Type of the binary output.
    /// </summary>
    /// <value></value>
    public BinaryType BinaryType { get; set; }

    public OSPlatform TargetPlatform { get; set; }
    public ObservableRangeCollection<ClassGraph> Classes { get; private set; } = [];
    /// <summary>
    /// Version of the editor that the project was saved with.
    /// </summary>
    /// <value></value>
    public Version SavedVersion { get; set; }
    /// <summary>
    /// Path to the last successfully compiled assembly.
    /// </summary>
    /// <value></value>
    public string LastCompiledAssemblyPath { get; set; }
    /// <summary>
    /// Default namespace of newly created classes.
    /// </summary>
    public string DefaultNamespace { get; set; }
    /// <summary>
    /// Paths to files for the class modles within this project.
    /// </summary>
    /// <value></value>
    public ObservableRangeCollection<string> ClassPaths { get; set; } = [];
    /// <summary>
    /// References of this project.
    /// </summary>
    /// <value></value>
    public ObservableRangeCollection<CompilationReference> References { get; set; } = [];

    Project(){}

}