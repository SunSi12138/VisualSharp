namespace VisualSharp;


/// <summary>
/// Class graph type. Contains methods, attributes and other common things usually associated
/// with classes.
/// </summary>
public partial class ClassGraph : Graph
{
    /// <summary>
    /// Return node of this class that receives the metadata for it.
    /// </summary>
    public ClassReturnNode ReturnNode=>Nodes.OfType<ClassReturnNode>().Single();

    /// <summary>
    /// Properties of this class.
    /// </summary>
    public ObservableRangeCollection<Variable> Variables { get; set; } = [];

    /// <summary>
    /// Methods of this class.
    /// </summary>
    public ObservableRangeCollection<MethodGraph> Methods { get; set; } = [];

    /// <summary>
    /// Constructors of this class.
    /// </summary>
    public ObservableRangeCollection<ConstructorGraph> Constructors { get; set; } = [];

    /// <summary>
    /// Base / super type of this class. The ultimate base type of all classes is System.Object.
    /// </summary>
    public TypeSpecifier SuperType => (TypeSpecifier)ReturnNode.SuperTypePin.InferredType?.Value ?? TypeSpecifier.FromType<object>();

    /// <summary>
    /// Type this class inherits from and interfaces this class implements.
    /// </summary>
    public IEnumerable<TypeSpecifier> AllBaseTypes => new[] { SuperType }.Concat(ReturnNode.InterfacePins.Select(pin => (TypeSpecifier)pin.InferredType?.Value ?? TypeSpecifier.FromType<object>()));

    /// <summary>
    /// Namespace this class is in.
    /// </summary>
    public string Namespace { get; set; }

    /// <summary>
    /// Name of the class without namespace.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Name of the class with namespace if any.
    /// </summary>
    public string FullName => string.IsNullOrWhiteSpace(Namespace) ? Name : $"{Namespace}.{Name}";

    /// <summary>
    /// Modifiers this class has.
    /// </summary>
    public ClassModifier Modifiers { get; set; }

    /// <summary>
    /// Visibility of this class.
    /// </summary>
    public MemberVisibility Visibility { get; set; } = MemberVisibility.Internal;

    /// <summary>
    /// Generic arguments this class takes.
    /// </summary>
    public ObservableRangeCollection<GenericType> DeclaredGenericArguments { get; set; } = new ObservableRangeCollection<GenericType>();

    /// <summary>
    /// TypeSpecifier describing this class.
    /// </summary>
    public TypeSpecifier Type=> new TypeSpecifier(FullName, SuperType.IsEnum, SuperType.IsInterface, DeclaredGenericArguments.Cast<BaseType>().ToList());

    public ClassGraph()=>_ = new ClassReturnNode(this);
}