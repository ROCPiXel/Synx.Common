namespace Synx.Common.Utils;

public class ProgressHelper
{
    private int _count = 1;

    public int Interval
    {
        get => _interval;
        set
        {
            _interval = value;
            _intervalsPerSecond = 1000 / (float)Interval;
        }
    }

    private int _interval;
    private float _intervalsPerSecond;

    private long _lastReaderPointers = 0;
    private long _lastWriterPointers = 0;
    public long FileSize { get; set; }

    public ProgressHelper(){}
    
    public ProgressHelper(int interval, long fileSize)
    {
        Interval = interval;
        FileSize = fileSize;
        _intervalsPerSecond = 1000 / (float)Interval;
    }
    
    public void PrintProgress(string sign, long readerPointer, long writerPointer)
    {
        float readerSpeed = (readerPointer - _lastReaderPointers) / (float)(1024 * 1024) * _intervalsPerSecond;
        float writerSpeed = (writerPointer - _lastWriterPointers) / (float)(1024 * 1024) * _intervalsPerSecond;

        Console.WriteLine($"#{_count} at {_count * Interval}ms: " +
                          $"RP: {readerPointer} Progress: {(float)readerPointer / FileSize} {readerSpeed}MiB/s | " +
                          $"WP: {writerPointer} Progress: {(float)writerPointer / FileSize} {writerSpeed}MiB/s");

        _lastReaderPointers = readerPointer;
        _lastWriterPointers = writerPointer;
        _count++;
    }

    public void Clear()
    {
        _count = 1;
        _lastReaderPointers = 0;
        _lastWriterPointers = 0;
    }
}