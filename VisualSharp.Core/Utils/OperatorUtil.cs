namespace VisualSharp;

public static class OperatorUtil
{
    const string OperatorPrefix = "op_";

    static readonly Dictionary<string,OperatorInfo> operatorSymbols = new()
    {
        // Unary
        {"op_Increment",     new OperatorInfo("Increment",     "++", true,  true )},
        {"op_Decrement",     new OperatorInfo("Decrement",     "--", true,  true )},
        {"op_UnaryPlus",     new OperatorInfo("UnaryPlus",     "+",  true,  false)},
        {"op_UnaryNegation", new OperatorInfo("UnaryNegation", "-",  true,  false)},
        {"op_LogicalNot",    new OperatorInfo("LogicalNot",    "!",  true,  false)},

        //Binary
        {"op_Addition",    new OperatorInfo("Addition",    "+",  false, false)},
        {"op_Subtraction", new OperatorInfo("Subtraction", "-",  false, false)},
        {"op_Multiply",    new OperatorInfo("Multiply",    "*",  false, false)},
        {"op_Division",    new OperatorInfo("Division",    "/",  false, false)},
        {"op_Modulus",     new OperatorInfo("Modulus",     "%",  false, false)},

        {"op_BitwiseAnd",  new OperatorInfo("BitwiseAnd",  "&",  false, false)},
        {"op_BitwiseOr",   new OperatorInfo("BitwiseOr",   "|",  false, false)},
        {"op_ExclusiveOr", new OperatorInfo("ExclusiveOr", "^",  false, false)},
        {"op_LeftShift",   new OperatorInfo("LeftShift",   "<<", false, false)},
        {"op_RightShift",  new OperatorInfo("RightShift",  ">>", false, false)},

        {"op_GreaterThan",        new OperatorInfo("GreaterThan",        ">",  false,false)},
        {"op_LessThan",           new OperatorInfo("LessThan",           "<",  false,false)},
        {"op_LessThanOrEqual",    new OperatorInfo("LessThanOrEqual",    "<=", false,false)},
        {"op_GreaterThanOrEqual", new OperatorInfo("GreaterThanOrEqual", ">=", false,false)},
        {"op_Equality",           new OperatorInfo("Equality",           "==", false,false)},
        {"op_Inequality",         new OperatorInfo("Inequality",         "!=", false,false)},
        
        
        {"op_Assign",                   new OperatorInfo("Assign",                   "=", false,false)},
        {"op_AdditionAssignment",       new OperatorInfo("AdditionAssignment",       "+=",false,false)},
        {"op_SubtractionAssignment",    new OperatorInfo("SubtractionAssignment",    "-=",false,false)},
        {"op_MultiplicationAssignment", new OperatorInfo("MultiplicationAssignment", "*=",false,false)},
        {"op_DivisionAssignment",       new OperatorInfo("DivisionAssignment",       "/=",false,false)},
        
        // Custom (not part of .Net symbols)
        {"op_BitwiseNot", new OperatorInfo("BitwiseNot", "-",  true,  false)},
        {"op_LogicAnd",   new OperatorInfo("And",        "&&", false, false)},
        {"op_LogicOr",    new OperatorInfo("Or",         "||", false, false)},
    
    };

    public static bool TryGetOperatorInfo(MethodSpecifier methodSpecifier, out OperatorInfo operatorInfo) =>operatorSymbols.TryGetValue(methodSpecifier.Name, out operatorInfo);
}