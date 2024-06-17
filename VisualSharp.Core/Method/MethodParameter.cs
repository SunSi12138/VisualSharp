namespace VisualSharp;
//  TODO: 序列化设置
public class MethodParameter(string name, BaseType type, MethodParameterPassType passType, bool hasExplicitDefaultValue = false, object explicitDefaultValue = null) : Named<BaseType>(name,type)
{
    public MethodParameterPassType PassType { get; private set; } = passType;

    public bool HasExplicitDefaultValue { get; private set; } = hasExplicitDefaultValue;

    public object ExplicitDefaultValue { get; private set; } = explicitDefaultValue;
}