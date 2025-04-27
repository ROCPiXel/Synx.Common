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
    /// <param name="fileConflictResolution">创建方式<see cref="FileConflictResolution"/></param>
    /// <param name="suffix">后缀</param>
    /// <returns></returns>
    public static TFileSysObj? Create(string fullPath,
        FileConflictResolution fileConflictResolution = FileConflictResolution.Keep, string suffix = Definition.DefaultSuffix)
    {
        ArgumentNullException.ThrowIfNull(fullPath);
        // 目标路径与唯一的新路径
        string finalPath = fullPath; 
        string uniqueFullPath = PathOperation.GenerateUniquePath<TFileSysObj>(fullPath, suffix);
        TFileSysObj newFileSysObj;

        // 基础情况：存在且保持，两者冲突
        if (TFileSysObj.FileSystem.Exists(fullPath) && fileConflictResolution == FileConflictResolution.Keep) return null;

        // 文件不存在：创建，忽略选项
        // if (!TFileSysObj.FileSystem.Exists(fullPath)) finalPath = fullPath;
        // 文件存在
        switch (fileConflictResolution)
        {
            // 选择新建且存在：生成唯一路径
            case FileConflictResolution.New:
                finalPath = PathOperation.GenerateUniquePath<TFileSysObj>(fullPath, suffix);
                break;

            // 选择覆盖且存在：直接覆盖文件
            // 注意：此选项将覆盖原有文件，谨慎操作
            case FileConflictResolution.Overwrite:
                // finalPath = fullPath;
                TFileSysObj.FileSystem.Delete(finalPath);
                break;
        }

        try
        {
            TFileSysObj.FileSystem.Create(finalPath).Close();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ERR] Failed to create FileSysObj: {ex}");
            throw new Exception($"Failed to create FileObject: {ex}");
        }
        finally
        {
            // 创建实例并更新路径相关信息
            newFileSysObj = new TFileSysObj()
            {
                Path = new CPath(fullPath)
            };
            newFileSysObj.Path.Sync();
        }
        return newFileSysObj;
    }

    public static TFileSysObj? Create(CPath fullCPath,
        FileConflictResolution fileConflictResolution = FileConflictResolution.Keep, string suffix = Definition.DefaultSuffix)
        => Create(fullCPath.Absolute, fileConflictResolution, suffix);
    
    public static TFileSysObj? Create(string name, string parentPath,
        FileConflictResolution fileConflictResolution = FileConflictResolution.Keep, string suffix = Definition.DefaultSuffix)
        => Create(Path.Combine([parentPath, name]), fileConflictResolution, suffix);
    
    public static TFileSysObj? Create(string name, CPath parentCPath,
        FileConflictResolution fileConflictResolution = FileConflictResolution.Keep, string suffix = Definition.DefaultSuffix) 
        => Create(Path.Combine([parentCPath.Absolute, name]), fileConflictResolution, suffix);

    public static TFileSysObj? Create(TFileSysObj fileSysObj,
        FileConflictResolution fileConflictResolution = FileConflictResolution.Keep, string suffix = Definition.DefaultSuffix)
    {
        fileSysObj = Create(fileSysObj.Path.Absolute, fileConflictResolution, suffix) ?? fileSysObj;
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
    
    public static bool Delete(CPath fullPath) => Delete(fullPath.Absolute);
    
    public static bool Delete(string name, string parentPath) => Delete(name + parentPath);
    
    public static bool Delete(TFileSysObj fileSysObj) => Delete(fileSysObj.Path.Absolute);

    /// <summary>
    /// Rename: tFunc
    /// 重命名文件实例
    /// </summary>
    /// <param name="sourceFPath">源路径</param>
    /// <param name="targetFPath">完整的目标路径</param>
    /// <param name="fileConflictResolution">创建方式<see cref="FileConflictResolution"/></param>
    /// <param name="suffix"></param>
    /// <returns>重命名之后的新实例，null为失败</returns>
    public static TFileSysObj? Rename(string sourceFPath, string targetFPath,
        FileConflictResolution fileConflictResolution = FileConflictResolution.Keep, string suffix = Definition.DefaultSuffix)
    {
        var creation = Create(sourceFPath, fileConflictResolution, suffix);
        if(creation == null) return null;
        var uniquePath = creation.Path.Absolute;

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
        FileConflictResolution fileConflictResolution = FileConflictResolution.Keep, string suffix = Definition.DefaultSuffix)
        => Rename(sourceCPath.Absolute, targetCPath.Absolute, fileConflictResolution, suffix);
    
    public static TFileSysObj? Rename(string sourceName, string targetName, string parentPath,
        FileConflictResolution fileConflictResolution = FileConflictResolution.Keep, string suffix = Definition.DefaultSuffix)
        => Rename(parentPath + sourceName, parentPath + targetName, fileConflictResolution, suffix);

    /// <summary>注意：此Rename重载不返回Null，若重命名失败会返回传入对象</summary>
    public static TFileSysObj Rename(TFileSysObj source, TFileSysObj target,
        FileConflictResolution fileConflictResolution = FileConflictResolution.Keep, string suffix = Definition.DefaultSuffix)
    {
        target = Rename(source.Path.Absolute, target.Path.Absolute, fileConflictResolution, suffix) ?? target;
        return target;
    }
    
    /// <summary>注意：此Rename重载不返回Null，若重命名失败会返回传入对象</summary>
    public static TFileSysObj Rename(TFileSysObj source, string targetFullPath, 
        FileConflictResolution fileConflictResolution = FileConflictResolution.Keep, string suffix = Definition.DefaultSuffix)
    {
        source = Rename(source.Path.Absolute, targetFullPath, fileConflictResolution, suffix) ?? source;
        return source;
    }
}