using System;
using System.Reflection;

namespace com.spector.Extensions
{
    public static class TypeExtensions
    {
        public static Type GetType(string className, bool searchAllAssemblies)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in assembly.GetTypes())
                {
                    if (t.FullName == className)
                    {
                        return Type.GetType($"{className}, {assembly.FullName}");
                    }
                }
            }

            return null;
        }
    }
}