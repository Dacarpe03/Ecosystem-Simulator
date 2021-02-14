using System;
using System.Collections.Generic;
using System.Linq;

public class AnimalHuntState : AnimalState
{
    private HuntingStrategy _strategy;

    public AnimalHuntState(HuntingStrategy strategy): base()
    {
        this._strategy = strategy;
    }

    public override void Update(Dictionary<int, Animal> friendly, Dictionary<int, Animal> foes)
    {
        this._strategy.Hunt(this._agent, friendly, foes);
    }
}
