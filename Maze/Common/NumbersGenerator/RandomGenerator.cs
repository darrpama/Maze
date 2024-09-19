namespace Common.NumbersGenerator;

public class RandomGenerator(Random random): IGenerator
{
    private Random _random = random;

    public bool NextBool()
    {
        var nextDouble = random.NextDouble();
        return nextDouble >= 0.5;
    }
}