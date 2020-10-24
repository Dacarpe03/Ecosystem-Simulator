using System;
using System.Collections.Generic;

public class AnimalFleeState : AnimalState
{
    public override void Update(List<Animal> friendly, List<Animal> foes)
    {
        this._agent.Flee();
        if (this._agent.IsSafe)
        {
            this._agent.TransitionTo(new AnimalStillState());
        }
    }
}
