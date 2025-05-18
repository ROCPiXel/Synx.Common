namespace Synx.Common.Collections.ProducerConsumer;

public interface IReusable
{
    int ReferenceCount { get; }
    
    int AddReference();
    
    int RemoveReference();
}