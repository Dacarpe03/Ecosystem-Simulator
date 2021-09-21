using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class SimpleStrategy: HuntingStrategy
{

    private const int PREYS_NEEDED_TO_SURVIVE = 1;
    private int _preysHunted = 0;

    private Boolean preyFixed = false;
    private int fixedPreyId = -1;
    
    public SimpleStrategy()
    {
        this.FramesUpdate = 1;
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


    private List<Animal> GetNearbyAnimals(Animal agent, Dictionary<int, Animal> animals, double squareRadius)
    {
        List<Animal> nearbyAnimals = new List<Animal>();
        foreach (Animal a in animals.Values)
        {
            if (agent.SquareDistanceTo(a) <= squareRadius)
            {
                nearbyAnimals.Add(a);
            }
        }

        return nearbyAnimals;
    }

    public override bool HasFixedPrey(Animal agent)
    {
        return this.fixedPreyId != -1;
    }

    public override void SelectPrey(Dictionary<int, Animal> friendly, Dictionary<int, Animal> foes, Animal agent)
    {
        List<Animal> nearbyPreys = this.GetNearbyAnimals(agent, foes, agent.SquaredVisionRadius);
        if (nearbyPreys.Count > 0)
        {
            fixedPreyId = this.GetBestPrey(agent, nearbyPreys);
        }
    }

    public override int GetFixedPreyId(Animal agent)
    {
        return this.fixedPreyId;
    }

    public override void HuntPrey(Animal agent, Animal prey)
    {
        this._preysHunted += 1;
        this.preyFixed = false;
        if (this._preysHunted >= PREYS_NEEDED_TO_SURVIVE)
        {
            agent.HasEaten = true;
            agent.IsDead = false;
            agent.IsSafe = false;
        }

    }

    public override Vec3 GetDesiredPosition(Animal agent, List<Vec3> friendlyPositions, Animal prey)
    {
        return prey.Position;
    }
}
