using Microsoft.AspNetCore.Components;

namespace Blazor.Reaktiv;

public interface IStateObserver
{
    public ComponentBase Component { get; }
    public EventCallback Callback { get; }
}

internal class StateObserver(ComponentBase component, EventCallback callback) : IStateObserver
{
    public ComponentBase Component { get; } = component;
    public EventCallback Callback { get; } = callback;
}
