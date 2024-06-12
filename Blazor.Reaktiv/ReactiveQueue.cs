using System.Threading.Channels;
using Microsoft.AspNetCore.Components;

namespace Blazor.Reaktiv;

public interface IReactiveQueue
{
    void QueueNotification(List<IStateObserver> observers);
}

internal class ReactiveQueue: IReactiveQueue, IDisposable
{
    private readonly Channel<EventCallback> _channel = Channel.CreateUnbounded<EventCallback>(options: new UnboundedChannelOptions {SingleWriter = false, SingleReader = false});

    public ReactiveQueue() => _ = ProcessQueue();

    public void QueueNotification(List<IStateObserver> observers)
    {
        foreach (var observer in observers)
        {
            _channel.Writer.TryWrite(observer.Callback);
        }
    }
    
    private async Task ProcessQueue()
    {
        await foreach (var notification in _channel.Reader.ReadAllAsync())
        {
            if (!notification.HasDelegate) continue;
            try
            {
                await notification.InvokeAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        _channel.Writer.Complete();
    }

    public void Dispose() => _channel.Writer.Complete();
}


