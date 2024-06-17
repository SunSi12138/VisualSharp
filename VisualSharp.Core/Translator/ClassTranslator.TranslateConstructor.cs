namespace VisualSharp;

public partial class ClassTranslator
{
    string TranslateConstructor(ConstructorGraph m)
    {
        return methodTranslator.Translate(m, true);
    }
}