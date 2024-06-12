# About
This library is a simple reactive state management solution for your reactive state management needs.
Instead of using INotifyPropertyChanged it uses Castle DynamicProxy to create a proxy object that intercepts all property changes and notifies all subscribers
of the change.

Make sure to try the Demo project to see the library in action.

# Quickstart
Before you can use the library, you need to add the necessary services to your application.
You can do this by adding the following code to your `Program.cs` file:

```csharp
builder.Services.AddReaktiv();
```

You can now create a state container e. g.:

```csharp
public class CounterState
{
    public virtual int Count { get; set; } // it is important to make the property virtual in order for the dynamic proxy to override it.
}
```

Then, register the state container in the service collection:

```csharp
builder.Services.AddReaktiv().AddState<CounterState>();
```

If you want to use the state container in a component, start by inheriting from `ReactiveComponentBase` and add the state container as a property:

```csharp
public class CounterComponent : ReactiveComponentBase
{
    [Reactive] private CounterState CounterState { get; set; } = default!;
}
```
The magic happens when you use the `Reactive` attribute.
This attribute will automatically subscribe the current component to updates of the state container and update the component when the state changes.

If you want to use the state container in a service, you can inject the state provider into a service:

```csharp
public class CounterService
{
    private readonly IStateProvider _stateProvider;

    public CounterService(IStateProvider stateProvider)
    {
        _stateProvider = stateProvider;
    }

    public void Increment()
    {
        var counterState = _stateProvider.GetState<CounterState>();
        counterState.Count++;
    }
}
```
