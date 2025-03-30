using Synx.Common.FileSystem.Interfaces;
using Synx.Common.Utils;

namespace Synx.Common.FileSystem.Providers;

/// <summary>
/// ActProvider: Abstract Class
/// 文件系统操作提供者，抽象模板
/// </summary>
public abstract class ActProvider : IFileSystem
{
    public static FileStream FileStream { get; set; }
    
    public abstract bool Exists(string fullPath);
    
    public abstract void Create(string fullPath);
    
    public abstract void Delete(string fullPath);
    
    public abstract void Move(string sourceFPath, string targetFPath);
    
    public abstract string GenerateUniquePath(string fullPath, string suffix);
}