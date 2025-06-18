using Synx.Common.Enums;
using Synx.Common.FileSystem.Helpers;

namespace Synx.Common.FileSystem.Structures;

/// <summary>
/// CPath - CompositePath
/// 复合路径，含有绝对与相对路径
/// </summary>
public record struct CPath
{
    private string? _absolute;
    private string? _relative;
    private string? _base;
    // 以下属性仅含getter
    private Uri? _uri;
    private string? _parent;
    private string? _name;
    private string? _realName;
    private string? _extension;

    private void SetReadonlyAttributes()
    {
        string? primaryPath = _absolute ?? _relative;
        ArgumentException.ThrowIfNullOrEmpty(primaryPath, nameof(primaryPath));
        _parent = PathOperation.GetParentPath(primaryPath);
        _name = PathOperation.GetNameFromPath(primaryPath);
        _extension = PathOperation.GetExtension(_name);
        _realName = PathOperation.GetRealName(_name);
    }
    
    /// <summary>
    /// 绝对路径
    /// 获取时 若为空则依赖相对路径生成，获取时绝对路径或<see cref="Relative">相对路径</see>
    /// 至少有一者不能为空
    /// </summary>
    public string Absolute
    {
        get
        {
            if (_absolute != null) return _absolute;
            ArgumentException.ThrowIfNullOrEmpty(_relative, nameof(_relative));
            return _absolute = PathOperation.GetAbsolutePath(_relative, _base);
        }
        set => _absolute = value;
    }
    
    /// <summary>
    /// 相对路径，相对于<see cref="Base">基路径</see>的路径
    /// 获取时 若为空则依赖绝对路径生成，获取时相对路径或<see cref="Absolute">绝对路径</see>
    /// 至少有一者不能为空
    /// </summary>
    public string Relative
    {
        get
        {
            if (_relative != null) return _relative;
            ArgumentException.ThrowIfNullOrEmpty(_absolute, nameof(_absolute));
            _base ??= Path.GetPathRoot(_absolute) ?? AppDomain.CurrentDomain.BaseDirectory;
            return _relative = PathOperation.GetRelativePath(_absolute, _base);
        }
        set => _relative = value;
    }
    
    /// <summary>
    /// 基路径，相对路径相对于的路径
    /// 获取时 若为空则根据<see cref="Relative">相对路径</see>
    /// 与<see cref="Absolute">绝对路径</see>反推，若两者有一为空则返回工作目录路径
    /// </summary>
    public string Base
    {
        get
        {
            if (_base != null) return _base;
            if(_relative == null || _absolute == null) 
                return _base = PathOperation.GetPathRoot(_absolute ?? _relative ?? null)
                    ?? AppDomain.CurrentDomain.BaseDirectory;
            return _base = PathOperation.GetAbsolutePath(_relative, _absolute);
        }
        set => _base = value;
    }

    /// <summary>
    /// <see cref="System.Uri">URI</see> 只读
    /// 获取时 若为空则根据<see cref="Absolute">绝对路径</see>
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
                _uri = new Uri(_absolute ?? Absolute);
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
    public string Parent
    {
        get
        {
            if (_parent != null) return _parent;
            SetReadonlyAttributes();
            return _parent!;
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
        Absolute = PathOperation.GetAbsolutePath(absolutePath.StandardizePath(), basePath);
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
            ?? PathOperation.GetPathRoot(_absolute ?? _relative)
            ?? AppDomain.CurrentDomain.BaseDirectory;
        switch (trigger)
        {
            case CPathSyncTrigger.AbsolutePath:
                _absolute = content ?? _absolute;
                ArgumentException.ThrowIfNullOrEmpty(_absolute, nameof(_absolute));
                _relative = Path.GetRelativePath(_base, _absolute);
                break;
            case CPathSyncTrigger.RelativePath:
                _relative = content ?? _relative;
                ArgumentException.ThrowIfNullOrEmpty(_relative, nameof(_relative));
                _absolute = Path.GetFullPath(_relative, _base);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(trigger), trigger, null);
        }
        
        _uri = new Uri(_absolute);
        _base = Base; // Base.getter 反推base或者返回工作目录
        SetReadonlyAttributes();
        return this;
    }

    /// <summary>获取绝对路径</summary>
    public override string ToString() => _absolute ?? string.Empty;
}