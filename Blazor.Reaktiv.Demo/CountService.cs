namespace Blazor.Reaktiv.Demo;

public class CountService
{
    private readonly IStateProvider _stateProvider;

    public CountService(IStateProvider stateProvider)
    {
        _stateProvider = stateProvider;
    }
    
    public void Increment()
    {
        var count = _stateProvider.GetState<CountState>();
        count.CurrentCount++;
    }
}
