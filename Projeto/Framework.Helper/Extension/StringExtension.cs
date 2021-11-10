using Framework.Helper.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Helper.Extension
{
    public static class StringExtensions
    {
        private static Encoding enconding = Encoding.GetEncoding("iso-8859-8");

        public static bool Contains(this string str, params string[] valores)
        {
            foreach (string item in valores)
            {
                if (str.Contains(item))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool ContainsIgnoreCase(this string a, string b)
        {
            return a.ToLower().RemoveAccents().Contains(b.ToLower().RemoveAccents());
        }

        public static bool In(this string str, params string[] values)
        {
            foreach (string value in values)
            {
                if (str == value)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool LikeIgnoreCase(this string a, string b)
        {
            return a.ToLower().RemoveAccents().Contains(b.ToLower().RemoveAccents());
        }

        public static bool NotIn(this string str, params string[] valores)
        {
            return !valores.Any(c => c == str);
        }

        public static bool StartWithIn(this string str, params string[] values)
        {
            foreach (string value in values)
            {
                if (str.StartsWith(value))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool StartsWithIgnoreCase(this string a, string b)
        {
            return a.ToLower().RemoveAccents().StartsWith(b.ToLower().RemoveAccents());
        }

        public static bool TrimIsNullOrEmpty(this string str)
        {
            return str == null || string.IsNullOrEmpty(str.Trim());
        }

        public static int ToIntDef(this string str, int def = 0)
        {

            if (!Int32.TryParse(str, out int numero))
            {
                return def;
            }

            return numero;
        }

        public static string ConcatIf(this string str, Func<bool> func, string strInsert)
        {
            if (func())
            {
                str += strInsert;
            }

            return str;
        }

        public static string ConcatIf(this string str, bool predicate, string strInsert)
        {
            if (predicate)
            {
                str += strInsert;
            }

            return str;
        }

        public static string ConcatIfNotNullOrEmpty(this string str, string strCheck)
        {
            if (!StringHelper.IsNullOrEmpty(strCheck))
            {
                str += strCheck;
            }

            return str;
        }

        public static string ConcatIfNotNullOrEmpty(this string str, string strCheck, string strInsert)
        {
            if (!StringHelper.IsNullOrEmpty(strCheck))
            {
                str += strInsert;
            }

            return str;
        }

        public static string ConcatFormatIfNotNullOrEmpty(this string str, string strCheck, string strFormatInsert)
        {
            if (!StringHelper.IsNullOrEmpty(strCheck))
            {
                str += string.Format(strFormatInsert, strCheck);
            }

            return str;
        }

        public static string InsertIf(this string value, Func<bool> predicate, string valueInsert)
        {
            if (predicate())
            {
                return value + valueInsert;
            }

            return value;
        }

        public static string InsertIf(this string value, Func<bool> predicate, Func<string> func)
        {
            if (predicate())
            {
                return value + func();
            }

            return value;
        }

        public static string InsertIf(this string value, bool insert, string valueInsert)
        {
            if (insert)
            {
                return value + valueInsert;
            }

            return value;
        }

        public static string InsertIf(this string value, bool insert, string valueTrue, string valueFalse)
        {
            return value + (insert ? valueTrue : valueFalse);
        }

        public static string InsertStartIf(this string value, Func<bool> predicate, string valueInsert)
        {
            if (predicate())
            {
                return value + valueInsert;
            }

            return value;
        }

        public static string InsertStartNotEqual(this string value, string valueInsert)
        {
            if (value.IndexOf(valueInsert) != 0)
            {
                value = valueInsert + value;
            }

            return value;
        }

        public static string InsertEndIsNotEqual(this string value, string caracter)
        {
            if (value.Last().ToString() != caracter)
            {
                value += caracter;
            }

            return value;
        }

        public static string PadLeft(this string value, int qtdZeros, bool onlyNumbers = true)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            if (onlyNumbers && value.Any(c => !Char.IsDigit(c)))
            {
                return value;
            }

            return value.PadLeft(qtdZeros, '0');
        }

        public static string Pluralize(this string str, IEnumerable enumerable)
        {
            return StringHelper.Pluralize(str, enumerable);
        }

        public static string RemoveAccents(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            return enconding.GetString(Encoding.Convert(Encoding.UTF8, enconding, Encoding.UTF8.GetBytes(str)));
        }

        public static string RemoveBlankSpaces(this string str)
        {
            return str.Replace(" ", string.Empty);
        }

        public static string RemoveFromEnd(this string str, params string[] values)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            foreach (var item in values)
            {
                if (str.EndsWith(item))
                {
                    str = str.Substring(0, str.Length - item.Length);
                }
                else if (str.StartsWith(item))
                {
                    str = str.Substring(item.Length, str.Length - item.Length);
                }
            }

            return str;
        }

        public static string RemoverNoInicio(this string texto, string textoRemover)
        {
            if (texto.StartsWith(textoRemover))
            {
                texto = texto.Remove(texto.IndexOf(textoRemover), textoRemover.Length);
            }

            return texto;
        }

        public static string RemoverNoFim(this string texto, string textoRemover)
        {
            if (texto.EndsWith(textoRemover))
            {
                texto = texto.Remove(texto.LastIndexOf(textoRemover));
            }

            return texto;
        }

        public static string RemovePrepositions(this string str, bool removeWhiteSpace = false)
        {
            return str.ReplaceIn(string.Empty, " da ", " de ", " do ", " das ", " dos ", " com ", removeWhiteSpace ? " " : "");
        }

        public static string ReplaceIn(this string str, string newValue, params string[] oldValues)
        {
            foreach (string oldValue in oldValues)
            {
                str = str.Replace(oldValue, newValue);
            }

            return str;
        }

        public static string SetArgs(this string str, params object[] args)
        {
            return string.Format(str, args);
        }

        public static string SplitFirst(this string str, string separator = " ")
        {
            string[] items = str.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            return items.Length > 0 ? items[0] : string.Empty;
        }

        public static string SplitFirstAndLast(this string str, string splitSeparator = " ", string stringSeparator = " ")
        {
            string[] items = str.Split(new string[] { splitSeparator }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length == 0)
            {
                return string.Empty;
            }

            return items.Length > 1 ? items[0] + stringSeparator + items[items.Length - 1] : items[0];
        }

        public static string Substring(this string str, string strBegin, string strEnd, int countAfterBegin = 0)
        {
            int indexBegin = str.IndexOf(strBegin);

            if (indexBegin < 0)
            {
                return null;
            }

            indexBegin += countAfterBegin;

            int indexEnd = str.IndexOf(strEnd, indexBegin);

            if (indexEnd < 0)
            {
                indexEnd = str.Length;
            }

            return str.Substring(indexBegin, indexEnd - indexBegin);
        }

        public static string Substring(this string str, string strBegin, int countAfterBegin = 0)
        {
            int indexBegin = str.IndexOf(strBegin);

            if (indexBegin < 0)
            {
                return null;
            }

            indexBegin += countAfterBegin;

            return str.Substring(indexBegin, str.Length - indexBegin);
        }

        public static string ToString(this bool boolean, string language)
        {
            if (language.ToLower() == "pt-br")
            {
                return boolean ? "Sim" : "Não";
            }

            throw new System.NotImplementedException();
        }
        
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static string[] Split(this string str, string separator, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries, int count = 0)
        {
            return count > 0 ?
                str.Split(new string[] { separator }, count, options) :
                str.Split(new string[] { separator }, options);
        }

        public static TResult ParseEnum<TResult>(this string value)
        {
            return (TResult)Enum.Parse(typeof(TResult), value, true);
        }

        public static IList<string> SplitList(this string str, char separator)
        {
            return str.Split(separator).ToList();
        }

        public static string ToFirstUpper(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return $"{char.ToUpperInvariant(str[0])}{str.Substring(1).ToLowerInvariant()}";
            }

            return str;
        }

        public static string ToCamelCase(this string str)
        {
            IList<string> values = new List<string>();

            foreach (string value in str.Split(' '))
            {
                if (!string.IsNullOrEmpty(value) && value.Length > 1)
                {
                    values.Add($"{char.ToUpperInvariant(value[0])}{value.Substring(1).ToLower()}");
                }
            }

            return string.Join(" ", values);
        }
    }
}