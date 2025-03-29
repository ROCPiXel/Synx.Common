namespace Synx.Common.FileSystem.Interfaces;

/// <summary>
/// IFileSysAct: Interface
/// 文件实例基本操作 委托定义
/// </summary>
public interface IFileSysAct
{
    /// <summary>
    /// 文件信息操作
    /// </summary>
    public static abstract bool GetExistsAction(string fullPath);
    
    /// <summary>
    /// 创建
    /// </summary>
    public static abstract void CreateAction(string fullPath);
    
    /// <summary>
    /// 删除
    /// </summary>
    public static abstract void DeleteAction(string fullPath);

    /// <summary>
    /// 重命名
    /// </summary>
    public static abstract void MoveAction(string sourceFPath, string targetFPath);

    /// <summary>
    /// 获取唯一的新路径
    /// </summary>
    public static abstract string GenerateUniquePathAction(string fullPath, string suffix);
}