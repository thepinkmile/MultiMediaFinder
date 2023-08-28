namespace MultiMediaFinder.Models;

public class FileDetails
{
    public string ParentPath { get; set; } = null!;

    public string FileNamae { get; set; } = null!;

    public string MD5_Hash { get; set; } = null!;

    public string SHA256_Hash { get; set; } = null!;

    public string SHA512_Hash { get; set; } = null!;

    public long FileSize { get; set; }

    public bool ShouldExport { get; set; } = true;

    public MultiMediaType FileType { get; set; }
}
