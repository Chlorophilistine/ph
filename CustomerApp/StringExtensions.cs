namespace CustomerApp
{
    using System;

    public static class StringExtensions
    {
        public static (bool parsed, T result) TryParse<T>(this string status) where T : struct, Enum
        {
            return (Enum.TryParse<T>(status, true, out var newStatus), newStatus);
        }
    }
}