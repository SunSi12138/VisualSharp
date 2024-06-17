namespace VisualSharp;

public partial class ExecutionGraphTranslator
{
    public void PureTranslateMakeDelegateNode(MakeDelegateNode node)
    {
        // Write assignment of return value
        string returnName = GetOrCreatePinName(node.OutputDataPins[0]);
        builder.Append($"{returnName} = ");

        // Static: Write class name / target, default to own class name
        // Instance: Write target, default to this

        if (node.IsFromStaticMethod)
        {
            builder.Append($"{node.MethodSpecifier.DeclaringType}.");
        }
        else
        {
            if (node.TargetPin.IncomingPin != null)
            {
                string targetName = GetOrCreatePinName(node.TargetPin.IncomingPin);
                builder.Append($"{targetName}.");
            }
            else
            {
                // Default to thise
                builder.Append("this.");
            }
        }

        // Write method name
        builder.AppendLine($"{node.MethodSpecifier.Name};");
    }
}