namespace Synx.Common.FileSystem.Providers.FIle;

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

    //TODO: 这里的创建目录会导致文件流无法打开，考虑更换泛型实现
    public override FileStream? Create(string fullPath)
    {
        Directory.CreateDirectory(fullPath);
        return null;
    }
    
    public override void Delete(string fullPath) 
        => Directory.Delete(fullPath);
    
    public override void Move(string sourceFPath, string targetFPath) 
        => Directory.Move(sourceFPath, targetFPath);

    public override string GetExtension(string fullPath, bool includeDot = true)
        => string.Empty;
}