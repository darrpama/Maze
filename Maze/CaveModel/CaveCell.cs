namespace CaveModel;

public record CaveCell
{
    public CaveCell()
    {
        
    }

    public CaveCell(bool isAlive)
    {
        IsAlive = isAlive;
    }
    public event EventHandler<bool>? AliveHasSet;

    private bool _isAlive = false;
    public bool IsAlive
    {
        get => _isAlive
        ;
        private set
        {
            _isAlive = value;
            OnAliveHasSet(value);
        }
    } 

    public bool IsDeath
    {
        get => !IsAlive;
    }

    public bool SetAlive(bool value) => IsAlive = value;
    public void MakeAlive() => IsAlive = true;
    public void MakeDeath() => IsAlive = false;


    protected virtual void OnAliveHasSet(bool value)
    {
        AliveHasSet?.Invoke(this, value);
    }
}