namespace VisualSharp;

public class OperatorInfo(string displayName, string symbol, bool unary, bool unaryRightPosition)
{
    public string DisplayName { get; } = displayName;
    public string Symbol { get; } = symbol;
    /// <summary>
    /// 是否一元运算符
    /// </summary>
    /// <value>布尔</value>
    public bool Unary { get; } = unary;
    /// <summary>
    /// 是否一元运算符右侧
    /// </summary>
    /// <value>布尔</value>
    public bool UnaryRightPosition { get; } = unaryRightPosition;
}