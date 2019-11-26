using System.Globalization;
using System.Text;

namespace MusicProject.Common
{
    public static class Common
    {
        public static string SafeFileName(this string fileName)
        {
            
            return fileName.Replace(" ", "-").Replace("/", "-").Replace(":", "-").Replace(";", "-").Replace("<", "")
              .Replace(">", "").Replace("\"", "").Replace("|", "").Replace("?", "").Replace("*", "").Replace(".", "")
              .Replace(",", "").Replace("\\", "")
              .Replace("/", "").RemoveDiacritics();
        }

        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        public static string SafeFileNameWithExtension(this string fileName)
        {
            return fileName.Replace(" ", "-").Replace("/", "-").Replace(":", "-").Replace(";", "-").Replace("<", "")
              .Replace(">", "").Replace("\"", "").Replace("|", "").Replace("?", "").Replace("*", "")
              .Replace(",", "").Replace("\\", "")
              .Replace("/", "").RemoveDiacritics();
        }

        //public static string RemoveDiacritics(string text)
        //{
        //    var normalizedString = text.Normalize(NormalizationForm.FormD);
        //    var stringBuilder = new StringBuilder();

        //    foreach (var c in normalizedString)
        //    {
        //        var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
        //        if (unicodeCategory != UnicodeCategory.NonSpacingMark)
        //        {
        //            stringBuilder.Append(c);
        //        }
        //    }

        //    return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        //}
    }
}