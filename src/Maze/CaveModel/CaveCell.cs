namespace CaveModel;

public sealed class CaveCell: ICloneable, IEquatable<CaveCell>
{
    public CaveCell()
    {
        
    }

    public CaveCell(bool isAlive)
    {
        IsAlive = isAlive;
    }
    public event EventHandler<bool>? AliveHasSet;

    private bool _isAlive;
    public bool IsAlive
    {
        get => _isAlive;
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


    private void OnAliveHasSet(bool value)
    {
        AliveHasSet?.Invoke(this, value);
    }

    public object Clone()
    {
        return new CaveCell(IsAlive);
    }

    public bool Equals(CaveCell? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return _isAlive == other._isAlive;
    }
}