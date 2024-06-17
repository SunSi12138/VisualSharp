using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace VisualSharp;

public partial class Project
{
    bool isCompiling = false;
    bool lastCompilationSucceeded = false;
    string compilationMessage = CompileStatus.Ready;
    ObservableRangeCollection<string> lastCompileErrors = [];
    public bool CanCompileAndRun=>CanCompile
        && BinaryType == BinaryType.Executable
        && CompilationOutput.HasFlag(CompilationOutput.Binaries);

    public bool CanCompile=>!isCompiling;
    public bool IsCompiling
    {
        get => isCompiling;
        set
        {
            if(isCompiling!=value) isCompiling = value;
        }
    }
    public string CompilationMessage
    {
        get => compilationMessage;
        set
        {
            if (compilationMessage != value) compilationMessage = value;
        }
    }
    public bool LastCompilationSucceeded
    {
        get=>lastCompilationSucceeded;
        set
        {
            if(lastCompilationSucceeded != value)
            {
                lastCompilationSucceeded = value;
            }
        }
    }    public ObservableRangeCollection<string> LastCompileErrors
    {
        get=>lastCompileErrors;
        set
        {
            if(lastCompileErrors!=value) lastCompileErrors = value;
        }
    }


    public async void Save()
    {
        foreach(ClassGraph csg in Classes)
        {
            // TODO: 只保存未保存的类和已经修改的类

            var outputPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Path),GetClassStoragePath(csg));
            var csgData = MessagePackSerializer.Serialize(csg);
            await File.WriteAllBytesAsync(outputPath,csgData);
        }

        ClassPaths = new ObservableRangeCollection<string>(Classes.Select(GetClassStoragePath));

        SavedVersion = Assembly.GetExecutingAssembly().GetName().Version;

        var data = MessagePackSerializer.Serialize(this);
        
        await File.WriteAllBytesAsync(Path, data);
    }
    public async void Compile()
    {
        if(!CanCompile||CompilationOutput == CompilationOutput.Nothing) return;

        IsCompiling = true;

        CompilationMessage = CompileStatus.Compiling;

        var references = References.ToArray();

        // var result = await 
        var compilationResult = await Task.Run(()=>
        {
            string projectDir = System.IO.Path.GetDirectoryName(Path);
            string compileDir = System.IO.Path.Combine(projectDir, "bin");
            if(Directory.Exists(compileDir))
            {
                Directory.Delete(compileDir, true);
            }
            Directory.CreateDirectory(compileDir);

            ConcurrentBag<(string Name,string Code)> classSources = [];

            Parallel.ForEach(Classes, csg=>
            {
                ClassTranslator ct = new();

                string code;

                try
                {
                    code = ct.TranslateClass(csg);
                }
                catch(Exception e)
                {
                    code = e.ToString();
                }

                string[] directories = csg.FullName.Split('.');
                directories = directories
                    .Take(directories.Length - 1)
                    .Prepend(compileDir)
                    .ToArray();

                string outputDir = System.IO.Path.Combine(directories);

                if(CompilationOutput.HasFlag(CompilationOutput.SourceCode))
                {
                    Directory.CreateDirectory(outputDir);
                    var fileName = System.IO.Path.Combine(outputDir, $"{csg.Name}.cs");
                    File.WriteAllText(fileName, code);
                }
                classSources.Add((csg.Name, code));
            });

            var generateExecutable = BinaryType == BinaryType.Executable;

            // TODO：在mac和linux上生成可执行文件的后缀是空的
            var executableExtesion = TargetPlatform == OSPlatform.Windows?".exe":"" ;
            var outputExtension = generateExecutable ? executableExtesion : ".dll";
            var outputName = System.IO.Path.Combine(compileDir, Name+outputExtension);

            var assemblyPaths = references.OfType<AssemblyReference>().Select(a => a.AssemblyPath);

            var compilationResults = CodeCompiler.CompileSources(outputName, assemblyPaths,classSources, generateExecutable);

            return compilationResults;
        });
        

        LastCompilationSucceeded  = compilationResult.Success;
        LastCompileErrors = new ObservableRangeCollection<string>(compilationResult.Errors);

        if(LastCompilationSucceeded)
        {
            LastCompiledAssemblyPath = compilationResult.PathToAssembly;
            CompilationMessage = CompileStatus.Succeeded;
        }
        else
        {
            CompilationMessage = $"Build Failed With {compilationResult.Errors.Count()} Error(s)";
        }

        IsCompiling = false;
        
    }

    public IEnumerable<(string Name,string Code)> GenerateClassSources()
    {
        if (Classes is null) return [];

        ConcurrentBag<(string Name,string Code)> classSources = [];

        // Translate classes in parallel
        Parallel.ForEach(Classes, csg =>
        {
            // Translate the class to C#
            ClassTranslator classTranslator = new ClassTranslator();

            string code;
            try
            {
                code = classTranslator.TranslateClass(csg);
            }
            catch (Exception ex)
            {
                code = ex.ToString();
            }

            classSources.Add((csg.Name,code));
        });

        return classSources;
    }

    public void RunProject()
    {
        if (BinaryType != BinaryType.Executable || !CompilationOutput.HasFlag(CompilationOutput.Binaries))
        {
            throw new InvalidOperationException("Can only run executable projects which output their binaries.");
        }

        string projectDir = System.IO.Path.GetDirectoryName(Path);
        string exePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDir, $"Compiled_{Name}", $"{Name}.exe"));

        if (!File.Exists(exePath))
        {
            throw new Exception($"The executable does not exist at {exePath}");
        }

        Process.Start(exePath);
    }

}