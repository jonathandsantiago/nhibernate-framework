using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Helper.Helpers
{
    public static class DecimalHelper
    {
        public static bool IsDecimal(string str)
        {
            return decimal.TryParse(str, out decimal numero);
        }

        public static bool HasDecimalPlaces(decimal value)
        {
            return value % 1 != 0;
        }

        public static bool TryParse(object obj, out decimal value)
        {
            bool result = false;
            value = 0;

            try
            {
                value = Convert.ToDecimal(obj);
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public static decimal Bigger(params decimal[] values)
        {
            return values.Max();
        }

        public static decimal CalculatePercent(decimal valueA, decimal valueB)
        {
            return Round(valueB != 0 ? (valueA / valueB) * 100 : 0);
        }

        public static decimal CalculatePercent(decimal valueA, decimal valueB, int numberDecimal = 2)
        {
            return Round((valueB > 0 ? (valueA / valueB) * 100 : 0), numberDecimal);
        }

        public static decimal CalculateChangePercent(decimal valueA, decimal valueB, int numberDecimal = 6)
        {
            if (valueB == 0)
            {
                return 0;
            }

            return Round(((valueA / valueB) * 100) - 100, numberDecimal);
        }

        public static decimal CalculateOverRoundSplit(decimal value, decimal divisor, int numberDecimal = 2)
        {
            decimal resultado = Math.Round(value / divisor, numberDecimal);
            return Math.Round(resultado * divisor, numberDecimal);
        }

        public static decimal Divide(decimal value, decimal divisor, int numberDecimal = 2)
        {
            return value == 0 || divisor == 0 ? 0 : Math.Round(value / divisor, numberDecimal);
        }

        public static decimal DivideAndRound(decimal value, decimal divisor, int numberDecimal = 2)
        {
            return Divide(value, divisor, numberDecimal);
        }

        public static decimal InverterSign(decimal value, bool inverter = true)
        {
            if (inverter)
            {
                value = value * -1;
            }

            return value;
        }

        public static decimal MaxValue()
        {
            return 99999999999.99999999M;
        }

        public static decimal Round(decimal value, int numeroCasasDecimais = 2)
        {
            return Math.Round(value, numeroCasasDecimais);
        }

        public static decimal SetPercent(decimal value, decimal percent, int numberDecimal = 2)
        {
            return Round((percent != 0 ? ((percent / 100M) * value) : 0), numberDecimal);
        }

        public static decimal Smaller(params decimal[] values)
        {
            return values.Min();
        }

        public static double CalculatePercentDouble(double valueA, double valueB)
        {
            return valueB > 0 ? (valueA / valueB) * 100 : 0;
        }

        public static int CalculatePercentInteiro(double valueA, double valueB)
        {
            return valueB > 0 ? Convert.ToInt32((valueA / valueB) * 100) : 0;
        }

        public static string FormatDecimalPlaces(decimal value, int minPlaces, int maxPlaces)
        {
            int places = BitConverter.GetBytes(decimal.GetBits(value)[3])[2];
            return value.ToString("N" + (places > maxPlaces ? maxPlaces : (places < minPlaces ? minPlaces : places)).ToString());
        }

        public static void Rate<T>(IList<T> itens, Expression<Func<T, decimal>> valueMemberExpression, decimal valueRate, Action<T, decimal> onRatear = null)
        {
            Func<T, decimal> func = valueMemberExpression.Compile();
            System.Reflection.PropertyInfo property = TypeHelper.GetProperty(valueMemberExpression);
            decimal valorTotalAtual = itens.Sum(func);
            decimal valorTotalDesejado = valorTotalAtual + valueRate;
            decimal diferenca = valueRate;

            for (int i = 0; i < 3 && diferenca != 0; i++)
            {
                foreach (T item in itens)
                {
                    decimal valorAtual = (decimal)property.GetValue(item);
                    decimal valorAplicado = DecimalHelper.Round((valorAtual / valorTotalAtual) * diferenca);

                    onRatear?.Invoke(item, valorAplicado);
                    property.SetValue(item, valorAtual + valorAplicado);
                }

                valorTotalAtual = itens.Sum(func);
                diferenca = valorTotalDesejado - valorTotalAtual;
            }

            for (int i = 0; i < 3 && diferenca != 0; i++)
            {
                foreach (T item in itens)
                {
                    if (diferenca == 0)
                    {
                        break;
                    }

                    if ((decimal)property.GetValue(item) <= 0)
                    {
                        continue;
                    }

                    decimal valorAtual = (decimal)property.GetValue(item);
                    decimal cent = diferenca < 0 ? -0.01M : 0.01M;
                    property.SetValue(item, valorAtual + cent);
                    diferenca += cent * -1;

                    onRatear?.Invoke(item, cent);
                }
            }

            if (diferenca != 0)
            {
                T item = itens.Where(c => func(c) > 0)
                    .OrderByDescending(c => func(c))
                    .FirstOrDefault();

                if (item == null)
                {
                    throw new InvalidOperationException("Não foi possível ratear.");
                }

                decimal valorAtual = (decimal)property.GetValue(item);
                property.SetValue(item, valorAtual + diferenca);

                onRatear?.Invoke(item, diferenca);
            }
        }
    }
}