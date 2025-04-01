using System;
namespace Helpers.Extentions
{


    public static class EnumExtensions
    {
        public static string GetEnumName<T>(this T enumValue, bool exceptionRise = true) where T : Enum
        {
            try
            {
                return Enum.GetName(typeof(T), enumValue);
            }
            catch (Exception) { if (!exceptionRise) return string.Empty; else throw; }

        }
        public static string GetEnumName<TEnum, TType>(this TType byteValue, bool exceptionRise = true) where TEnum : Enum
        {
            try
            {
                return Enum.GetName(typeof(TEnum), byteValue);
            }
            catch (Exception) { if (!exceptionRise) return string.Empty; else throw; }
        }

        public static T ToEnum<T>(this string value, bool ignoreCase = true) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
            }

            if (Enum.TryParse<T>(value, ignoreCase, out var result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException($"'{value}' is not a valid value for enum type {typeof(T).Name}.");
            }
        }
    }
}


   