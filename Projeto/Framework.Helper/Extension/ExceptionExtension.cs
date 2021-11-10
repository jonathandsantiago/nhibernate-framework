using System;

namespace Framework.Helper.Extension
{
    public static class ExceptionExtension
    {
        public static string GetCompleteMessage(this Exception ex)
        {
            string message = ex.Message;

            while (ex.InnerException != null)
            {
                message += $"{Environment.NewLine}{ex.InnerException.Message}";
                ex = ex.InnerException;
            }

            return message;
        }

        public static string GetCompleteStackTrace(this Exception ex)
        {
            string stack = ex.StackTrace;

            while (ex.InnerException != null)
            {
                stack += $"{Environment.NewLine}{ex.InnerException.StackTrace}";
                ex = ex.InnerException;
            }

            return stack;
        }
    }
}
