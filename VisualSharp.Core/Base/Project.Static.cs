using System.Threading.Tasks;
using System.Collections.Concurrent;
namespace VisualSharp;
public partial class Project
{
    static readonly IEnumerable<AssemblyReference> DefaultReferences = 
    [
        AssemblyReference.FromName("mscorlib"),
        AssemblyReference.FromName("System"),
        AssemblyReference.FromName("System.Core"),
    ];
    public static string GetClassStoragePath(ClassGraph csg)=>$"{csg.FullName}.csg";
    
    public static Project Create(string name, string defaultNamespace, bool addDefaultReference=true, CompilationOutput compilationOutput = CompilationOutput.All)
    {
        Project project = new Project
        {
            Name = name,
            DefaultNamespace = defaultNamespace,
            CompilationOutput = compilationOutput,
        };

        if(addDefaultReference)
        {
            project.References.AddRange(DefaultReferences);
        }

        return project;
    }

    public static async Task<(bool,Project)> LoadFromPath(string path)
    {
        var data = await File.ReadAllBytesAsync(path);

        if(MessagePackSerializer.Deserialize<Project>(data) is Project project)
        {
            project.Path = path;

            var directoryName = System.IO.Path.GetDirectoryName(path);
            ConcurrentBag<ClassGraph> classes = [];
            await Parallel.ForEachAsync(project.ClassPaths, async (classPath, cts) =>
            {
                var classData = await File.ReadAllBytesAsync(System.IO.Path.Combine(directoryName, classPath));
                var csg = MessagePackSerializer.Deserialize<ClassGraph>(classData);
                csg.Project = project;
                classes.Add(csg);
            });

            project.Classes.ReplaceRange(classes);

            return (true, project);
        }

        return (false, null);
    }
}