using Microsoft.AspNetCore.Components;

namespace Blazor.Reaktiv;

public interface IStateObserver
{
    public ComponentBase Component { get; }
    public EventCallback Callback { get; }
}

internal class StateObserver : IStateObserver
{
    public StateObserver(ComponentBase component, EventCallback callback)
    {
        Component = component;
        Callback = callback;
    }
    
    public ComponentBase Component { get; }
    public EventCallback Callback { get; }
}
