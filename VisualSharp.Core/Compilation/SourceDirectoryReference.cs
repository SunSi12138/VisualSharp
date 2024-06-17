

namespace VisualSharp;

public class SourceDirectoryReference(string directory, bool includeInCompilation = false) : CompilationReference
{
    /// <summary>
    /// All source file paths in the source directory.
    /// </summary>
    public IEnumerable<string> SourceFilePaths => Directory.GetFiles(SourceDirectory, "*.cs", SearchOption.AllDirectories).Where(p => !p.Contains("obj" + Path.DirectorySeparatorChar) && !p.Contains("bin" + Path.DirectorySeparatorChar));

    /// <summary>
    /// Whether to include the source files in compilation.
    /// </summary>
    public bool IncludeInCompilation { get; set; } = includeInCompilation;

    /// <summary>
    /// Path of source directory.
    /// </summary>
    public string SourceDirectory { get; private set; } = directory;

    public override string ToString() => $"Source files at {SourceDirectory}";
}