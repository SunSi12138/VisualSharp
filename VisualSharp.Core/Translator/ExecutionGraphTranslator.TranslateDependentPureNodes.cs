namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    void TranslateDependentPureNodes(Node node)
    {
        var sortedPureNodes = TranslatorUtil.GetSortedPureNodes(node);
        foreach(Node depNode in sortedPureNodes)
        {
            TranslateNode(depNode, 0);
        }
    }
}