namespace VisualSharp;

/// <summary>
/// Type graph that returns a type.
/// </summary>
public class TypeGraph : Graph
{
    /// <summary>
    /// Return node of this type graph that receives the type.
    /// </summary>
    public TypeReturnNode ReturnNode=>Nodes.OfType<TypeReturnNode>().Single();
    /// <summary>
    /// TypeSpecifier for the type this graph returns.
    /// </summary>
    public TypeSpecifier ReturnType=>(TypeSpecifier)ReturnNode.TypePin.InferredType?.Value ?? TypeSpecifier.FromType<object>();

    public TypeGraph()=>_ = new TypeReturnNode(this);
}
