using System.Collections;
using Synx.Common.Enums;
using Synx.Common.FileSystem.Operations;

namespace Synx.Common.FileSystem.Structures;

/// <summary>
/// CPath - CompositePath: struct
/// 复合路径，含有绝对与相对路径
/// </summary>
///  TODO: IEnumerable，实现foreach
public struct CPath : IEnumerable<CPath>
{
    private string? _absolutePath;
    private string? _relativePath;
    private string? _base;
    // 以下属性仅含getter
    private Uri? _uri;
    private string? _parentPath;
    private string? _name;
    private string? _realName;
    private string? _extension;

    private void SetReadonlyAttributes()
    {
        string? primaryPath = _absolutePath ?? _relativePath;
        ArgumentException.ThrowIfNullOrEmpty(primaryPath, nameof(primaryPath));
        _parentPath = PathOperation.GetParentPath(primaryPath);
        _name = PathOperation.GetNameFromPath(primaryPath);
        _extension = PathOperation.GetExtension(_name);
        _realName = PathOperation.GetRealName(_name);
    }
    
    /// <summary>
    /// 绝对路径
    /// 获取时 若为空则依赖相对路径生成，获取时绝对路径或<see cref="RelativePath">相对路径</see>
    /// 至少有一者不能为空
    /// </summary>
    public string AbsolutePath
    {
        get
        {
            if (_absolutePath != null) return _absolutePath;
            ArgumentException.ThrowIfNullOrEmpty(_relativePath, nameof(_relativePath));
            return _absolutePath = PathOperation.GetAbsolutePath(_relativePath, _base);
        }
        set => _absolutePath = value;
    }
    
    /// <summary>
    /// 相对路径，相对于<see cref="Base">基路径</see>的路径
    /// 获取时 若为空则依赖绝对路径生成，获取时相对路径或<see cref="AbsolutePath">绝对路径</see>
    /// 至少有一者不能为空
    /// </summary>
    public string RelativePath
    {
        get
        {
            if (_relativePath != null) return _relativePath;
            ArgumentException.ThrowIfNullOrEmpty(_absolutePath, nameof(_absolutePath));
            _base ??= Path.GetPathRoot(_absolutePath) ?? AppDomain.CurrentDomain.BaseDirectory;
            return _relativePath = PathOperation.GetRelativePath(_absolutePath, _base);
        }
        set => _relativePath = value;
    }
    
    /// <summary>
    /// 基路径，相对路径相对于的路径
    /// 获取时 若为空则根据<see cref="RelativePath">相对路径</see>
    /// 与<see cref="AbsolutePath">绝对路径</see>反推，若两者有一为空则返回工作目录路径
    /// </summary>
    public string Base
    {
        get
        {
            if (_base != null) return _base;
            if(_relativePath == null || _absolutePath == null) 
                return _base = PathOperation.GetPathRoot(_absolutePath ?? _relativePath ?? null)
                    ?? AppDomain.CurrentDomain.BaseDirectory;
            return _base = PathOperation.GetAbsolutePath(_relativePath, _absolutePath);
        }
        set => _base = value;
    }

    /// <summary>
    /// <see cref="System.Uri">URI</see> 只读
    /// 获取时 若为空则根据<see cref="AbsolutePath">绝对路径</see>
    /// 尝试生成新的Uri
    /// </summary>
    /// <exception cref="ArgumentException">无法生成Uri</exception>
    public Uri Uri
    {
        get
        {
            if (_uri is not null) return _uri;
            try
            {
                _uri = new Uri(_absolutePath ?? AbsolutePath);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"无法获取URI，因为路径格式错误{ex.Message}", ex);
            }
            return _uri;
        }
    }

    /// <summary>
    /// 路径所代表文件或目录的名字 只读
    /// 获取时 尝试从_absolutePath或_relativePath中生成
    /// </summary>
    public string Name
    {
        get
        {
            if (_name != null) return _name;
            SetReadonlyAttributes();
            return _name!;
        }
    }

    /// <summary>
    /// 父目录 只读
    /// 获取时 尝试从_absolutePath或_relativePath中生成
    /// </summary>
    public string ParentPath
    {
        get
        {
            if (_parentPath != null) return _parentPath;
            SetReadonlyAttributes();
            return _parentPath!;
        }
    }
    
    /// <summary>
    /// 真名 除去扩展名的文件名 只读
    /// </summary>
    public string RealName
    {
        get
        {
            if (_realName != null) return _realName;
            SetReadonlyAttributes();
            return _realName!;
        }
    }
    
    /// <summary>
    /// 扩展名 默认带上点号
    /// </summary>
    public string Extension
    {
        get
        {
            if (_extension != null) return _extension;
            SetReadonlyAttributes();
            return _extension!;
        }
    }
    
    /// <summary>
    /// CPath构造函数，默认传入绝对路径
    /// </summary>
    /// <param name="absolutePath"></param>
    /// <param name="basePath"></param>
    public CPath(string absolutePath, string? basePath = null)
    {
        AbsolutePath = PathOperation.GetAbsolutePath(absolutePath.StandardizePath(), basePath);
        _base = basePath ?? null;
    }

    /// <summary>
    /// CPath构造函数，传入后拼接路径并更新绝对路径
    /// </summary>
    /// <param name="paths"></param>
    public CPath(params string[] paths):
        this(Path.Combine(paths).StandardizePath()) {}

    public CPath(){}

    /// <summary>
    /// （使用某个属性以及它的值）更新本实例的所有属性
    /// </summary>
    /// <param name="trigger"><see cref="Synx.Common.Enums.CPathSyncTrigger"/></param>
    /// <param name="content">trigger所对应的值，可空</param>
    /// <param name="basePath">手动指定<see cref="Base"/></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">枚举值不正确</exception>
    public CPath Sync(CPathSyncTrigger trigger = CPathSyncTrigger.AbsolutePath, 
        string? content = null, string? basePath = null)
    {
        _base = basePath 
            ?? _base 
            ?? PathOperation.GetPathRoot(_absolutePath ?? _relativePath)
            ?? AppDomain.CurrentDomain.BaseDirectory;
        switch (trigger)
        {
            case CPathSyncTrigger.AbsolutePath:
                _absolutePath = content ?? _absolutePath;
                ArgumentException.ThrowIfNullOrEmpty(_absolutePath, nameof(_absolutePath));
                _relativePath = Path.GetRelativePath(_base, _absolutePath);
                break;
            case CPathSyncTrigger.RelativePath:
                _relativePath = content ?? _relativePath;
                ArgumentException.ThrowIfNullOrEmpty(_relativePath, nameof(_relativePath));
                _absolutePath = Path.GetFullPath(_relativePath, _base);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(trigger), trigger, null);
        }
        
        _uri = new Uri(_absolutePath);
        _base = Base; // Base.getter 反推base或者返回工作目录
        SetReadonlyAttributes();
        return this;
    }

    /// <summary>获取绝对路径</summary>
    public override string ToString() => _absolutePath ?? string.Empty;
    
    public IEnumerator<CPath> GetEnumerator()
    {
        throw new NotImplementedException();
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}