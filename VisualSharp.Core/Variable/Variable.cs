namespace VisualSharp;

public class Variable
{
    /// <summary>
    /// Name of the variable without any prefixes.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Class this variable is contained in.
    /// </summary>
    public ClassGraph Class { get; private set; }

    /// <summary>
    /// Specifier for the type of the variable.
    /// </summary>
    public TypeSpecifier Type => TypeGraph.ReturnType;

    private TypeSpecifier OldType
    {
        get => null;
        set
        {
            TypeGraph = new TypeGraph();
            GraphUtil.CreateNestedTypeNode(TypeGraph, value, 500, 500);
        }
    }

    /// <summary>
    /// Get method for this variable. Can be null.
    /// </summary>
    public MethodGraph GetterMethod { get; set; }

    /// <summary>
    /// Set method for this variable. Can be null.
    /// </summary>
    public MethodGraph SetterMethod { get; set; }

    /// <summary>
    /// Graph specifying the type of this variable.
    /// </summary>
    public TypeGraph TypeGraph { get; set; }

    /// <summary>
    /// Whether this variable has a public getter.
    /// </summary>
    public bool HasPublicGetter=>HasAccessors ?(GetterMethod?.Visibility == MemberVisibility.Public) : Visibility == MemberVisibility.Public;

    /// <summary>
    /// Whether this variable has a public setter.
    /// </summary>
    public bool HasPublicSetter=> HasAccessors ?(SetterMethod?.Visibility == MemberVisibility.Public) :Visibility == MemberVisibility.Public;
    /// <summary>
    /// Whether this variable declares a get or set method.
    /// </summary>
    public bool HasAccessors=>GetterMethod != null || SetterMethod != null;

    /// <summary>
    /// Visibility of this property.
    /// </summary>
    public MemberVisibility Visibility { get; set; } = MemberVisibility.Private;

    /// <summary>
    /// Modifiers of this variable.
    /// </summary>
    public VariableModifier Modifiers { get; set; }

    public VariableSpecifier Specifier=>new VariableSpecifier(Name, Type, GetterMethod?.Visibility ?? Visibility, SetterMethod?.Visibility ?? Visibility, Class.Type, Modifiers);

    /// <summary>
    /// Creates a PropertySpecifier.
    /// </summary>
    /// <param name="cls">Graph the variable is a part of.</param>
    /// <param name="name">Name of the property.</param>
    /// <param name="type">Specifier for the type of this property.</param>
    /// <param name="getter">Get method for the property. Can be null if there is none.</param>
    /// <param name="setter">Set method for the property. Can be null if there is none.</param>
    /// <param name="modifiers">Modifiers of the variable.</param>
    public Variable(ClassGraph cls, string name, TypeSpecifier type, MethodGraph getter,
        MethodGraph setter, VariableModifier modifiers)
    {
        Class = cls;
        Name = name;
        GetterMethod = getter;
        SetterMethod = setter;
        Modifiers = modifiers;

        // Create a type graph with the type as its return type.
        TypeGraph = new TypeGraph();
        var typePin = GraphUtil.CreateNestedTypeNode(TypeGraph, type, 500, 300).OutputTypePins[0];
        TypeGraph.ReturnNode.PositionX = 800;
        TypeGraph.ReturnNode.PositionY = 300;
        GraphUtil.ConnectTypePins(typePin, TypeGraph.ReturnNode.TypePin);
    }
}