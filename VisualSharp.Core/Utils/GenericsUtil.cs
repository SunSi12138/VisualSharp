namespace VisualSharp;

public static class GenericsUtil
{
    public static BaseType ConstructWithTypePins(BaseType type, IEnumerable<InputTypePin> inputTypePins)
    {
        if(type is TypeSpecifier typeSpecifier)
        {
            Dictionary<GenericType, BaseType> replacementTypes = [];

            foreach(InputTypePin inputTypePin in inputTypePins)
            {
                if(inputTypePin.InferredType?.Value is BaseType replacementType && replacementType is not null)
                {
                    GenericType typeToReplace = typeSpecifier.GenericArguments.SingleOrDefault(arg=>arg.Name == inputTypePin.Name) as GenericType;

                    if(typeToReplace is not null)
                    {
                        replacementTypes.Add(typeToReplace, replacementType);
                    }
                }
            }

            try
            {
                var constructedType = typeSpecifier.Construct(replacementTypes);
                return constructedType;
            }
            catch
            {
                return typeSpecifier;
            }
        }
        else if(type is GenericType genericType)
        {
            var replacementType = inputTypePins.SingleOrDefault(p=>p.Name == genericType.Name)?.InferredType?.Value;
            if(replacementType is not null)
            {
                return replacementType;
            }
        }

        return type;
    }
}