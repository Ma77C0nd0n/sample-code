using System.Text.RegularExpressions;

namespace DocumentProcessingService.app.Infrastructure.Extensions
{
    public static class FileNameExtensions
    {
        public static bool IsValidFileName(this string fileName)
        {
            var patternForValidFileName = @"\w_\w*\.\w";
            return Regex.IsMatch(fileName, patternForValidFileName, RegexOptions.IgnoreCase);
        }

        public static string GetDocumentId(this string fileName)
        {
            var documentId = Regex.Match(fileName, @"^.*?(?=_)");
            return documentId.Value;
        }
        
        public static string GetDocumentName(this string fileName)
        {
            var documentId = Regex.Match(fileName, @"(?<=_).*?(?=\.)");
            return documentId.Value;
        }
    }
}
