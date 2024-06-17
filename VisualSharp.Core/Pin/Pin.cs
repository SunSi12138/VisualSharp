namespace VisualSharp;

/// <summary>
/// Represents a pin on a node.
/// 表示节点上的引脚。
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Pin"/> class with the specified node.
/// 使用指定的节点初始化 <see cref="Pin"/> 类的新实例。
/// </remarks>
/// <param name="node">The node that this pin belongs to.</param>
public abstract class Pin(Node node, string name)
{
    /// <summary>
    /// The name of the pin.
    /// 获取或设置引脚的名称。
    /// </summary>
    public string Name { get; set; } = name;

    /// <summary>
    /// The node that this pin belongs to.
    /// 此引脚所属的节点。
    /// </summary>
    public Node Node { get; private set; } = node;

    public override string ToString()=>Name;
}