namespace Common.NumbersGenerator;

public class SequenceGenerator : IGenerator
{
    private IEnumerator<int> _enumerator;

    public SequenceGenerator(IEnumerable<int> numbers)
    {
        _enumerator = numbers.Cast<int>().GetEnumerator();
    }

    public bool NextBool()
    {
        if (!_enumerator.MoveNext()) throw new IndexOutOfRangeException();
        
        var value = _enumerator.Current == 1;
        return value;
    }
}