﻿using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace FirebaseREST.Auth
{
    public static class EnumExtensions
    {
        public static string ToEnumString<T>(this T type)
        {
            var enumType = typeof(T);
            var name = Enum.GetName(enumType, type);
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetTypeInfo().DeclaredFields.First(f => f.Name == name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();

            return enumMemberAttribute.Value;
        }
    }
}
