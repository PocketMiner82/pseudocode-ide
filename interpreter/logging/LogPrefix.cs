using System;

namespace pseudocodeIde.interpreter.logging
{
    public static class LogPrefix
    {
        public const string INFO = "[INFO ] ";

        public const string ERROR = "[ERROR] ";

        public static string TIMESTAMP
        {
            get
            {
                return DateTime.Now.ToString("HH:mm:ss.fff") + "   ";
            }
        }
    }
}
