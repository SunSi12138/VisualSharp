namespace VisualSharp;

/// <summary>
/// Represents an abstract base class for data pins in a node.
/// 表示节点中数据引脚的抽象基类。
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="NodeDataPin"/> class.
/// 初始化 <see cref="NodeDataPin"/> 类的新实例。
/// </remarks>
/// <param name="node">The node that the data pin belongs to.</param>
/// <param name="name">The name of the data pin.</param>
/// <param name="pinType">The pin type of the data pin.</param>
public abstract class NodeDataPin(Node node, string name, ObservableValue<BaseType> pinType) : Pin(node, name)
{
    /// <summary>
    /// Gets the pin type of the data pin.
    /// 获取数据引脚的引脚类型。
    /// </summary>
    public ObservableValue<BaseType> PinType { get; private set; } = pinType;

    /// <summary>
    /// Returns a string representation of the data pin.
    /// 返回数据引脚的字符串表示形式。
    /// </summary>
    /// <returns>A string representation of the data pin.</returns>
    public override string ToString() => $"{Name} : {PinType.Value.ShortName}";
}
