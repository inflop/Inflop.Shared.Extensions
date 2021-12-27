using System.IO;

namespace Inflop.Shared.Extensions
{
    public static class FileExtensions
    {
        public static string GetSHA256HashFromFile(this string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Nie odnaleziono pliku w podanej ścieżce", filePath);

            string result = string.Empty;

            using (FileStream fileStream = File.OpenRead(filePath))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    fileStream.CopyTo(ms);
                    result = ms.ToArray().ToSha256Hash();
                }
                fileStream.Close();
            }

            return result;
        }
    }
}