using System;
using System.ComponentModel;
using System.Linq;

namespace ScreenshotOne
{
    internal static class BoolExtensions
    {
        internal static string ToTrueFalseString(this bool value, string trueValue = "true", string falseValue = "false") => value ? trueValue : falseValue;
    }

    internal static class EnumExtensions
    {
        internal static string GetDescriptionOrValue(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
    }
}