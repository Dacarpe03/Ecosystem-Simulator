using System;
using System.Collections.Generic;
using System.Linq;

public class SimpleStrategy
{

    private const int PREYS_NEEDED_TO_SURVIVE = 1;
    private int _preysHunted = 0;

    private Boolean preyFixed = false;
    private int fixedPreyId = -1;

    public void Hunt(Animal agent, Dictionary<int, Animal> friendly, Dictionary<int, Animal> foes)
    {
        if (_preysHunted >= PREYS_NEEDED_TO_SURVIVE)
        {
            agent.IsDead = false;
            agent.IsSafe = false;
            agent.HasEaten = true;
        }
        else
        {
            agent.IsDead = true;
        }

        if (!this.preyFixed)
        {
            List<Animal> nearbyPreys = this.GetNearbyAnimals(agent, foes, agent.SquaredVisionRadius);
            if (nearbyPreys.Count > 0)
            {
                fixedPreyId = this.GetBestPrey(agent, nearbyPreys);
                if (fixedPreyId == -1)
                {
                    Vec3 avoid = this.Avoidance(agent, friendly);
                    avoid.Multiply(0.2);
                    agent.UpdateSpeed(avoid);
                    agent.Move();
                }
                else
                {
                    preyFixed = true;
                }
            }
            else
            {
                Vec3 avoid = this.Avoidance(agent, friendly);
                avoid.Multiply(0.2);
                agent.UpdateSpeed(avoid);
                agent.Move();
            }
        }
        else if (foes.ContainsKey(fixedPreyId))
        {

            Animal fixedPrey = (Animal)foes.Where(a => a.Value.Id == fixedPreyId).Select(a => a.Value).ToList().First();
            if (fixedPrey.IsDead | fixedPrey.IsSafe)
            {
                this.preyFixed = false;
            }
            else if (fixedPrey.SquareDistanceTo(agent) < 4)
            {
                fixedPrey.IsDead = true;
                fixedPrey.IsSafe = true;
                fixedPrey.TransitionTo(new AnimalStillState());
                this._preysHunted += 1;
                this.preyFixed = false;
            }

            Vec3 acceleration = Vec3.CalculateVectorsBetweenPoints(agent.Position, fixedPrey.Position);
            acceleration.Add(this.Avoidance(agent, friendly));
            acceleration.Expand(agent.MaxSpeed);

            agent.UpdateSpeed(acceleration);
            agent.Move();
        }
        else
        {
            this.preyFixed = false;
        }
    }

    private int GetBestPrey(Animal agent, List<Animal> nearbyPreys)
    {
        double maxFitness = 0;
        int idFixed = -1;
        foreach (Animal p in nearbyPreys)
        {
            double preySpeed = p.Speed.Module;
            double distanceToPrey = Vec3.CalculateVectorsBetweenPoints(agent.Position, p.Position).Module;
            double fitness = distanceToPrey / preySpeed;
            if (fitness > maxFitness & (!p.IsDead | !p.IsSafe))
            {
                idFixed = p.Id;
                maxFitness = fitness;
            }
        }

        return idFixed;
    }

    private Vec3 Avoidance(Animal agent, Dictionary<int, Animal> nearbyAnimals) //Avoid nearby animals creating a repelling force between them
    {
        Vec3 avoidanceVector = Vec3.Zero();
        List<Animal> closeAnimals = this.GetNearbyAnimals(agent, nearbyAnimals, 2.5);
        int animalCount = closeAnimals.Count;

        if (animalCount > 0)
        {

            foreach (Animal a in closeAnimals)
            {
                Vec3 force = Vec3.CalculateVectorsBetweenPoints(a.Position, agent.Position);
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

    private List<Animal> GetNearbyAnimals(Animal agent, Dictionary<int, Animal> animals, double squareRadius)
    {
        List<Animal> nearbyAnimals = new List<Animal>();
        foreach (Animal a in animals.Values)
        {
            if (a.Id != agent.Id & agent.SquareDistanceTo(a) <= squareRadius)
            {
                nearbyAnimals.Add(a);
            }
        }

        return nearbyAnimals;
    }
}
