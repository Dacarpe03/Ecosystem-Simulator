using System;
using System.Collections.Generic;

public class AnimalHuntState : AnimalState
{
    private Boolean preyFixed;
    public override void Update(List<Animal> friendly, List<Animal> foes)
    {
        List<Animal> nearbyPreys = this.GetNearbyAnimals(foes, this._agent.SquaredVisionRadius);
    }

    private 
}
