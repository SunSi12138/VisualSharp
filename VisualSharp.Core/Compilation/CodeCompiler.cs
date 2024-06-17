using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection;
using System.Threading.Tasks;

namespace VisualSharp;

/// <summary>
/// Compiles code into binaries.
/// </summary>
public static class CodeCompiler
{
    const string PROJECT_NAME = "VisualSharpOutput";
    const string ASSEMBLY_NAME = "VisualSharpOutput";
    /// <summary>
    /// Compiles code into a binary.
    /// TODO: 目前编译使用的是 Roslyn，但是Roslyn的实现并不完整，不能支持源生成器等功能，需要添加分析器引用
    /// </summary>
    /// <param name="outputPath">Output path for the compilation.</param>
    /// <param name="assemblyPaths">Paths to assemblies to reference.</param>
    /// <param name="sourc">Source code to compile.</param>
    /// <param name="generateExecutable">Whether to generate an executable or a dynamically linked library.</param>
    /// <returns>Results for the compilation.</returns>
    public static async Task<CodeCompilationResult> CompileSources(string outputPath, IEnumerable<string> assemblyPaths,
        IEnumerable<(string Name,string Code)> sources, bool generateExecutable)
    {
        var projectInfo = ProjectInfo.Create(
            ProjectId.CreateNewId(),
            VersionStamp.Create(),
            PROJECT_NAME,
            ASSEMBLY_NAME,
            LanguageNames.CSharp
        );
        

        var solution = new AdhocWorkspace()
            .CurrentSolution
            .AddProject(projectInfo);
        
        var project = solution.GetProject(projectInfo.Id);

        IEnumerable<MetadataReference> references = assemblyPaths.Select(path => MetadataReference.CreateFromFile(path));

        var compilationOptions = new CSharpCompilationOptions(generateExecutable ? OutputKind.ConsoleApplication : OutputKind.DynamicallyLinkedLibrary);
        project = project.WithCompilationOptions(compilationOptions);


        foreach(var source in sources)
        {
            _ = project.AddDocument($"{source.Name}.cs", source.Code);
        }

        // Create compilation
        var compilation = await project.GetCompilationAsync();
        IEnumerable<AnalyzerReference> analyzerReferences = project.AnalyzerReferences;
        IEnumerable<ISourceGenerator> generators = analyzerReferences.SelectMany(reference => reference.GetGenerators(language: LanguageNames.CSharp));
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            generators:generators,
            driverOptions: new GeneratorDriverOptions(default,trackIncrementalGeneratorSteps:true)
        );
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var updatedCompilation, out var diagnostics);

        EmitResult emitResult = updatedCompilation.Emit(outputPath);
        // EmitResult emitResult = updatedCompilation.Emit(outputPath);

        IEnumerable<string> errors = emitResult.Diagnostics
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .Select(d => d.GetMessage());

        return new CodeCompilationResult(emitResult.Success, errors, emitResult.Success ? outputPath : null);
    }
}
