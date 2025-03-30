using Synx.Common.FileSystem.Interfaces;
using Synx.Common.Utils;

namespace Synx.Common.FileSystem.Providers;

public class FileSystemProvider : ActProvider
{
    public override bool Exists(string fullPath) => File.Exists(fullPath);
    
    public override void Create(string fullPath) => File.Create(fullPath);
    
    public override void Delete(string fullPath) => File.Delete(fullPath);
    
    public override void Move(string sourceFPath, string targetFPath) =>  File.Move(sourceFPath, targetFPath);
    
    public override string GenerateUniquePath(string fullPath, string suffix) => PathStringProc.GenerateFilePath(fullPath, suffix);
}