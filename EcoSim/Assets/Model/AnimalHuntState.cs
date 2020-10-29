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
            if (nearbyPreys.Count > 0)
            {
                fixedPreyId = this.GetSlowestAnimal(nearbyPreys);
                preyFixed = true;
            }
            else
            {
                this._agent.Move();
            }
        }
        else
        {

            Animal fixedPrey = (Animal)foes.Where(a => a.Id == fixedPreyId).Select(a => a).ToList().First();

            Vec3 acceleration = Vec3.CalculateVectorsBetweenPoints(this._agent.Position, fixedPrey.Position);
            acceleration.Expand(this._agent.MaxSpeed);

            this._agent.UpdateSpeed(acceleration);
            this._agent.Move();

            if(this._agent.SquareDistanceTo(fixedPrey) < 1)
            {
                fixedPrey.IsDead = true;
                fixedPrey.IsSafe = true;
            }
        }
    }
    
    private int GetSlowestAnimal(List<Animal> nearbyPreys)
    {
        double maxSpeed = Double.MaxValue;
        int idFixed = -1;
        foreach(Animal p in nearbyPreys)
        {
           double preySpeed = p.Speed.SquaredModule;
           if(preySpeed < maxSpeed & !p.IsDead)
            {
                maxSpeed = preySpeed;
                idFixed = p.Id;
            }
        }

        return idFixed;
    }
}
