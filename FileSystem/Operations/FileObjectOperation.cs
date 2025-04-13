using System.Diagnostics;
using Synx.Common.Enums;
using Synx.Common.FileSystem.Interfaces;
using Synx.Common.FileSystem.Structures;

namespace Synx.Common.FileSystem.Operations;

/// <summary>
/// FileObjectOperation: staticClass
/// 单个文件对象的操作：创建，删除，重命名
/// 合并: DirectoryOperation, FileOperation
/// </summary>
/// <typeparam name="TFileSysObj">文件系统中的对象，此处要求SingleDirectory/File</typeparam>
public static class FileObjectOperation<TFileSysObj> 
    where TFileSysObj: class, IFileObject<TFileSysObj>, new()
{
    /// <summary>
    /// Create: tFunc
    /// 创建文件对象，重载时文件夹名加上前缀"\\"
    /// </summary>
    /// <param name="fullPath">完整路径</param>
    /// <param name="creationMethod">创建方式<see cref="CreationMethod"/></param>
    /// <param name="suffix">后缀</param>
    /// <returns></returns>
    public static TFileSysObj? Create(string fullPath,
        CreationMethod creationMethod = CreationMethod.Keep, string suffix = Definition.DefaultSuffix)
    {
        ArgumentNullException.ThrowIfNull(fullPath);
        // 目标路径与唯一的新路径
        string newFullPath = fullPath;
        string uniqueFullPath = PathOperation.GenerateUniquePath<TFileSysObj>(newFullPath, suffix);
        
        // 创建实例并使用FillInfo模拟构造
        var newFileSysObj = new TFileSysObj();
        newFileSysObj.FillInfo(new CPath(newFullPath));

        // 基础情况：存在且保持，两者冲突
        if (TFileSysObj.FileSystem.Exists(newFullPath) && creationMethod == CreationMethod.Keep) return null;

        try
        {
            if (!TFileSysObj.FileSystem.Exists(newFullPath))
            {
                TFileSysObj.FileSystem.Create(newFullPath).Close();
                return newFileSysObj;
            }

            switch (creationMethod)
            {
                case CreationMethod.New:
                    TFileSysObj.FileSystem.Create(uniqueFullPath).Close();
                    newFileSysObj.FillInfo(new CPath(uniqueFullPath));
                    break;

                // 注意：此选项将覆盖原有文件，谨慎操作
                case CreationMethod.Cover:
                    TFileSysObj.FileSystem.Delete(newFullPath);
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ERR] Failed to create FileSysObj: {ex}");
            throw new Exception($"Failed to create FileObject: {ex}");
        }
        return newFileSysObj;
    }

    public static TFileSysObj? Create(CPath fullCPath,
        CreationMethod creationMethod = CreationMethod.Keep, string suffix = Definition.DefaultSuffix)
        => Create(fullCPath.AbsolutePath, creationMethod, suffix);
    
    public static TFileSysObj? Create(string name, string parentPath,
        CreationMethod creationMethod = CreationMethod.Keep, string suffix = Definition.DefaultSuffix)
        => Create(PathOperation.Combine([parentPath, name]), creationMethod, suffix);
    
    public static TFileSysObj? Create(string name, CPath parentCPath,
        CreationMethod creationMethod = CreationMethod.Keep, string suffix = Definition.DefaultSuffix) 
        => Create(PathOperation.Combine([parentCPath.AbsolutePath, name]), creationMethod, suffix);

    public static TFileSysObj? Create(TFileSysObj fileSysObj,
        CreationMethod creationMethod = CreationMethod.Keep, string suffix = Definition.DefaultSuffix)
    {
        fileSysObj = Create(fileSysObj.Path.AbsolutePath, creationMethod, suffix) ?? fileSysObj;
        return fileSysObj;
    }

    /// <summary>
    /// Delete: tFunc
    /// 删除文件对象，重载时文件夹名注意添加"\\"
    /// </summary>
    /// <param name="fullPath">完整路径</param>
    /// <returns></returns>
    public static bool Delete(string fullPath)
    {
        if (!TFileSysObj.FileSystem.Exists(fullPath)) return false;
        
        try
        {
            TFileSysObj.FileSystem.Delete(fullPath);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ERR] Failed to delete FileSysObj: {ex}");
            // throw new Exception($"Failed to delete FileSysObject: {ex}");
        }

        return false;
    }
    
    public static bool Delete(CPath fullPath) => Delete(fullPath.AbsolutePath);
    
    public static bool Delete(string name, string parentPath) => Delete(name + parentPath);
    
    public static bool Delete(TFileSysObj fileSysObj) => Delete(fileSysObj.Path.AbsolutePath);

    /// <summary>
    /// Rename: tFunc
    /// 重命名文件实例
    /// </summary>
    /// <param name="sourceFPath">源路径</param>
    /// <param name="targetFPath">完整的目标路径</param>
    /// <param name="creationMethod">创建方式<see cref="CreationMethod"/></param>
    /// <param name="suffix"></param>
    /// <returns>重命名之后的新实例，null为失败</returns>
    public static TFileSysObj? Rename(string sourceFPath, string targetFPath,
        CreationMethod creationMethod = CreationMethod.Keep, string suffix = Definition.DefaultSuffix)
    {
        var creation = Create(sourceFPath, creationMethod, suffix);
        if(creation == null) return null;
        var uniquePath = creation.Path.AbsolutePath;

        try
        {
            TFileSysObj.FileSystem.Move(sourceFPath, uniquePath);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ERR] Failed to rename FileSysObj: {ex}");
            throw new Exception($"Failed to rename FileObject: {ex}");
        }

        return creation;
    }
    
    public static TFileSysObj? Rename(CPath sourceCPath, CPath targetCPath,
        CreationMethod creationMethod = CreationMethod.Keep, string suffix = Definition.DefaultSuffix)
        => Rename(sourceCPath.AbsolutePath, targetCPath.AbsolutePath, creationMethod, suffix);
    
    public static TFileSysObj? Rename(string sourceName, string targetName, string parentPath,
        CreationMethod creationMethod = CreationMethod.Keep, string suffix = Definition.DefaultSuffix)
        => Rename(parentPath + sourceName, parentPath + targetName, creationMethod, suffix);

    /// <summary>注意：此Rename重载不返回Null，若重命名失败会返回传入对象</summary>
    public static TFileSysObj Rename(TFileSysObj source, TFileSysObj target,
        CreationMethod creationMethod = CreationMethod.Keep, string suffix = Definition.DefaultSuffix)
    {
        target = Rename(source.Path.AbsolutePath, target.Path.AbsolutePath, creationMethod, suffix) ?? target;
        return target;
    }
    
    /// <summary>注意：此Rename重载不返回Null，若重命名失败会返回传入对象</summary>
    public static TFileSysObj Rename(TFileSysObj source, string targetFullPath, 
        CreationMethod creationMethod = CreationMethod.Keep, string suffix = Definition.DefaultSuffix)
    {
        source = Rename(source.Path.AbsolutePath, targetFullPath, creationMethod, suffix) ?? source;
        return source;
    }
}