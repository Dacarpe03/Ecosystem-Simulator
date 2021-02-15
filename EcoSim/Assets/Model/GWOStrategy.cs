using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class GWOStrategy : HuntingStrategy, MetaHeuristic
{
    private int METAHEURISTIC_ITERATIONS = 20;
    private int METAHEURISTIC_CANDIDATES = 5; //MUST BE >= 3
    private int SIZE_OF_SPACE = 25;

    private class CandidateSolution
    {
        private Vec3 _solution;
        public Vec3 Solution { get => _solution; set => _solution = value; }

        private double _fitness;
        public double Fitness{ get => _fitness; set => _fitness = value; }

        public CandidateSolution(Vec3 solution, double fitness)
        {
            this.Solution = solution;
            this.Fitness = fitness;
        }
    }

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

        //Calculate optimal position of the predator agent
        Vec3 desiredAgentPosition = this.GreyWolfOptimizer(agent, predatorPositions, fixedPrey);

        throw new System.NotImplementedException();
    }


    public Vec3 GreyWolfOptimizer(Animal agent, List<Vec3> predatorPositions, Animal fixedPrey)
    {
        //Initialize the candidateSolutions in the 
        Random rand = new Random();

        //Declare the space of solutions
        double xUpperLimit = agent.Position.XCoord + this.SIZE_OF_SPACE;
        double xLowerLimit = agent.Position.XCoord - this.SIZE_OF_SPACE;
        double zUpperLimit = agent.Position.ZCoord + this.SIZE_OF_SPACE;
        double zLowerLimit = agent.Position.ZCoord - this.SIZE_OF_SPACE;

        //Initialize the candidate solutions in the space of solutions
        List<CandidateSolution> candidates = new List<CandidateSolution>();

        for(int i=0; i<=this.METAHEURISTIC_CANDIDATES; i++)
        {
            Vec3 candidateSolution = new Vec3(xUpperLimit, xLowerLimit, 0, 0, zUpperLimit, zLowerLimit, rand);
            double fitness = this.CalculateFitness(candidateSolution, predatorPositions, fixedPrey);
            candidates.Add(new CandidateSolution(candidateSolution, fitness));
        }

        //Main GWO algorithm loop
        for(int i=0; i<=this.METAHEURISTIC_ITERATIONS; i++)
        {
            //Order the candidates by fitness (the lower the better)
            candidates = candidates.OrderBy(c => c.Fitness).ToList();

            //Fittest Solutions
            Vec3 alphaSol = candidates[0].Solution.Clone();
            Vec3 betaSol = candidates[1].Solution.Clone();
            Vec3 gamaSol = candidates[2].Solution.Clone();

            foreach(CandidateSolution cand in candidates)
            {
                //Random vectors (coords between 0 and 1)
                Vec3 r1 = new Vec3(1, 0, 0, 0, 1, 0, rand);
                Vec3 r2 = new Vec3(1, 0, 0, 0, 1, 0, rand);

                //Scalar a (coords go from 2 to 0 lineally during the iterations)
                double a = 2 - i * (2 / this.METAHEURISTIC_ITERATIONS);

                //

            }


        }
        
        return Vec3.Zero();
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

    private double CalculateFitness(Vec3 candidateSolution, List<Vec3> predatorPositions, Animal prey)
    {
        Vec3 predictedPreyPosition = this.PredictPreyPosition(candidateSolution, prey.Position, prey.MaxSquaredSpeed, prey.VisionRadius);
        return ObjectiveFunction(predatorPositions, prey.Position);
    }
}
