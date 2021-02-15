using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GWOStrategy : HuntingStrategy, MetaHeuristic
{
    public override void Hunt(Animal agent, Dictionary<int, Animal> friendly, Dictionary<int, Animal> foes)
    {
        //TODO: Check if the prey is hunted before doing anything
        if(agent.AnimalMediator.FixedPreyId == -1)
        {
            agent.AnimalMediator.UpdateBestPreyId(foes);
        }

        List<Vec3> predatorPositions = new List<Vec3>();

        foreach (Animal a in friendly.Values)
        {
            if (agent.Id == a.Id)
            {
                predatorPositions.Add(a.Position);
            }
        }

        Animal fixedPrey = foes[agent.AnimalMediator.FixedPreyId];
        Vec3 preyPosition = fixedPrey.Position;
        double preyMaxSquaredSpeed = fixedPrey.MaxSquaredSpeed;

        Vec3 desiredPosition = this.GreyWolfOptimizer(agent.Position, predatorPositions, preyPosition, agent.MaxSquaredSpeed, preyMaxSquaredSpeed);

        throw new System.NotImplementedException();
    }

    //The objective function minimizes the minimum distance between a predator and a prey
    public double objectiveFunction(Vec3 possiblePosition, List<Vec3> predatorPositions, Vec3 preyPosition, double maxSquaredAgentSpeed, double maxSquaredPreySpeed)
    {
        double minDistance = double.MaxValue;

        //Calculate where the prey would move if the agent moves to the possible position 
        Vec3 preyDirection = Vec3.CalculateVectorsBetweenPoints(possiblePosition, preyPosition);


        throw new System.NotImplementedException();
    }

    public Vec3 GreyWolfOptimizer(Vec3 agentPosition, List<Vec3> predatorPosition, Vec3 preyPosition, double maxSquaredAgentSpeed, double maxSquaredPreySpeed)
    {
        return Vec3.Zero();
    }
}
