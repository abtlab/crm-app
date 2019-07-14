﻿namespace Crm.Utils.String
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string value)
        {
            return value == null || string.IsNullOrWhiteSpace(value);
        }
        
        public static bool IsNotEmpty(this string value)
        {
            return value != null && !string.IsNullOrWhiteSpace(value);
        }
    }
}