using Microsoft.AspNetCore.Components;

namespace Blazor.Reaktiv;

public abstract class ReactiveComponentBase : OwningComponentBase, IDisposable
{
    [Inject] protected IStateProvider StateProvider { get; set; } = default!;
    [Inject] protected IObserverManager ObserverManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        var properties = this.GetStateProperties();
        foreach (var state in properties)
        {
            var stateProxy = StateProvider.GetStateProxy(state);
            state.SetValue(this, stateProxy);

            var observer = new StateObserver(this, EventCallback.Factory.Create(this, InvokeStateHasChanged));
            ObserverManager.AttachObserver(state, observer);
        }
        
        base.OnInitialized();
    }
    
    // Create a template method for the InvokeStateHasChanged method such that it can be overridden in derived classes
    protected virtual ValueTask OnStateChanged() => ValueTask.CompletedTask;

    private async Task InvokeStateHasChanged()
    {
        await InvokeAsync(async () =>
        {
            StateHasChanged();
            await OnStateChanged();
        });
    }
    
    /// <summary>
    ///  Get the state of the specified type. This can be used in cases where the state is only read and does not need to react to changes.
    /// </summary>
    /// <typeparam name="T">The class name of the state requested.</typeparam>
    /// <returns>Yields the requested state container.</returns>
    protected T GetState<T>() where T : class => StateProvider.GetState<T>();

    public virtual void Dispose()
    {
        ObserverManager.Detach(this);
    }

    
}
