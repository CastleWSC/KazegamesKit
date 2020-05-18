using System.Text.RegularExpressions;

namespace KazegamesKit
{
    public static class StringExtension 
    {

        public static string FirstCharToUpper(this string input)
        {
            if (input == null)
                return "";

            if (input.Length > 1)
                return char.ToUpper(input[0]) + input.Substring(1);

            return input.ToUpper();
        }

        public static bool IsUrl(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string v_pattern = @"((ht|f)tp(s?)\:\/\/)?www[.][0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?";
                if (Regex.IsMatch(str, v_pattern))
                    return true;
            }
            return false;
        }

        public static string ReplaceFirst(string text, string oldText, string newText)
        {
            try
            {
                text = text != null ? text : "";
                oldText = oldText != null ? oldText : "";
                newText = newText != null ? newText : "";

                int pos = string.IsNullOrEmpty(oldText) ? -1 : text.IndexOf(oldText);
                if (pos < 0)
                    return text;

                string result = text.Substring(0, pos) + newText + text.Substring(pos, oldText.Length);
                return result;
            }
            catch
            {
                return text;
            }
        }

        public static string ReplaceLast(string text, string oldText, string newText)
        {
            try
            {
                text = text != null ? text : "";
                oldText = oldText != null ? oldText : "";
                newText = newText != null ? newText : "";

                int pos = string.IsNullOrEmpty(oldText) ? -1 : text.IndexOf(oldText);
                if (pos < 0)
                    return text;

                string result = text.Remove(pos, oldText.Length).Insert(pos, newText);
                return result;
            }
            catch
            {
                return text;
            }
        }
    }
}
