using Synx.Common.FileSystem.Helpers;

namespace Synx.Common.FileSystem.Providers.File;

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
                       bufferSize ?? Definition.DefaultIoBufferSize,
                       options ?? FileOptions.None);

    public override bool Exists(string fullPath) => System.IO.File.Exists(fullPath);
    
    public override void Create(string fullPath) 
        => System.IO.File.Create(fullPath).Close();
    
    public override void Delete(string fullPath) 
        => System.IO.File.Delete(fullPath);
    
    public override void Move(string sourceFPath, string targetFPath) 
        =>  System.IO.File.Move(sourceFPath, targetFPath);
    public override string GetExtension(string fullPath, bool includeDot = true)
        => PathHelper.GetExtension(fullPath, includeDot);
}