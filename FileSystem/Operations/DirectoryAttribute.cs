using Synx.Common.FileSystem.Structures;

namespace Synx.Common.FileSystem.Operations;

public static class DirectoryAttribute
{
    /// <summary>
    /// GetDirectoryInfo - func
    /// 获取目录信息，如果目录真实存在
    /// </summary>
    /// <param name="singleDirectory"></param>
    /// <returns></returns>
    /// <exception cref="DirectoryNotFoundException"></exception>
    public static SingleDirectory GetDirectoryInfo(SingleDirectory singleDirectory)
    {
        DirectoryInfo directoryInfo = new(singleDirectory.Path.AbsolutePath);
        singleDirectory.DirectoryInfo = directoryInfo;
        singleDirectory.IsExists = directoryInfo.Exists;

        if (!singleDirectory.IsExists)
        {
            throw new DirectoryNotFoundException("Directory does not exist");
        }

        singleDirectory.CreateTime = directoryInfo.CreationTime; // 创建时间
        singleDirectory.ModifyTime = directoryInfo.LastWriteTime; // 修改时间
        singleDirectory.AccessTime = directoryInfo.LastAccessTime; // 访问时间

        return singleDirectory;
    }

    /// <summary>
    /// GetDepth: func
    /// 获取目录深度，当前层级为0，逐级各加一
    /// </summary>
    /// <param name="directoryInfo"></param>
    /// <param name="maxDepth"></param>
    /// <returns></returns>
    public static int GetDepth(DirectoryInfo directoryInfo, int maxDepth = Definition.DirectoryScanningMaxDepth)
    {
        int depth = 0;
        try
        {
            foreach (var di in directoryInfo.GetDirectories())
            {
                int step = GetDepth(di);
                if (step > depth)
                {
                    if (step >= maxDepth)
                    {
                        return step;
                    }

                    depth = step;
                }
            }
        }
        catch (Exception)
        {
            return 0;
        }

        return depth + 1;
    }

    /// <summary>
    /// GetContent: func
    /// 获取文件夹下一层级中的所有内容
    /// 承担了所有内容的填充
    /// </summary>
    /// <param name="sd">SingleDirectory</param>
    /// <returns></returns>
    public static SingleDirectory GetContent(SingleDirectory sd)
    {
        if (sd.DirectoryInfo is null) GetDirectoryInfo(sd);
        try
        {
            if (sd.DirectoryInfo == null)
            {
                GetDirectoryInfo(sd); // 当递归传入空目录，引发异常返回
            }
            DirectoryInfo[] directoriesInfo = sd.DirectoryInfo!.GetDirectories();
            //sd.ChildDirectoriesInfo = directoriesInfo;
            foreach (DirectoryInfo di in directoriesInfo)
            {
                sd.SubDirectory.Add(new SingleDirectory(di));
            }
        }
        catch (Exception)
        {
            return new SingleDirectory();
        }
        return sd;
    }

    /// <summary>
    /// Traverse: func
    /// 遍历整个文件夹（所有层级）
    /// </summary>
    /// <param name="sd">SingleDirectory</param>
    /// <returns></returns>
    public static SingleDirectory Traverse(SingleDirectory sd)
    {
        if (sd.DirectoryInfo is null) GetDirectoryInfo(sd); // 预处理，并添加目录信息
        try
        {
            // 遍历每个子目录
            foreach (DirectoryInfo di in sd.DirectoryInfo!.GetDirectories())
            {
                SingleDirectory dir = new(di);
                sd.SubDirectory.Add(Traverse(dir)); // 递归
            }
        }
        catch (Exception)
        {
            return new SingleDirectory(); // 空目录
        }
        return sd; // 返回当前目录
    }
}