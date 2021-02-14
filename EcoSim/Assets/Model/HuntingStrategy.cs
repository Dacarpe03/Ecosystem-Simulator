using System.Collections.Generic;

public abstract class HuntingStrategy
{
    public abstract void Hunt(Animal agent, Dictionary<int, Animal> friendly, Dictionary<int, Animal> foes);
}
