namespace VisualSharp;

public partial class ClassTranslator
{
    string TranslateMethod(MethodGraph m)
    {
        return methodTranslator.Translate(m, true);
    }
}