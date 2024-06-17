using Microsoft.AspNetCore.Components;

namespace Blazor.Reaktiv;

public interface IStateProvider
{
    T GetState<T>() where T : class;
}

public interface IObserverManager
{
    void Detach(ComponentBase component);
    void Attach<T>(IStateObserver observer) where T : class;
}


internal class StateProvider : IStateProvider, IObserverManager
{
    private readonly IEnumerable<IStateSubject> _reactiveStates;
    public StateProvider(IEnumerable<IStateSubject> reactiveStates) => _reactiveStates = reactiveStates;

    public T GetState<T>() where T : class
    {
        return GetInterceptor<T>().Object;
    }

    private ReactiveState<T> GetInterceptor<T>() where T : class
    {
        return _reactiveStates.OfType<ReactiveState<T>>().Single();
    }

    public void Attach<T>(IStateObserver reactiveComponent) where T : class
    {
        var reactiveState = GetInterceptor<T>();
        reactiveState.Attach(reactiveComponent);
    }
    
    public void Detach(ComponentBase subscriber)
    {
        foreach (var reactiveState in _reactiveStates)
        {
            reactiveState.Detach(subscriber);
        }
    }
}


