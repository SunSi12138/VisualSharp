namespace VisualSharp;

public class Named<T>(string name, T value)
{
    public string Name { get; set; } = name;
    public T Value { get; set; } = value;

    public static implicit operator T(Named<T> namedValue)=>namedValue.Value;
    public override string ToString()=>$"{Name}:{Value}";
}