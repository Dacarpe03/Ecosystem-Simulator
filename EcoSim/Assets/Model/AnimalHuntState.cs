using System;
using System.Collections.Generic;
using System.Linq;

public class AnimalHuntState : AnimalState
{
    private const int PREYS_NEEDED_TO_SURVIVE = 1;
    private int _preysHunted = 0;

    private Boolean preyFixed = false;
    private int fixedPreyId = -1;

    public override void Update(Dictionary<int, Animal> friendly, Dictionary<int, Animal> foes)
    {
        if(_preysHunted >= PREYS_NEEDED_TO_SURVIVE)
        {
            this._agent.IsDead = false;
            this._agent.IsSafe = true;
        }
        else
        {
            this._agent.IsDead = true;
        }

        if (!preyFixed)
        {
            List<Animal> nearbyPreys = this.GetNearbyAnimals(foes, this._agent.SquaredVisionRadius);
            if (nearbyPreys.Count > 0)
            {
                fixedPreyId = this.GetBestPrey(nearbyPreys);
                if(fixedPreyId == -1)
                {
                    Vec3 avoid = this.Avoidance(friendly);
                    avoid.Multiply(0.2);
                    this._agent.UpdateSpeed(avoid);
                    this._agent.Move();
                }
                else
                {
                    preyFixed = true;
                }
            }
            else
            {
                Vec3 avoid = this.Avoidance(friendly);
                avoid.Multiply(0.2);
                this._agent.UpdateSpeed(avoid);
                this._agent.Move();
            }
        }
        else
        {

            Animal fixedPrey = (Animal)foes.Where(a => a.Value.Id == fixedPreyId).Select(a => a.Value).ToList().First();
            if (fixedPrey.IsDead | fixedPrey.IsSafe)
            {
                this.preyFixed = false;
            }
            else if(fixedPrey.SquareDistanceTo(this._agent) < 4)
            {
                fixedPrey.IsDead = true;
                fixedPrey.IsSafe = true;
                fixedPrey.TransitionTo(new AnimalStillState());
                this._preysHunted += 1;
            }

            Vec3 acceleration = Vec3.CalculateVectorsBetweenPoints(this._agent.Position, fixedPrey.Position);
            acceleration.Add(this.Avoidance(friendly));
            acceleration.Expand(this._agent.MaxSpeed);

            this._agent.UpdateSpeed(acceleration);
            this._agent.Move();
        }
    }
    
    private int GetBestPrey(List<Animal> nearbyPreys)
    {
        double maxFitness = 0;
        int idFixed = -1;
        foreach(Animal p in nearbyPreys)
        {
            double preySpeed = p.Speed.Module;
            double distanceToPrey = Vec3.CalculateVectorsBetweenPoints(this._agent.Position, p.Position).Module;
            double fitness = distanceToPrey / preySpeed;
            if(fitness > maxFitness & (!p.IsDead | !p.IsSafe))
            {
                idFixed = p.Id;
                maxFitness = fitness;
            }
        }

        return idFixed;
    }

    private Vec3 Avoidance(Dictionary<int, Animal> nearbyAnimals) //Avoid nearby animals creating a repelling force between them
    {
        Vec3 avoidanceVector = Vec3.Zero();
        List<Animal> closeAnimals = this.GetNearbyAnimals(nearbyAnimals, 2.5);
        int animalCount = closeAnimals.Count;

        if (animalCount > 0)
        {

            foreach (Animal a in closeAnimals)
            {
                Vec3 force = Vec3.CalculateVectorsBetweenPoints(a.Position, this._agent.Position);
                avoidanceVector.Add(force);
            }

            avoidanceVector.Divide(animalCount);

            return avoidanceVector;
        }
        else
        {
            return Vec3.Zero();
        }
    }
}
