using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Helper.Helpers
{
    public static class TypeHelper
    {
        private static List<Func<object, Type>> realTypeResolvers;
        private static List<Func<object, string, bool>> checkerPropProxies;
        public static readonly MethodInfo ConverterMethod = typeof(TypeHelper).GetMethod("Converter");

        static TypeHelper()
        {
            realTypeResolvers = new List<Func<object, Type>>();
            checkerPropProxies = new List<Func<object, string, bool>>();
        }

        public static bool HasDefaultConstructor(Type type)
        {
            return type.GetConstructor(Type.EmptyTypes) != null;
        }

        public static bool InitializedProxyProperty(object obj, string propName)
        {
            foreach (Func<object, string, bool> checker in checkerPropProxies)
            {
                if (!checker(obj, propName))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsIEnumerable(Type type)
        {
            return type.IsGenericType
                && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        public static bool IsNotCoreType(Type type)
        {
            return (type != typeof(object) && Type.GetTypeCode(type) == TypeCode.Object);
        }

        public static bool PropExists(Type tipo, string propertyName)
        {
            return tipo.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Any(c => c.Name == propertyName);
        }

        public static object GetPropValue(object objeto, string nome)
        {
            PropertyInfo prop = GetProperties(objeto.GetType()).FirstOrDefault(c => c.Name == nome);

            if (prop == null)
            {
                return null;
            }

            return prop.GetValue(objeto, null);
        }

        public static object GetFieldValue(object objeto, string nome)
        {
            FieldInfo field = objeto.GetType().GetField(nome, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (field == null)
            {
                return null;
            }

            return field.GetValue(objeto);
        }

        public static object GetValueNestedProperty(object objeto, string propertyName)
        {
            if (objeto == null || objeto is System.DBNull)
            {
                return null;
            }

            object value = objeto;

            foreach (PropertyInfo prop in propertyName.Split('.').Select(s => value.GetType().GetProperty(s)))
            {
                value = prop.GetValue(value, null);

                if (value == null)
                {
                    return null;
                }
            }

            return value;
        }

        public static object GetNestedPropertyValue(object source, string propertyName)
        {
            if (source == null)
            {
                return null;
            }

            PropertyInfo propInfo = null;
            Type type = source.GetType();
            object obj = source;

            foreach (string propName in propertyName.Split('.'))
            {
                propInfo = type.GetProperty(propName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                type = propInfo.PropertyType;
                obj = propInfo.GetValue(obj, null);
            }

            return obj;
        }

        public static object InvokeMethod(object obj, string methodName, params object[] parameters)
        {
            MethodInfo methodInfo = obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic);
            return methodInfo.Invoke(obj, parameters);
        }

        public static string GetPropertyCaller(int index = 1)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame frame = stackTrace.GetFrame(index);
            MethodBase method = frame.GetMethod();
            return method.Name.Replace("get_", "").Replace("set_", "");
        }

        public static void UpperStringProperties(object objeto)
        {
            IEnumerable<PropertyInfo> propriedades = TypeHelper.GetProperties(objeto.GetType())
                .Where(c => typeof(string).IsAssignableFrom(c.PropertyType) && c.CanWrite);

            foreach (PropertyInfo item in propriedades)
            {
                string valor = Convert.ToString(item.GetValue(objeto, null));

                if (!string.IsNullOrEmpty(valor))
                {
                    item.SetValue(objeto, valor.ToUpper(), null);
                }
            }
        }

        public static T GetFieldValue<T>(object objeto, string nome)
        {
            object valor = GetFieldValue(objeto, nome);
            return valor != null ? (T)valor : default(T);
        }

        public static T GetNestedPropertyValue<TSource, T>(object source, Expression<Func<TSource, object>> memberExpression)
        {
            string propertyName = ExpressionHelper.GetNestledPropertyName<TSource, object>(memberExpression);
            return (T)GetNestedPropertyValue(source, propertyName);
        }

        public static T Converter<T>(object value, CultureInfo cultureInfo = null)
        {
            Type toType = typeof(T);

            if (value == null || value is DBNull)
            {
                return default;
            }

            if (cultureInfo == null)
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }

            if (value is string)
            {
                if (toType == typeof(string[]))
                {
                    return value == null ? default(T) : (T)(object)((string)value).Split('|');
                }
                if (toType == typeof(Guid?))
                {
                    return Converter<T>(value == null ? default(Guid?) : new Guid(Convert.ToString(value, cultureInfo)), cultureInfo);
                }
                if (toType == typeof(Guid))
                {
                    return Converter<T>(new Guid(Convert.ToString(value, cultureInfo)), cultureInfo);
                }
                if ((string)value == string.Empty && toType != typeof(string))
                {
                    return Converter<T>(null, cultureInfo);
                }
            }
            else
            {
                if (typeof(T) == typeof(string))
                {
                    return Converter<T>(Convert.ToString(value, cultureInfo), cultureInfo);
                }
            }


            if (toType.IsGenericType &&
                toType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                toType = Nullable.GetUnderlyingType(toType); ;
            }

            if (toType.IsEnum && value != null)
            {
                return (T)Enum.Parse(toType, value.ToString());
            }
            else if (toType == typeof(TimeSpan) && value?.GetType() != typeof(TimeSpan))
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(TimeSpan));
                return (T)converter.ConvertFrom(value);
            }

            bool canConvert = toType is IConvertible || (toType.IsValueType && !toType.IsEnum);

            if (canConvert)
            {
                return (T)Convert.ChangeType(value, toType, cultureInfo);
            }
            return (T)value;
        }

        public static IList<FieldInfo> GetConstants(Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Static |
                BindingFlags.FlattenHierarchy).Where(c => c.IsLiteral && !c.IsInitOnly)
                .ToList();
        }

        public static IList<FieldInfo> GetFields(Type tipo)
        {
            return tipo.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
        }

        public static IList<FieldInfo> GetAllFields(Type tipo)
        {
            IList<FieldInfo> fieldsInfo = tipo.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            Type baseType = tipo.BaseType;

            while (baseType != null && baseType.FullName != "System.Object")
            {
                fieldsInfo = fieldsInfo.Concat(baseType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).ToArray();
                baseType = baseType.BaseType;
            }

            return fieldsInfo.ToList();
        }

        public static IList<FieldInfo> GetConstantFields(Type type)
        {
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public |
                 BindingFlags.Static | BindingFlags.FlattenHierarchy);

            return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
        }

        public static IList<PropertyInfo> GetPropertiesWritable(Type tipo)
        {
            return tipo.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(c => c.CanWrite).ToList();
        }

        public static IList<PropertyInfo> GetProperties(Type tipo)
        {
            return tipo.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
        }

        public static FieldInfo ObterField(Type tipo, string fieldName)
        {
            FieldInfo[] fieldsInfo = tipo.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            Type baseType = tipo.BaseType;

            while (baseType != null && baseType.FullName != "System.Object")
            {
                fieldsInfo = fieldsInfo.Concat(baseType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).ToArray();
                baseType = baseType.BaseType;
            }

            return fieldsInfo.FirstOrDefault(c => c.Name == fieldName);
        }

        public static FieldInfo GetDepthField(Type type, string fieldName)
        {
            FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic);

            if (field != null)
            {
                return field;
            }
            Type baseType = type.BaseType;

            while (baseType != null && baseType.FullName != "System.Object")
            {
                field = baseType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (field != null)
                {
                    return field;
                }

                baseType = baseType.BaseType;
            }

            return null;
        }

        public static FieldInfo GetBackFieldFromPropertyName(Type type, string propertyName)
        {
            string fieldName = string.Format("<{0}>k__BackingField", propertyName);
            FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (field == null)
            {
                Type baseType = type.BaseType;

                while (field == null && baseType != null && baseType.FullName != "System.Object")
                {
                    field = baseType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    baseType = baseType.BaseType;
                }
            }

            return field;
        }

        public static PropertyInfo GetProperty(Type tipo, string propertyName)
        {
            return tipo.GetProperty(propertyName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public static PropertyInfo GetProperty<T, T2>(object obj, Expression<Func<T, T2>> propExpression)
        {
            return GetNestedProperty(obj.GetType(), ExpressionHelper.GetNestledPropertyName(propExpression));
        }

        public static PropertyInfo GetProperty<T, T2>(Expression<Func<T, T2>> propExpression)
        {
            return GetNestedProperty(typeof(T), ExpressionHelper.GetNestledPropertyName(propExpression));
        }

        public static PropertyInfo GetNestedProperty(Type type, string prop)
        {
            PropertyInfo propInfo = null;

            foreach (string propName in prop.Split('.'))
            {
                propInfo = type.GetProperty(propName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                type = propInfo.PropertyType;
            }

            return propInfo;
        }

        public static Type GetRealType(object proxy)
        {
            if (realTypeResolvers != null && realTypeResolvers.Count > 0)
            {
                Type type = null;

                foreach (Func<object, Type> item in realTypeResolvers)
                {
                    type = item(proxy);

                    if (type != null)
                    {
                        return type;
                    }
                }
            }

            return proxy.GetType();
        }

        public static Type GetIEnumerableImpl(Type type)
        {
            if (IsIEnumerable(type))
            {
                return type;
            }

            Type[] t = type.FindInterfaces((m, o) => IsIEnumerable(m), null);

            return t[0];
        }

        public static void AddRealTypeResolver(Func<object, Type> resolver)
        {
            realTypeResolvers.Add(resolver);
        }

        public static void AddCheckerPropProxies(Func<object, string, bool> checker)
        {
            checkerPropProxies.Add(checker);
        }

        public static void ForPropertysOfType<T>(object objeto, Func<T, T> func)
        {
            IEnumerable<PropertyInfo> propriedades = TypeHelper.GetProperties(objeto.GetType())
                .Where(c => typeof(T).IsAssignableFrom(c.PropertyType) && c.CanWrite);

            foreach (PropertyInfo item in propriedades)
            {
                T valor = Converter<T>(item.GetValue(objeto, null));
                T novoValor = func(valor);
                item.SetValue(objeto, novoValor, null);
            }
        }

        public static void SetPropValue(object objeto, PropertyInfo prop, object valor)
        {
            prop.SetValue(objeto, valor, null);
        }

        public static void SetNestedPropertyValue<TSource>(object source, Expression<Func<TSource, object>> memberExpression, object value)
        {
            string propertyName = ExpressionHelper.GetNestledPropertyName<TSource, object>(memberExpression);
            SetNestedPropertyValue(source, propertyName, value);
        }

        public static void SetNestedPropertyValue(object source, string propertyName, object value)
        {
            if (source == null)
            {
                return;
            }

            PropertyInfo propInfo = null;
            Type type = source.GetType();
            object obj = source;

            string[] properties = propertyName.Split('.');

            for (int i = 0; i < properties.Length; i++)
            {
                propInfo = type.GetProperty(properties[i], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                type = propInfo.PropertyType;

                if (i != properties.Length - 1)
                {
                    obj = propInfo.GetValue(obj, null);
                }
            }

            if (obj != null)
            {
                propInfo.SetValue(obj, value);
            }
        }

        public static void TrimStringProperties(object objeto)
        {
            IEnumerable<PropertyInfo> propriedades = TypeHelper.GetProperties(objeto.GetType())
                .Where(c => typeof(string).IsAssignableFrom(c.PropertyType) && c.CanWrite);

            foreach (PropertyInfo item in propriedades)
            {
                string valor = Convert.ToString(item.GetValue(objeto, null));

                if (!string.IsNullOrEmpty(valor))
                {
                    item.SetValue(objeto, valor.Trim(), null);
                }
            }
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
    }
}