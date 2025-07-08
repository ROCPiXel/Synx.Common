namespace Synx.Common.FileSystem.Providers.File;

public class DirectoryFileSystem : FileSystemBase<DirectoryFileSystem>
{
    private DirectoryFileSystem() { }
    
    public override FileStream? Open(string fullPath,
                                     FileMode mode,
                                     FileAccess? fileAccess,
                                     FileShare? fileShare,
                                     int? bufferSize, 
                                     FileOptions? options)
        => null;

    public override bool Exists(string fullPath) 
        => Directory.Exists(fullPath);
    
    public override void Create(string fullPath)
        => Directory.CreateDirectory(fullPath);
    
    public override void Delete(string fullPath) 
        => Directory.Delete(fullPath);
    
    public override void Move(string sourceFPath, string targetFPath) 
        => Directory.Move(sourceFPath, targetFPath);

    public override string GetExtension(string fullPath, bool includeDot = true)
        => string.Empty;
}