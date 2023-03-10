using System.Text;

namespace FSTools
{
    public static class Extensions
    {
        public static string[] FSSplit(this string input, char separator, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
        {
            return input.FSSplit(separator, int.MaxValue, stringSplitOptions);
        }
        public static string[] FSSplit(this string input, char separator, int count = int.MaxValue, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
        {
            FSList<string> list = new FSList<string>();

            if (count < 1)
            {
                throw new ArgumentException();
            }

            int sections = 1;
            string tmp = "";
            foreach (char c in input)
            {
                if (c == separator)
                {
                    if (sections < count)
                    {
                        if (tmp.Length == 0 && stringSplitOptions.HasFlag(StringSplitOptions.RemoveEmptyEntries))
                        {
                            continue;
                        }
                        else
                        {
                            list.Add(tmp);
                            tmp = "";
                            sections++;
                        }
                    }
                    else
                    {
                        tmp += c;
                    }
                }
                else
                {
                    tmp += c;
                }
            }

            if (tmp.Length == 0 && stringSplitOptions.HasFlag(StringSplitOptions.RemoveEmptyEntries))
            {

            }
            else
            {
                list.Add(tmp);
            }

            if (count != int.MaxValue && !stringSplitOptions.HasFlag(StringSplitOptions.RemoveEmptyEntries))
            {
                while (list.Count < count)
                {
                    list.Add("");
                }
            }

            return list.ToArray();
        }

        public static char[] FSToCharArray(this string input)
        {
            char[] result = new char[input.Length];
            int i = 0;
            while (i < input.Length)
            {
                result[i] = input[i];
                i++;
            }
            return result;
        }

        public static string FSTrimString(this string input, string strToTrim)
        {
            StringBuilder newStr = new StringBuilder();
            for (int i = strToTrim.Length; i < input.Length; i++)
            {
                newStr.Append(input[i]);
            }

            return newStr.ToString();
        }

        public static string FSTrimEnd(this string input, string toTrim)
        {
            int lastIndex = input.Length - 1;
            int trimIndex = toTrim.Length - 1;
            while (lastIndex >= 0)
            {
                if (input[lastIndex] == toTrim[trimIndex])
                {
                    lastIndex--;
                    trimIndex--;
                    if (trimIndex < 0)
                    {
                        trimIndex = toTrim.Length - 1;
                    }
                    continue;
                }
                else
                {
                    break;
                }
            }
            string result = new string(input.FSToCharArray(), 0, lastIndex + 1);
            return result;
        }

        public static byte[] FSToByteArray(this int[] source)
        {
            byte[] result = new byte[source.Length * 4];
            for (int i = 0; i < source.Length; i++)
            {
                result[i * 4] = (byte)(source[i] >> 24);
                result[i * 4 + 1] = (byte)(source[i] >> 16);
                result[i * 4 + 2] = (byte)(source[i] >> 8);
                result[i * 4 + 3] = (byte)(source[i]);
            }
            return result;
        }
        public static int[] FSToIntArray(this byte[] source)
        {
            int[] result = new int[source.Length / 4];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)(source[i * 4] << 24);
                result[i] = (byte)(source[i * 4 + 1] << 16);
                result[i] = (byte)(source[i * 4 + 2] << 8);
                result[i] = (byte)(source[i * 4 + 3]);
            }
            return result;
        }
    }

}
