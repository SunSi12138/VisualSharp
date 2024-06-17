using System.Text.RegularExpressions;

namespace VisualSharp;

public static partial class NameUtil
{
    // TODO:?看不懂，为什么要这么做
    public static string GetUniqueName(string name, IList<string> names)
    {
        int i = 1;

        while (true)
        {
            string uniqueName = i == 1 ? name : $"{name}{i}";

            if (!names.Contains(uniqueName))
            {
                return uniqueName;
            }

            i++;
        }
    }

    public static string SplitCamelCase(string input)
    {
        return MyRegex().Replace(input, " $1").Trim();
    }

    [GeneratedRegex("([A-Z])", RegexOptions.Compiled)]
    private static partial Regex MyRegex();
}