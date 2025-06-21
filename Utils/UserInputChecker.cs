namespace Synx.Common.Utils;

// TODO: UnitTest
public static class UserInputChecker
{
    public static bool TypeChecker(object content, Type? targetType)
    {
        if (content.GetType() == targetType)
        {
            return true;
        }
        return false;
    }

    public static bool NumericChecker(object content)
    {
        if (content is int or long or float or double or decimal)
        {
            return true;
        }

        if (content is string str)
        {
            return double.TryParse(str, out _);
        }

        return false;
    }

    public static bool ValueOutOfRangeChecker(object content, object[] range)
    {
        if (NumericChecker(content) && range[0].GetType() == content.GetType() && range.Length == 2)
        {
            
        }

        throw new NotImplementedException();
    }
    
    public static bool Check(object content, object? rule, Func<object, object?, bool>[] checkers)
    {
        var result = true;
        foreach (var checker in checkers)
        {
            result = result && checker(content, rule);
        }
        return result;
    }
}