using System;
using System.Collections.Generic;
using System.Linq;

public class AnimalHuntState : AnimalState
{
    private Boolean preyFixed;
    private int preyFixedId;

    public override void Update(List<Animal> friendly, List<Animal> foes)
    {
        if (!preyFixed) 
        {
            List<Animal> nearbyPreys = this.GetNearbyAnimals(foes, this._agent.SquaredVisionRadius);
            preyFixedId = this.GetSlowestAnimal(nearbyPreys);
        }

        Vec3 preyPosition = (Vec3) friendly.Where(a => a.Id == preyFixedId);
        Vec3 force = Vec3.CalculateVectorsBetweenPoints(this._agent.Position, preyPosition);
        force.Trim(this._agent.MaxSquaredSpeed);

        this._agent.UpdateSpeed(force);
        this._agent.Move();
    }
    
    private int GetSlowestAnimal(List<Animal> nearbyPreys)
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
