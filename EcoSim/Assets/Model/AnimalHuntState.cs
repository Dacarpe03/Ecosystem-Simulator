using System;
using System.Collections.Generic;
using System.Linq;

public class AnimalHuntState : AnimalState
{
    private Boolean preyFixed = false;
    private int fixedPreyId = -1;

    public override void Update(List<Animal> friendly, List<Animal> foes)
    {
        if (!preyFixed) 
        {
            List<Animal> nearbyPreys = this.GetNearbyAnimals(foes, this._agent.SquaredVisionRadius);
            fixedPreyId = this.GetSlowestAnimal(nearbyPreys);
            preyFixed = true; 
        }

        List<Vec3> preyPositions = foes.Where(a => a.Id == fixedPreyId).Select(a => a.Position).ToList();
        Vec3 fixedPreyPosition = preyPositions.First();

        Vec3 force = Vec3.CalculateVectorsBetweenPoints(this._agent.Position, fixedPreyPosition);
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
