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


internal class StateProvider(IEnumerable<IStateSubject> reactiveStates) : IStateProvider, IObserverManager
{
    public T GetState<T>() where T : class
    {
        return GetInterceptor<T>().Object;
    }

    private ReactiveState<T> GetInterceptor<T>() where T : class
    {
        return reactiveStates.OfType<ReactiveState<T>>().Single();
    }

    public void Attach<T>(IStateObserver reactiveComponent) where T : class
    {
        var reactiveState = GetInterceptor<T>();
        reactiveState.Attach(reactiveComponent);
    }
    
    public void Detach(ComponentBase subscriber)
    {
        foreach (var reactiveState in reactiveStates)
        {
            reactiveState.Detach(subscriber);
        }
    }
}


