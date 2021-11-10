namespace Framework.Helper.Helpers
{
    public static class ConvertHelper
    {
        public static int SafeToInt(string str)
        {
            int.TryParse(str, out int number);
            return number;
        }
    }
}
