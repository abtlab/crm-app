﻿namespace Crm.Utils.String
{
    public static class StringExtension
    {
        public static bool IsEmpty(this string value)
        {
            return value == null || string.IsNullOrWhiteSpace(value);
        }
    }
}