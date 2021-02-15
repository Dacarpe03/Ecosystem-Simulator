using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GWOStrategy : HuntingStrategy, MetaHeuristic
{
    private int METAHEURISTIC_ITERATIONS = 20;
    private int METAHEURISTIC_CANDIDATES = 5;

    public override void Hunt(Animal agent, Dictionary<int, Animal> friendly, Dictionary<int, Animal> foes)
    {
        //TODO: Check if the prey is hunted before doing anything
        if(agent.AnimalMediator.FixedPreyId == -1)
        {
            agent.AnimalMediator.UpdateBestPreyId(foes);
        }

        //Get all the positions
        List<Vec3> predatorPositions = new List<Vec3>();

        foreach (Animal a in friendly.Values)
        {
            if (agent.Id == a.Id)
            {
                predatorPositions.Add(a.Position);
            }
        }

        //Get data from fixedPrey
        Animal fixedPrey = foes[agent.AnimalMediator.FixedPreyId];
        Vec3 preyPosition = fixedPrey.Position;
        double preyMaxSquaredSpeed = fixedPrey.MaxSquaredSpeed;
        double preyVisionRadius = fixedPrey.VisionRadius;

        //Calculate optimal position of the predator agent
        Vec3 desiredAgentPosition = this.GreyWolfOptimizer(agent.Position, predatorPositions, preyPosition, agent.MaxSquaredSpeed, preyMaxSquaredSpeed, preyVisionRadius);

        throw new System.NotImplementedException();
    }


    public Vec3 GreyWolfOptimizer(Vec3 agentPosition, List<Vec3> predatorPosition, Vec3 preyPosition, double maxSquaredAgentSpeed, double maxSquaredPreySpeed, double preyVisionRadius)
    {
        //Initialize the solutions
        
    }


    private Vec3 PredictPreyPosition(Vec3 candidatePosition, Vec3 preyPosition, double maxSquaredPreySpeed, double preyVisionRadius)
    {
        //Calculate where the prey would move if the agent moves to the possible position 
        Vec3 preyDirection = Vec3.CalculateVectorsBetweenPoints(candidatePosition, preyPosition);
        double distance = preyDirection.Module;
        preyDirection.Trim(maxSquaredPreySpeed);

        double force = preyVisionRadius / distance;
        preyDirection.Multiply(force);

        return Vec3.Add(preyDirection, preyPosition);
    }


    public double ObjectiveFunction(List<Vec3> predatorPositions, Vec3 preyPosition)
    {
        //We want to minimize the minimun distance of all the predators to the prey
        double minDistance = double.MaxValue;

        //Calculate the distance
        foreach(Vec3 position in predatorPositions)
        {
            double sqrdDistance = position.SquaredDistanceTo(preyPosition);
            if (sqrdDistance < minDistance)
            {
                minDistance = sqrdDistance;
            }
        }

        return minDistance;
    }
}
