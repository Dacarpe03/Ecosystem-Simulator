using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WOAStrategy : HuntingStrategy, MetaHeuristic
{
    private int METAHEURISTIC_ITERATIONS = 50;
    private int METAHEURISTIC_CANDIDATES = 3;
    private int SIZE_OF_SPACE = 50;

    private class CandidateSolution
    {
        private Vec3 _solution;
        public Vec3 Solution { get => _solution; set => _solution = value; }

        private double _fitness;
        public double Fitness { get => _fitness; set => _fitness = value; }

        public CandidateSolution(Vec3 solution, double fitness)
        {
            this.Solution = solution;
            this.Fitness = fitness;
        }
    }

    public WOAStrategy()
    {
        this.FramesUpdate = 15;
    }

    public override Vec3 GetDesiredPosition(Animal agent, List<Vec3> predatorPositions, Animal fixedPrey)
    {
        //Initialize solutions
        System.Random rand = new System.Random();
        //Declare the space of solutions
        double xUpperLimit = agent.Position.XCoord + this.SIZE_OF_SPACE;
        double xLowerLimit = agent.Position.XCoord - this.SIZE_OF_SPACE;
        double zUpperLimit = agent.Position.ZCoord + this.SIZE_OF_SPACE;
        double zLowerLimit = agent.Position.ZCoord - this.SIZE_OF_SPACE;

        //Initialize the candidate solutions in the space of solutions
        List<CandidateSolution> candidates = new List<CandidateSolution>();

        for (int i = 0; i <= this.METAHEURISTIC_CANDIDATES; i++)
        {
            Vec3 candidateSolution = new Vec3(xUpperLimit, xLowerLimit, 0, 0, zUpperLimit, zLowerLimit, rand);
            double fitness = this.CalculateFitness(candidateSolution, predatorPositions, fixedPrey);
            candidates.Add(new CandidateSolution(candidateSolution, fitness));
        }

        //Main GWO algorithm loop
        for (int i = 0; i <= this.METAHEURISTIC_ITERATIONS; i++)
        {
            //Order the candidates by fitness (the lower the better)
            candidates = candidates.OrderBy(c => c.Fitness).ToList();
            //Fittest Solutions
            Vec3 bestSolution = candidates[0].Solution.Clone();

            //Debug.Log("Mejor fitness iteracion" + i + "lobo "+ agent.Id +" : " + candidates[0].Fitness);
            //Debug.Log("Segunda fitness iteracion" + i + "lobo " + agent.Id + " : " + candidates[1].Fitness);
            //Debug.Log("Tercera fitness iteracion" + i + "lobo " + agent.Id + " : " + candidates[2].Fitness);
            for (int j = 0; j < candidates.Count; j++)
            {
                Vec3 A = CalculateAVector(i, rand);
            }
        }

        //Return the optimal solution found
        candidates = candidates.OrderBy(c => c.Fitness).ToList();
        Debug.Log("Mejor fitness lobo " + agent.Id + " : " + candidates[0].Fitness);
        return candidates[0].Solution;
    }

    private Vec3 CalculateAVector(int iteration, System.Random rand)
    {
        //Scalar a (coords go from 2 to 0 linearly during the iterations)
        double coord = 2 - (iteration * (2 / this.METAHEURISTIC_ITERATIONS));
        Vec3 a = new Vec3(coord, 0, coord);

        //Random vector (coords between 0 and 1)
        Vec3 r = new Vec3(1, 0, 0, 0, 1, 0, rand);

        //A vector
        Vec3 A = Vec3.WolfProduct(a, r);
        A.Multiply(2);
        A.Substract(a);

        return A;
    }

    public override int GetFixedPreyId(Animal agent)
    {
        return agent.Mediator.FixedPreyId;
    }

    public override bool HasFixedPrey(Animal agent)
    {
        return agent.Mediator.FixedPreyId != -1;
    }

    public override void HuntPrey(Animal agent, Animal prey)
    {
        agent.Mediator.PreyHunted(prey);
    }

    public override void SelectPrey(Dictionary<int, Animal> friendly, Dictionary<int, Animal> foes, Animal agent)
    {
        agent.Mediator.UpdateBestPreyId(friendly, foes);
    }

    public double ObjectiveFunction(List<Vec3> predatorPositions, Vec3 preyPosition, Vec3 candidateSolution)
    {
        //We want to minimize the maximum distance of all the predators to the prey
        double maxDistance = candidateSolution.SquaredDistanceTo(preyPosition);
        //double maxDistance = 0;
        //double minDistance = double.MaxValue;
        //Calculate the distance
        foreach (Vec3 position in predatorPositions)
        {
            double sqrdDistance = position.SquaredDistanceTo(preyPosition);
            if (sqrdDistance > maxDistance)
            {
                maxDistance = sqrdDistance;
            }
        }

        return maxDistance;
    }

    public double CalculateFitness(Vec3 candidateSolution, List<Vec3> predatorPositions, Animal prey)
    {
        Vec3 predictedPreyPosition = this.PredictPreyPosition(candidateSolution, prey.Position, prey.MaxSquaredSpeed, prey.VisionRadius);
        double fitness = ObjectiveFunction(predatorPositions, predictedPreyPosition, candidateSolution);
        return fitness;
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
}
