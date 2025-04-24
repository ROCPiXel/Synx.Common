using Synx.Common.FileSystem.Operations;

namespace Synx.Common.FileSystem.Providers.FIle;

public class SingleFileSystem : FileSystemBase<SingleFileSystem>
{
    private SingleFileSystem() { }
    
    public override FileStream? Open(string fullPath,
                                     FileMode mode,
                                     FileAccess? access,
                                     FileShare? share,
                                     int? bufferSize, 
                                     FileOptions? options) =>
        new FileStream(fullPath,
                       mode,
                       access ?? FileAccess.ReadWrite,
                       share ?? FileShare.None,
                       bufferSize ?? 4096, // TODO: Make this configurable
                       options ?? FileOptions.None);

    public override bool Exists(string fullPath) => File.Exists(fullPath);
    
    public override FileStream Create(string fullPath) 
        => File.Create(fullPath);
    
    public override void Delete(string fullPath) 
        => File.Delete(fullPath);
    
    public override void Move(string sourceFPath, string targetFPath) 
        =>  File.Move(sourceFPath, targetFPath);
    public override string GetExtension(string fullPath, bool includeDot = true)
        => PathOperation.GetExtension(fullPath, includeDot);
}