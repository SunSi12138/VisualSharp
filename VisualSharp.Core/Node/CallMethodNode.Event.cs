namespace VisualSharp;

public partial class CallMethodNode
{
    protected override void OnInputTypeChanged(object sender)
    {
        base.OnInputTypeChanged(sender);
        UpdateTypes();
    }
}