using System;

namespace CommonQuizFramework.CommonClass
{
    public class ByteSizeFormatter
    {
        public static string FormatBytes(long bytes)
        {
            string[] sizes = { "Bytes", "KB", "MB" };
            double len = bytes;
            var order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{Math.Round(len, 1)} {sizes[order]}";
        }
    }
}