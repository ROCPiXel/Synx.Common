using Synx.Common.FileSystem.Interfaces;
using Synx.Common.FileSystem.Structures;

namespace Synx.Common.FileSystem.Operations;

public static class CPathExtension
{
    /// <summary>
    /// 将字符串作为绝对路径转换为CPath对象
    /// </summary>
    /// <param name="path">绝对路径</param>
    /// <returns></returns>
    public static CPath ToCPath(this string path) => new(path);

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
    /// 返回传入路径的绝对路径的Uri
    /// </summary>
    /// <param name="cPath"></param>
    /// <returns>绝对路径的Uri</returns>
    public static Uri GetUri(this CPath cPath) => new(cPath.AbsolutePath);
    
    public static string GetNameFromPath(this CPath cPath) => PathOperation.GetNameFromPath(cPath.AbsolutePath);
    
    public static string GetParentPath(this CPath cPath) => PathOperation.GetParentPath(cPath.AbsolutePath);

    public static T CreateFileObjectInstance<T>(this CPath cPath) 
        where T : IFileObject<T>, new()
    {
        ArgumentException.ThrowIfNullOrEmpty(cPath.AbsolutePath, nameof(cPath));
        return new T
        {
            Path = cPath
        };
    }
}