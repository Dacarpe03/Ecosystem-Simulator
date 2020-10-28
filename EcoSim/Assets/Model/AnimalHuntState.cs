using System;
using System.Collections.Generic;

public class AnimalHuntState : AnimalState
{
    private Boolean preyFixed;
    private int preyFixedId;

    public override void Update(List<Animal> friendly, List<Animal> foes)
    {
        if (!preyFixed) 
        {
            List<Animal> nearbyPreys = this.GetNearbyAnimals(foes, this._agent.SquaredVisionRadius);
            preyFixedId = getSlowestAnimal(nearbyPreys);
        }
    }
    
    private int getSlowestAnimal(List<Animal> nearbyPreys)
    {
        double maxSpeed = Double.MaxValue;
        int idFixed = -1;
        foreach(Animal p in nearbyPreys)
        {
            double preySpeed = p.Speed.SquaredModule;
           if(preySpeed < maxSpeed)
            {
                maxSpeed = preySpeed;
                idFixed = p.Id;
            }
        }

        return idFixed;
    }
}
