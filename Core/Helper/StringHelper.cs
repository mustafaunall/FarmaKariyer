namespace Core.Helper;

public static class StringHelper
{
    public static string LimitString(this string str, int limit = 50)
    {
        if (str.Length == 0) return string.Empty;

        if (str.Length > limit)
        {
            return str.Substring(0, limit) + "...";
        }
        else
        {
            return str;
        }
    }

    public static string ConvertToEnglish(string input)
    {
        Dictionary<char, char> trToEng = new Dictionary<char, char>()
        {
            { 'ç', 'c' },
            { 'ğ', 'g' },
            { 'ı', 'i' },
            { 'ö', 'o' },
            { 'ş', 's' },
            { 'ü', 'u' },
            { 'Ç', 'C' },
            { 'Ğ', 'G' },
            { 'İ', 'I' },
            { 'Ö', 'O' },
            { 'Ş', 'S' },
            { 'Ü', 'U' }
        };

        char[] chars = input.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (trToEng.ContainsKey(chars[i]))
            {
                chars[i] = trToEng[chars[i]];
            }
        }

        return new string(chars);
    }

    public static string ToCensoredAdvertTitle(string input)
    {
        if (input.Length == 0) return input;

        string[] words = input.Split(' ');

        for (int i = 0; i < words.Length; i++)
        {
            words[i] = ToCensoredText(words[i]);
        }

        return string.Join(" ", words);
    }

    public static string ToCensoredText(string input)
    {
        if (input.Length == 0) return input;

        char firstChar = input[0];
        string maskedPart = new string('*', 5);

        return $"{firstChar}{maskedPart}";
    }
}
