using System.Text;

namespace VisualSharp;

public partial class ClassTranslator
{
    const string NAMESPACE = "%Namespace%";
    const string CLASS_MODIFIER = "%ClassModifier%";
    const string CLASS_NAME = "%ClassName%";
    const string GENERIC_ARGUMENTS = "%GenericArguments%";
    const string BASE_TYPE = "%BaseTypes%";
    const string CONTENT = "%Content%";
    const string CLASS_TEMPLATE =
@$"namespace {NAMESPACE};
{CLASS_MODIFIER}class {CLASS_NAME}{GENERIC_ARGUMENTS} : {BASE_TYPE}
{{
    {CONTENT}
}}";
    const string CLASS_TEMPLATE_NO_NAMESPACE = 
@$"{CLASS_MODIFIER}class {CLASS_NAME}{GENERIC_ARGUMENTS} : {BASE_TYPE}
{{
    {CONTENT}
}}";


    readonly ExecutionGraphTranslator methodTranslator = new();

    /// <summary>
    /// Translates a class into C#.
    /// </summary>
    /// <param name="c">Class to translate.</param>
    /// <returns>C# code for the class.</returns>
    public string TranslateClass(ClassGraph c)
    {
        StringBuilder content = new StringBuilder();

        foreach (Variable v in c.Variables)
        {
            content.AppendLine(TranslateVariable(v));
        }

        foreach (ConstructorGraph constructor in c.Constructors)
        {
            content.AppendLine(TranslateConstructor(constructor));
        }

        foreach (MethodGraph m in c.Methods)
        {
            content.AppendLine(TranslateMethod(m));
        }

        StringBuilder modifiers = new StringBuilder();

        modifiers.Append($"{TranslatorUtil.VisibilityTokens[c.Visibility]} ");

        if (c.Modifiers.HasFlag(ClassModifier.Static))
        {
            modifiers.Append("static ");
        }

        if (c.Modifiers.HasFlag(ClassModifier.Abstract))
        {
            modifiers.Append("abstract ");
        }

        if (c.Modifiers.HasFlag(ClassModifier.Sealed))
        {
            modifiers.Append("sealed ");
        }

        if (c.Modifiers.HasFlag(ClassModifier.Partial))
        {
            modifiers.Append("partial ");
        }

        string genericArguments = "";
        if (c.DeclaredGenericArguments.Count > 0)
        {
            genericArguments = "<" + string.Join(", ", c.DeclaredGenericArguments) + ">";
        }

        string baseTypes = string.Join(", ", c.AllBaseTypes);

        string generatedCode = (string.IsNullOrWhiteSpace(c.Namespace) ? CLASS_TEMPLATE_NO_NAMESPACE : CLASS_TEMPLATE)
            .Replace(NAMESPACE, c.Namespace)
            .Replace(CLASS_MODIFIER, modifiers.ToString())
            .Replace(CLASS_NAME, c.Name)
            .Replace(GENERIC_ARGUMENTS, genericArguments)
            .Replace(BASE_TYPE, baseTypes)
            .Replace(CONTENT, content.ToString());

        return TranslatorUtil.FormatCode(generatedCode);
    }
    

    
}