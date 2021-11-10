namespace Framework.Helper.Extension
{
    public static class IntegerExtension
    {
        public static bool Between(this int valorInteiro, int valorInicial, int valorFinal)
        {
            return valorInteiro >= valorInicial && valorInteiro <= valorFinal;
        }

        public static bool EqualsAndNotDefault(this int intValue, int intValueToCompare)
        {
            return intValue == intValueToCompare && intValue != default;
        }

        public static string PadLeft(this int intVaule, int totalWidth, char paddingChar)
        {
            return intVaule.ToString().PadLeft(totalWidth, paddingChar);
        }
    }
}