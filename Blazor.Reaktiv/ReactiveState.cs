using Castle.DynamicProxy;
using Microsoft.AspNetCore.Components;

namespace Blazor.Reaktiv;

public interface IStateSubject
{
    void Attach(IStateObserver observer);
    void Detach(ComponentBase component);
    
    void Notify();
}

public class ReactiveState<T> : IInterceptor, IStateSubject, IDisposable where T : class
{
    private readonly ProxyGenerator _proxyGenerator = new();
    private readonly List<IStateObserver> _observers = new();
    public T Object { get; }
    private readonly IReactiveQueue _reactiveQueue;
    
    public ReactiveState(IReactiveQueue reactiveQueue)
    {
        _reactiveQueue = reactiveQueue;
        Object = _proxyGenerator.CreateClassProxy<T>(this);
    }

    public void Attach(IStateObserver observer) => _observers.Add(observer);

    public void Detach(ComponentBase component) => _observers.RemoveAll(x => x.Component == component);
    public void Notify()
    {
        _reactiveQueue.QueueNotification(_observers);
    }

    public void Intercept(IInvocation invocation)
    {
        invocation.Proceed();
        // If it's a setter, queue a notification
        if (!invocation.Method.Name.StartsWith("set_")) return;
        
        Notify();
    }

    public void Dispose() => _observers.Clear();
}
