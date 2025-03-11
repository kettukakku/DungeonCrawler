
using System;

public static class TypeExtensions
{
    public static bool Between(this int value, int min, int max)
    {
        return value >= min && value <= max;
    }

}
