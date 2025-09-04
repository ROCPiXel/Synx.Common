namespace Synx.Common.FileSystem.Interfaces;

/// <summary>
/// IFileSystem: TaskList
/// 用于定义一个文件系统（数据源）所含的信息与操作
/// </summary>
public interface IFileSystem
{
    /// <summary>代表文件系统的流</summary>
    FileStream? Open(string fullPath,
                            FileMode mode,
                            FileAccess? access = FileAccess.ReadWrite,
                            FileShare? share = FileShare.None, 
                            int? bufferSize = 4096,
                            FileOptions? options = FileOptions.None);

    /// <summary>判断存在</summary>
    bool Exists(string fullPath);
    
    /// <summary>创建</summary>
    void Create(string fullPath);
    
    /// <summary>删除</summary>
    void Delete(string fullPath);
    
    /// <summary>移动到（原生操作而并非重命名）</summary>
    void Move(string sourceFPath, string targetFPath);

    /// <summary>获取路径的后缀名，文件与目录路径上的唯一（？）区别</summary>
    string GetExtension(string fullPath, bool includeDot = true);
}