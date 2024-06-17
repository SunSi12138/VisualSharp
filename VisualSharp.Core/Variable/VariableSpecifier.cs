namespace VisualSharp;

public class VariableSpecifier(string name, TypeSpecifier type, MemberVisibility getterVisibility, MemberVisibility setterVisibility,TypeSpecifier declaringType, VariableModifier modifiers)
{
    /// <summary>
    /// Name of the property without any prefixes.
    /// </summary>
    public string Name { get; set; } = name;

    /// <summary>
    /// Specifier for the type this property is contained in.
    /// </summary>
    public TypeSpecifier DeclaringType { get; private set; } = declaringType;

    /// <summary>
    /// Specifier for the type of the property.
    /// </summary>
    public TypeSpecifier Type { get; set; } = type;

    /// <summary>
    /// Whether this property has a public getter.
    /// </summary>
    public MemberVisibility GetterVisibility { get; set; } = getterVisibility;

    /// <summary>
    /// Whether this property has a public setter.
    /// </summary>
    public MemberVisibility SetterVisibility { get; set; } = setterVisibility;

    /// <summary>
    /// Visibility of this property.
    /// </summary>
    public MemberVisibility Visibility { get; set; } = MemberVisibility.Private;

    /// <summary>
    /// Modifiers of this variable.
    /// </summary>
    public VariableModifier Modifiers { get; set; } = modifiers;
}
