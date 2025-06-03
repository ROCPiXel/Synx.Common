namespace Synx.Common.Collections;

public interface IReusable
{
    int ReferenceCount { get; }
    
    int AddReference();
    
    int RemoveReference();
}