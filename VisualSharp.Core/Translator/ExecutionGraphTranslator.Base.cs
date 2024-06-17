using System.Text;

namespace VisualSharp;

public partial class ExecutionGraphTranslator
{

    readonly Dictionary<OutputDataPin,string> variableNames = [];

    readonly Dictionary<Node,List<int>> nodeStateIds = [];

    int nextStateId = 0;

    IEnumerable<Node> execNodes = [];
    IEnumerable<Node> nodes = [];
    readonly StringBuilder builder = new();

    private ExecutionGraph graph;

    Random random;

    delegate void NodeTypeHandler(ExecutionGraphTranslator translator, Node node);

    readonly Dictionary<Type,List<NodeTypeHandler>> nodeTypeHandlers = new()
    {
        {typeof(CallMethodNode),     [(translator,node)=>translator.TranslateCallMethodNode(node as CallMethodNode)]},
        {typeof(VariableSetterNode), [(translator,node)=>translator.TranslateVariableSetterNode(node as VariableSetterNode)]},
        {typeof(ReturnNode),         [(translator,node)=>translator.TranslateReturnNode(node as ReturnNode)]},
        {typeof(MethodEntryNode),    [(translator,node)=>translator.TranslateMethodEntryNode(node as MethodEntryNode)]},
        {typeof(IfElseNode),         [(translator,node)=>translator.TranslateIfElseNode(node as IfElseNode)]},
        {typeof(ConstructorNode),    [(translator,node)=>translator.TranslateConstructorNode(node as ConstructorNode)]},
        {typeof(ExplicitCastNode),   [(translator, node) => translator.TranslateExplicitCastNode(node as ExplicitCastNode) ] },
        {typeof(ThrowNode),          [(translator, node) => translator.TranslateThrowNode(node as ThrowNode) ] },
        {typeof(AwaitNode),          [(translator, node) => translator.TranslateAwaitNode(node as AwaitNode) ] },
        {typeof(TernaryNode),        [(translator, node) => translator.TranslateTernaryNode(node as TernaryNode) ] },
        {typeof(RerouteNode),        [(translator, node) => translator.TranslateRerouteNode(node as RerouteNode) ] },
        {typeof(ForLoopNode),        [
            (translator, node) => translator.TranslateStartForLoopNode(node as ForLoopNode),
            (translator, node) => translator.TranslateContinueForLoopNode(node as ForLoopNode)
        ]},
    
        { typeof(VariableGetterNode), [(translator, node) => translator.PureTranslateVariableGetterNode(node as VariableGetterNode) ]},
        { typeof(LiteralNode),        [(translator, node) => translator.PureTranslateLiteralNode(node as LiteralNode) ] },
        { typeof(MakeDelegateNode),   [(translator, node) => translator.PureTranslateMakeDelegateNode(node as MakeDelegateNode) ] },
        { typeof(TypeOfNode),         [(translator, node) => translator.PureTranslateTypeOfNode(node as TypeOfNode) ] },
        { typeof(MakeArrayNode),      [(translator, node) => translator.PureTranslateMakeArrayNode(node as MakeArrayNode) ] },
        { typeof(DefaultNode),        [(translator, node) => translator.PureTranslateDefaultNode(node as DefaultNode) ] },
    };
}