using System.Text;

namespace VisualSharp;

public partial class ClassTranslator
{
    const string VARIABLE_MODIFIER = "%VariableModifier%";
    const string VARIABLE_TYPE = "%VariableType%";
    const string VARIABLE_NAME = "%VariableName%";
    const string GETTER = "%Get%";
    const string SETTER = "%Set%";
    const string VARIABLE_TEMPLATE = $"{VARIABLE_MODIFIER}{VARIABLE_TYPE} {VARIABLE_NAME};";
    const string PROPERTY_TEMPLATE = 
@$"{VARIABLE_MODIFIER}{VARIABLE_TYPE} {VARIABLE_NAME}
{{
    {GETTER}
    {SETTER}
}}";
    /// <summary>
    /// Translates a variable into C#.
    /// </summary>
    /// <param name="variable">Variable to translate.</param>
    /// <returns>C# code for the variable.</returns>
    string TranslateVariable(Variable variable)
    {
        StringBuilder modifiers = new StringBuilder();

        modifiers.Append($"{TranslatorUtil.VisibilityTokens[variable.Visibility]} ");

        if (variable.Modifiers.HasFlag(VariableModifier.Static))
        {
            modifiers.Append("static ");
        }

        if (variable.Modifiers.HasFlag(VariableModifier.ReadOnly))
        {
            modifiers.Append("readonly ");
        }

        if (variable.Modifiers.HasFlag(VariableModifier.New))
        {
            modifiers.Append("new ");
        }

        if (variable.Modifiers.HasFlag(VariableModifier.Const))
        {
            modifiers.Append("const ");
        }

        if (variable.HasAccessors)
        {
            // Translate get / set methods

            string output = PROPERTY_TEMPLATE
                .Replace(VARIABLE_MODIFIER, modifiers.ToString())
                .Replace(VARIABLE_TYPE, variable.Type.FullCodeName)
                .Replace(VARIABLE_NAME, variable.Name);

            if (variable.GetterMethod != null)
            {
                string getterMethodCode = methodTranslator.Translate(variable.GetterMethod, false);
                string visibilityPrefix = variable.GetterMethod.Visibility != variable.Visibility ? $"{TranslatorUtil.VisibilityTokens[variable.GetterMethod.Visibility]} " : "";

                output = output.Replace(GETTER, $"{visibilityPrefix}get\n{getterMethodCode}");
            }
            else
            {
                output = output.Replace(GETTER, "");
            }

            if (variable.SetterMethod != null)
            {
                string setterMethodCode = methodTranslator.Translate(variable.SetterMethod, false);
                string visibilityPrefix = variable.SetterMethod.Visibility != variable.Visibility ? $"{TranslatorUtil.VisibilityTokens[variable.SetterMethod.Visibility]} " : "";

                output = output.Replace(SETTER, $"{visibilityPrefix}set\n{setterMethodCode}");
            }
            else
            {
                output = output.Replace(SETTER, "");
            }

            return output;
        }
        else
        {
            return VARIABLE_TEMPLATE
                .Replace(VARIABLE_MODIFIER, modifiers.ToString())
                .Replace(VARIABLE_TYPE, variable.Type.FullCodeName)
                .Replace(VARIABLE_NAME, variable.Name);
        }
    }
}