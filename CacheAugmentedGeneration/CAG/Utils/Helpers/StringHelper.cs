using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace CAG.Utils.Helpers;
public static class StringHelper
{

    public static string NormalizeString(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // 1. Unicode normalizasyonu (FormD ile ayrıştırılmış karakterler)
        string normalized = input.Normalize(NormalizationForm.FormD);
        // 2. Unicode kategori filtresi: Harf ve sayı dışında kalanları kaldır
        StringBuilder builder = new();
        foreach (char c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(c);
            }
        }

        // 3. Temizlenmiş metni tekrar normalize et (FormC)
        normalized = builder.ToString().Normalize(NormalizationForm.FormC);
        // 4. Küçük harfe çevirme
        normalized = normalized.ToLowerInvariant();
        // 5. Özel karakterleri ve fazla boşlukları temizleme
        normalized = Regex.Replace(normalized, @"[^\w\s]", ""); // Harf, sayı ve boşluk dışındaki karakterleri kaldır
        normalized = Regex.Replace(normalized, @"\s+", " ");    // Fazla boşlukları tek bir boşlukla değiştir
        normalized = normalized.Trim();                        // Baş ve sondaki boşlukları kaldır

        return normalized;
    }

    public static List<string> SplitWord(string word)
    {
        return [.. word.Split()];
    }
}
