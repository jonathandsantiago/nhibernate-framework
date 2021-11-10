using Framework.Helper.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Framework.Helper.Helpers
{
    public class StringHelper
    {
        private static int GetRestCalcCnpj(int digits, string cnpj, int[] multiplicator)
        {
            int sum = 0;
            for (int index = 0; index < digits; index++)
            {
                sum += int.Parse(cnpj[index].ToString()) * multiplicator[index];
            }

            int rest = (sum % 11);
            return rest < 2 ? 0 : (11 - rest);
        }

        public static bool Equals(string str1, string str2, bool toLower = false)
        {
            return toLower ? ToLower(str1) == ToLower(str2) : str1 == str2;
        }

        public static bool EndsWith(string str, string value, bool toLower = false)
        {
            if (str == null)
            {
                return false;
            }

            if (toLower)
            {
                str = str.ToLower();
            }

            return str.EndsWith(value);
        }

        public static bool IsValidUrl(string str)
        {
            return Uri.IsWellFormedUriString(str, UriKind.RelativeOrAbsolute);
        }

        public static bool IsCelular(string str)
        {
            if (string.IsNullOrEmpty(str) || str.StartsWith("00"))
            {
                return false;
            }

            str = str.Replace(" ", "");

            if (str.Length < 9)
            {
                return false;
            }

            return Regex.IsMatch(str, @"^\(\d{3}\)\d{1}\d{4}\d{4}$") ||
                Regex.IsMatch(str, @"^\(\d{3}\)\d{1}\d{4}-\d{4}$") ||
                Regex.IsMatch(str, @"^\(\d{2}\)\d{1}\d{4}-\d{4}$") ||
                Regex.IsMatch(str, @"^\d{2}\d{1}\d{4}-\d{4}$") ||
                (Regex.IsMatch(str, @"^\d{3}\d{5}\d{4}") && str.Substring(3, str.Length - 3).StartsWith("9")) ||
                (Regex.IsMatch(str, @"^\d{4}\d{5}\d{4}") && str.Substring(4, str.Length - 4).StartsWith("9")) ||
                (Regex.IsMatch(str, @"^\d{2}\d{5}\d{4}$") && str.Substring(2, str.Length - 2).StartsWith("9")) ||
                (Regex.IsMatch(str, @"^\d{5}\d{4}$") && str.StartsWith("9"));
        }

        public static bool IsCnpj(string cnpj)
        {
            if (IsNullOrEmpty(cnpj))
            {
                return false;
            }

            cnpj = OnlyNumbers(cnpj.Trim());

            if (cnpj.Length != 14)
            {
                return false;
            }

            string hasCnpj = cnpj.Substring(0, 12);
            int resto = GetRestCalcCnpj(12, hasCnpj, new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 });

            string digits = resto.ToString();
            hasCnpj = hasCnpj + digits;
            resto = GetRestCalcCnpj(13, hasCnpj, new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 });

            digits = digits + resto.ToString();
            return cnpj.EndsWith(digits);
        }

        public static bool IsCpf(string cpf)
        {
            if (IsNullOrEmpty(cpf))
            {
                return false;
            }

            cpf = OnlyNumbers(cpf.Trim());

            if (cpf.Length != 11)
            {
                return false;
            }

            string hasCnpj = cpf.Substring(0, 9);
            int resto = GetRestCalcCnpj(9, hasCnpj, new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 });

            string digits = resto.ToString();
            hasCnpj = hasCnpj + digits;
            resto = GetRestCalcCnpj(10, hasCnpj, new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 });

            digits = digits + resto.ToString();
            return cpf.EndsWith(digits);
        }

        public static bool IsEmail(string str)
        {
            return new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Match(str).Success;
        }

        public static bool IsNullOrEmpty(params string[] values)
        {
            foreach (string str in values)
            {
                if (IsNullOrEmpty(str))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsNullOrEmpty(string str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        }

        public static bool IsNullOrEmpty(object str)
        {
            return str == null || IsNullOrEmpty(str.ToString());
        }

        public static bool IsPhone(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            str = str.Replace(" ", "");

            if (str.Length < 8)
            {
                return false;
            }

            return Regex.IsMatch(str, @"^\(\d{3}\)\d{4}\d{4}$") ||
                Regex.IsMatch(str, @"^\(\d{3}\)\d{4}-\d{4}$") ||
                Regex.IsMatch(str, @"^\(\d{2}\)\d{4}-\d{4}$") ||
                Regex.IsMatch(str, @"^\d{2}\d{4}-\d{4}$") ||
                (Regex.IsMatch(str, @"^\d{3}\d{4}\d{4}$") && str.StartsWith("0")) ||
                Regex.IsMatch(str, @"^\d{2}\d{4}\d{4}$") ||
                Regex.IsMatch(str, @"^\d{4}\d{4}$") ||
                Regex.IsMatch(str, @"^\d{4}-\d{4}$");
        }

        public static bool IsNumber(string str)
        {
            return Regex.IsMatch(str, @"^\d+$");
        }

        public static bool IsSmaller(string str, int length)
        {
            return IsNullOrEmpty(str) || str.Length < length;
        }

        public static bool StartWithIn(string str, params string[] values)
        {
            if (str == null)
            {
                return false;
            }

            return str.StartWithIn(values);
        }

        public static string Base64Encode(string plainText)
        {
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            byte[] base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string BreakLine(int count = 1)
        {
            if (count <= 0)
            {
                throw new ArgumentException("Count deve ser maior que zero.");
            }

            string breaks = string.Empty;

            for (int i = 0; i < count; i++)
            {
                breaks += Environment.NewLine;
            }

            return breaks;
        }

        public static string FirstToUpper(string str)
        {
            return ToUpper(str.Substring(0, 1)) + ToLower(str.Substring(1, str.Length - 1));
        }

        public static string FormatCpfCnpj(string cpfCnpj)
        {
            try
            {
                if (IsNullOrEmpty(cpfCnpj))
                {
                    return string.Empty;
                }

                string valor = Regex.Replace(cpfCnpj.ToString(), @"[^\d]", "");

                return string.Format(valor.Length <= 11 ? @"{0:000\.000\.000\-00}" : @"{0:00\.000\.000\/0000\-00}", Convert.ToInt64(valor));
            }
            catch
            {
                return cpfCnpj;
            }
        }

        public static string FormatPhone(string Phone)
        {
            if (IsNullOrEmpty(Phone))
            {
                return Phone;
            }

            string valor = Regex.Replace(Phone.ToString(), @"[^\d]", "");

            if (valor.Length.In(10, 11) && valor.First() != '0')
            {
                valor = "0" + valor;
            }

            return string.Format(valor.Length <= 11 ? @"{0:\(000\) 0000\-0000}" : @"{0:\(000\) 00000\-0000}", Convert.ToInt64(valor));
        }

        public static string FormatCellWithBrazilCode(string telefone)
        {
            if (IsNullOrEmpty(telefone))
            {
                return telefone;
            }

            string valor = Regex.Replace(telefone.ToString(), @"[^\d]", "");

            return string.Format(@"{0:\+00 (00\) 00000\-0000}", Convert.ToInt64(valor));
        }

        public static string FormatIfIsNotNullOrEmpty(string str, string format)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return string.Format(format, str);
        }

        public static string GetDiff(object str1, object str2, string modificationName, string separator = "\r\n")
        {
            string str = string.Empty;

            str1 = str1 == null ? str1 = string.Empty : str1;
            str2 = str2 == null ? str2 = string.Empty : str2;

            if (str1.ToString() != str2.ToString())
            {
                str = modificationName + ": " + str1.ToString() + separator;
            }

            return str;
        }

        public static string GetInitials(string name)
        {
            name = RemoveMask(name) ?? string.Empty;

            string[] itens = name.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            if (itens.Length > 0)
            {
                name = itens[0].FirstOrDefault().ToString().ToUpper();
            }

            if (itens.Length > 1)
            {
                name += itens[itens.Length - 1].FirstOrDefault().ToString().ToUpper();
            }

            return name;
        }

        public static string GetMemberName<T>(Expression<Func<T, object>> memberExpression)
        {
            return ExpressionHelper.GetNestledPropertyName(memberExpression);
        }

        public static string Join<T>(IEnumerable<T> values, string separator = ", ")
        {
            return string.Join(separator, values);
        }

        public static string JoinIfNotNullOrEmpty(string separator, params string[] strings)
        {
            List<string> str = new List<string>();

            foreach (string item in strings)
            {
                if (!IsNullOrEmpty(item))
                {
                    str.Add(item);
                }
            }

            return string.Join(separator, str);
        }

        public static string OnlyNumbers(string str)
        {
            return str == null ? null : string.Join(string.Empty, str.ToCharArray().Where(char.IsDigit));
        }

        public static string Pluralize(string str, IEnumerable itens)
        {
            return itens.Cast<object>().Count() > 1 ? str + "s" : str;
        }

        public static string RemoveCharacters(string str, params char[] characters)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return string.Concat(str.Where(c => !characters.Any(d => d == c)));
        }

        public static string RemoveLineBreak(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return str.Replace(Environment.NewLine, " ").Replace("\n", " ");
        }

        public static string RemoveMask(string str)
        {
            if (IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            char[] arr = str.Where(c => (char.IsLetterOrDigit(c) ||
                             char.IsWhiteSpace(c))).ToArray();

            return new string(arr);
        }

        public static string RemoveSpecialCharacters(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            text = text.Normalize(NormalizationForm.FormD);
            char[] chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        public static string Substring(string str, int startIndex, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return str.Substring(startIndex, maxLength > str.Length ? str.Length : maxLength);
        }

        public static string ToUpper(string str)
        {
            return !string.IsNullOrEmpty(str) ? str.ToUpper() : str;
        }

        public static string ToLower(string str)
        {
            return !string.IsNullOrEmpty(str) ? str.ToLower() : str;
        }

        public static string Trim(string str)
        {
            return str != null ? str.Trim() : str;
        }

        public static Stream ToStream(string str)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}