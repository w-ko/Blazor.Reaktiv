using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Reaktiv;

public interface IReaktiv
{
    IReaktiv AddState<T>() where T : class;
}

internal class Reaktiv : IReaktiv
{
    private readonly IServiceCollection _services;
    public Reaktiv(IServiceCollection services) => _services = services;

    public IReaktiv AddState<T>() where T : class
    {
        _services.AddScoped<IStateSubject, ReactiveState<T>>();
        return this;
    }
}

public static class ServiceExtensions
{
    public static IReaktiv AddReaktiv(this IServiceCollection services)
    {
        services.AddScoped<StateProvider>();
        services.AddScoped<IStateProvider, StateProvider>(sp => sp.GetRequiredService<StateProvider>());
        services.AddScoped<IObserverManager, StateProvider>(sp => sp.GetRequiredService<StateProvider>());
        services.AddScoped<IReactiveQueue, ReactiveQueue>();
        return new Reaktiv(services);
    } 
}
