using System.Reflection;

namespace Blazor.Reaktiv;

internal static class ReflectionExtensions
{
    public static IEnumerable<PropertyInfo> GetStateProperties(this object obj)
    {
        return obj.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(p => Attribute.IsDefined(p, typeof(ReactiveAttribute)));
    }

    public static object GetStateProxy(this IStateProvider stateProvider, PropertyInfo property)
    {
        var getStateMethod = typeof(IStateProvider).GetMethod(nameof(IStateProvider.GetState));
        var getStateMethodInfo = getStateMethod?.MakeGenericMethod(property.PropertyType);
        var stateProxy = getStateMethodInfo?.Invoke(stateProvider, null);

        if (stateProxy == null)
        {
            throw new InvalidOperationException($"State proxy for {property.PropertyType.Name} not found.");
        }
        
        
        return stateProxy;
    }
    
    public static void AttachObserver(this IObserverManager observerManager, PropertyInfo property, StateObserver stateObserver)
    {
        var getAttachMethod = typeof(IObserverManager).GetMethod(nameof(IObserverManager.Attach));
        var getAttachMethodInfo = getAttachMethod?.MakeGenericMethod(property.PropertyType);
        getAttachMethodInfo?.Invoke(observerManager, new object[] { stateObserver });
    }
}
