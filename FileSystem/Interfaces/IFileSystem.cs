namespace Synx.Common.FileSystem.Interfaces;

/// <summary>
/// IFileSystem: Interface
/// 用于定义一个文件系统（数据源）所含的信息与操作
/// </summary>
public interface IFileSystem
{
    /// <summary>代表文件系统的流</summary>
    public static FileStream FileStream { get; }

    /// <summary>文件信息操作</summary>
    public bool Exists(string fullPath);
    
    /// <summary>创建</summary>
    public void Create(string fullPath);
    
    /// <summary>删除</summary>
    public void Delete(string fullPath);
    
    /// <summary>移动到（原生操作而并非重命名）</summary>
    public void Move(string sourceFPath, string targetFPath);
    
    /// <summary>生成唯一的新路径</summary>
    public string GenerateUniquePath(string fullPath, string suffix);
}