namespace VisualSharp;

/// <summary>
/// 抽象基类，表示类型的基本信息。
/// </summary>
public abstract class BaseType
{
    // ie: NameSpace.TpypeName
    /// <summary>
    /// 类型的名称。 (ie. NameSpace.TypeName)
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 获取类型的完整代码名称，用于在代码中表示。
    /// 与 Name 的区别在于，嵌套类在后端使用 "+" 表示，而在代码中使用 "." 表示。
    /// </summary>
    public virtual string FullCodeName => Name.Replace("+", ".");

    /// <summary>
    /// 获取类型的未绑定完整代码名称，用于在代码中表示未绑定的泛型参数。
    /// 例如 List<T> -> List<>。在代码中引用未绑定类型时需要使用此名称。
    /// </summary>
    public virtual string FullCodeNameUnbound => Name.Replace("+", ".");

    /// <summary>
    /// 获取类型的简短名称（不包含命名空间）。
    /// </summary>
    public virtual string ShortName => Name;

    /// <summary>
    /// 初始化 BaseType 类的新实例。
    /// </summary>
    /// <param name="name">类型的名称。</param>
    protected BaseType(string name) => Name = name;
}