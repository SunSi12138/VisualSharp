namespace VisualSharp;

/// <summary>
/// 表示节点的输出数据引脚
/// </summary>
/// <remarks>
/// Initializes a new instance of the NodeOutputDataPin class with the specified node, name, and pin type.
/// 使用指定的节点、名称和引脚类型初始化 NodeOutputDataPin 类的新实例。
/// </remarks>
/// <param name="node">引脚属于的节点</param>
/// <param name="name">引脚的名称</param>
/// <param name="pinType">输出的数据类型</param>
public class OutputDataPin(Node node, string name, ObservableValue<BaseType> pinType) : NodeDataPin(node, name, pinType)
{
    /// <summary>
    /// 获取此输出引脚连接到的所有输入引脚。
    /// </summary>
    public ObservableRangeCollection<InputDataPin> OutgoingPins { get; private set; } = [];
}