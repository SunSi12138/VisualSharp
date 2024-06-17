namespace VisualSharp;
[Flags]
 public enum MethodModifier
{
    None = 0,
    Sealed = 1,
    Abstract = 2,
    Static = 4,
    Virtual = 8,
    Override = 16,
    Async = 32,
}