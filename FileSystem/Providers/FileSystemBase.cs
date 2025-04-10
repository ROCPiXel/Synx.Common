using System.Diagnostics.CodeAnalysis;
using Synx.Common.FileSystem.Interfaces;
using Synx.Common.Utils;

namespace Synx.Common.FileSystem.Providers;

/// <summary>
/// FileSystemBase: Abstract Class
/// 提供文件系统操作，抽象模板
/// </summary>
public abstract class FileSystemBase<TFileSystem> : IFileSystem
    where TFileSystem : class, IFileSystem
{
    private static volatile TFileSystem? _instance = null;
    private static readonly object _instanceLock = new object();
    public static TFileSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = (TFileSystem)Activator.CreateInstance(typeof(TFileSystem), nonPublic:true)!;
                    }
                }
            }
            return _instance;
        }
    }
    protected FileSystemBase(){}

    public abstract FileStream? Open(string fullPath,
                                     FileMode mode,
                                     FileAccess? access,
                                     FileShare? share, 
                                     int? bufferSize,
                                     FileOptions? options);

    public abstract bool Exists(string fullPath);
    
    public abstract FileStream Create(string fullPath);
    
    public abstract void Delete(string fullPath);
    
    public abstract void Move(string sourceFPath, string targetFPath);
    
    public abstract string GenerateUniquePath(string fullPath, string suffix);
}