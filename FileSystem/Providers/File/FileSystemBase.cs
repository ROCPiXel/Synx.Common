using Synx.Common.FileSystem.Interfaces;

namespace Synx.Common.FileSystem.Providers.File;

/// <summary>
/// FileSystemBase: Abstract Class
/// 提供文件系统操作，抽象模板
/// </summary>
public abstract class FileSystemBase<TFileSystem> : IFileSystem
    where TFileSystem : class, IFileSystem
{
    private static volatile Lazy<TFileSystem> _instance = 
        new(() => (TFileSystem)Activator.CreateInstance(typeof(TFileSystem), nonPublic: true)!);
    public static TFileSystem Instance => _instance.Value;
    protected FileSystemBase(){}

    public abstract FileStream? Open(string fullPath,
                                     FileMode mode,
                                     FileAccess? access,
                                     FileShare? share, 
                                     int? bufferSize,
                                     FileOptions? options);

    public abstract bool Exists(string fullPath);
    
    public abstract void Create(string fullPath);
    
    public abstract void Delete(string fullPath);
    
    public abstract void Move(string sourceFPath, string targetFPath);
    
    public abstract string GetExtension(string fullPath, bool includeDot = true);
}