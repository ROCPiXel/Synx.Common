namespace Synx.Common.FileSystem.Interfaces;

// TODO：即将被IFileSystem替代
/// <summary>
/// IFileObjectAct: Interface
/// 文件实例基本操作
/// </summary>
public interface IFileObjectAct
{
    /// <summary>判断存在</summary>
    public static abstract bool Exists(string fullPath);
    
    /// <summary>创建</summary>
    public static abstract void Create(string fullPath);
    
    /// <summary>删除</summary>
    public static abstract void Delete(string fullPath);

    /// <summary>移动到（原生操作而并非重命名）</summary>
    public static abstract void Move(string sourceFPath, string targetFPath);

    /// <summary>生成唯一的新路径</summary>
    public static abstract string GenerateUniquePath(string fullPath, string suffix);
}