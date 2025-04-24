using Synx.Common.FileSystem.Interfaces;
using Synx.Common.FileSystem.Structures;

namespace Synx.Common.FileSystem.Operations;

/// <summary>
/// CPath的扩展方法
/// <seealso cref="CPath">对象：CPath</seealso>
/// <seealso cref="PathOperation">方法：PathOperation</seealso>
/// </summary>
public static class CPathExtension
{
    /// <summary>
    /// 将字符串作为绝对路径转换为CPath对象
    /// </summary>
    /// <param name="path">绝对路径</param>
    /// <returns></returns>
    public static CPath ToCPath(this string path) => new(path);

    /// <summary>
    /// 标准化绝对路径
    /// <see cref="PathOperation.StandardizePath(string)"/>"/>
    /// </summary>
    /// <param name="cPath"></param>
    /// <returns></returns>
    public static string StandardizePath(this CPath cPath) => cPath.AbsolutePath.StandardizePath();

    public static string GetAbsolutePath(this CPath cPath, string? basePath = null)
    {
        return PathOperation.GetAbsolutePath(cPath.RelativePath, basePath);
    }

    public static CPath GetAbsolutePath(this ref CPath cPath, string? basePath = null)
    {
        cPath.AbsolutePath = PathOperation.GetAbsolutePath(cPath.RelativePath, basePath);
        return cPath;
    }
    
    public static string GetRelativePath(CPath cPath, string? basePath = null) =>
        PathOperation.GetRelativePath(cPath.RelativePath, basePath); 
    
    /// <summary>
    /// 生成 对应的uri（不更新）
    /// </summary>
    /// <param name="cPath"></param>
    /// <returns>绝对路径的Uri</returns>
    public static Uri GetUri(this CPath cPath) => new(cPath.AbsolutePath);
    
    public static string GetNameFromPath(this CPath cPath) => PathOperation.GetNameFromPath(cPath.AbsolutePath);
    
    public static string GetParentPath(this CPath cPath) => PathOperation.GetParentPath(cPath.AbsolutePath);

    public static T CreateFileObject<T>(this CPath cPath) 
        where T : IFileObject<T>, new()
    {
        ArgumentException.ThrowIfNullOrEmpty(cPath.AbsolutePath, nameof(cPath));
        return new T
        {
            Path = cPath
        };
    }

    public static T CreateFileObjectInstance<T>(this CPath cPath)
        where T : class, IFileObject<T>, new()
    {
        ArgumentException.ThrowIfNullOrEmpty(cPath.AbsolutePath, nameof(cPath));
        FileObjectOperation<T>.Create(cPath.AbsolutePath);
        return new T
        {
            Path = cPath
        };
    }
}