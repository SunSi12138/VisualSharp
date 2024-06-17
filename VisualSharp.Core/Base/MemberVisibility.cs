namespace VisualSharp;

[Flags]
public enum MemberVisibility
{
    Invalid = 0,
    Private = 1,
    Public = 2,
    Protected = 4,
    Internal = 8,

    Any = Private | Public | Protected | Internal,
    ProtectedOrPublic = Protected | Public,
}