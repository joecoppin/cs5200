using System;
using System.Linq;

namespace Utils
{
    public delegate void StringMethod(string message);

    public static class HelperFunctions
    {
        public static string ComputeNthLabel(Int32 n)
        {
            string prefix = (n < 0) ? "-" : string.Empty;
            string suffix = "th";

            n = Math.Abs(n);
            Int32 mod10 = (n % 10);
            if (mod10 == 1 && n != 11)
                suffix = "st";
            else if (mod10 == 2 && n != 12)
                suffix = "nd";
            else if (mod10 == 3 && n != 13)
                suffix = "rd";

            return $"{prefix}{n}{suffix}";
        }

        public static string ByteToStringDisplay(byte[] data)
        {
            string result = string.Empty;
            result = (data == null) ? "null" : data.Aggregate(result, (current, b) => current + $" {b.ToString("D3"),3}");
            return result;
        }

        public static string IntArrayToString(int[] values)
        {
            string result = string.Empty;
            if (values != null && values.Length > 0)
            {
                foreach (int v in values)
                {
                    if (result != string.Empty)
                        result += ", ";
                    result += v.ToString();
                }
            }
            return result;
        }
    }
}
