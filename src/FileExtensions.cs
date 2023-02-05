namespace Inflop.Shared.Extensions;

public static class FileExtensions
{
    public static string GetSha256HashFromFile(this string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Nie odnaleziono pliku w podanej ścieżce", filePath);

        using var fileStream = File.OpenRead(filePath);
        using var ms = new MemoryStream();
        fileStream.CopyTo(ms);
        var result = ms.ToArray().ToSha256Hash();
        fileStream.Close();

        return result;
    }
}
