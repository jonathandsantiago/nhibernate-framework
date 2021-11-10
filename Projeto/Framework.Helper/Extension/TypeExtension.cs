using Framework.Helper.Helpers;
using System;
using System.Globalization;
using System.Linq;

namespace Framework.Helper.Extension
{
    public static class TypeExtension
    {
        public static bool InheritsOrImplements(this Type typeA, Type typeB, bool checkGenericArgument = true)
        {
            if (typeA == typeB)
            {
                return true;
            }

            typeB = ResolveGenericTypeDefinition(typeB);

            if (typeA.IsGenericType && checkGenericArgument)
            {
                Type[] arguments = typeA.GetGenericArguments();

                if (arguments != null && arguments.Count() > 0)
                {
                    Type arg = arguments[0];

                    if (arg.InheritsOrImplements(typeB))
                    {
                        return true;
                    }
                }
            }

            Type value = typeA.IsGenericType
                                   ? typeA.GetGenericTypeDefinition()
                                   : typeA;

            while (value != typeof(object))
            {
                if (value.IsAssignableFrom(typeB) || typeB == value || HasAnyInterfaces(typeB, value))
                {
                    return true;
                }

                value = value.BaseType != null
                               && value.BaseType.IsGenericType
                                   ? value.BaseType.GetGenericTypeDefinition()
                                   : value.BaseType;

                if (value == null)
                {
                    return false;
                }
            }

            return false;
        }

        public static bool InheritsOrImplements(this Type typeA, params Type[] typeB)
        {
            foreach (Type item in typeB)
            {
                if (typeA.InheritsOrImplements(item, false))
                {
                    return true;
                }
            }

            return false;
        }

        public static T ChangeType<T>(this object value, CultureInfo cultureInfo)
        {
            return TypeHelper.Converter<T>(value, cultureInfo);
        }

        public static T ChangeType<T>(this object value)
        {
            return TypeHelper.Converter<T>(value);
        }

        public static object GetDefaultValue(this Type t)
        {
            if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
            {
                return Activator.CreateInstance(t);
            }
            else
            {
                return null;
            }
        }

        public static bool PropExists(this Type tipo, string propertyName)
        {
            return TypeHelper.PropExists(tipo, propertyName);
        }

        private static bool HasAnyInterfaces(Type parent, Type child)
        {
            return child.GetInterfaces()
                .Any(childInterface =>
                {
                    Type currentInterface = childInterface.IsGenericType
                        ? childInterface.GetGenericTypeDefinition()
                        : childInterface;

                    return currentInterface == parent;
                });
        }

        private static Type ResolveGenericTypeDefinition(Type parent)
        {
            bool shouldUseGenericType = true;
            if (parent.IsGenericType && parent.GetGenericTypeDefinition() != parent)
            {
                shouldUseGenericType = false;
            }

            if (parent.IsGenericType && shouldUseGenericType)
            {
                parent = parent.GetGenericTypeDefinition();
            }

            return parent;
        }

        public static bool TypeEqualsIn(this Type tipo, params Type[] tipos)
        {
            foreach (Type item in tipos)
            {
                if (tipo == item)
                {
                    return true;
                }
            }

            return false;
        }

        public static object GetDefault(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }
    }
}