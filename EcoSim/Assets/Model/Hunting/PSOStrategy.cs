using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PSOStrategy : HuntingStrategy, MetaHeuristic
{
    private int METAHEURISTIC_ITERATIONS = 50;
    private int METAHEURISTIC_CANDIDATES = 3;
    private int SIZE_OF_SPACE = 50;

    private double MAX_VELOCITY = 1;
    private int C1 = 2;
    private int C2 = 2;
    private double INITIAL_INERTIA = 0.9;
    private double MINIMUM_INERTIA = 0.4;

    private class CandidateSolution
    {
        private Vec3 _solution;
        private Vec3 _velocity;
        private Vec3 _personalBest;

        public Vec3 Solution { get => _solution; set => _solution = value; }
        public Vec3 Velocity { get => _velocity; set => _velocity = value; }
        public Vec3 PersonalBest { get => _personalBest; set => _personalBest = value; }

        private double _fitness;
        public double Fitness { get => _fitness; set => _fitness = value; }

        public CandidateSolution(Vec3 solution, Vec3 velocity, double fitness)
        {
            this.Solution = solution;
            this.PersonalBest = solution;
            this.Velocity = velocity;
            this.Fitness = fitness;
        }
    }

    public PSOStrategy(int iterations, int candidates)
    {
        this.METAHEURISTIC_ITERATIONS = iterations;
        this.METAHEURISTIC_CANDIDATES = candidates;
        this.FramesUpdate = 15;
    }
    //PSO Algorithm
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
            Vec3 initialSolution = new Vec3(xUpperLimit, xLowerLimit, 0, 0, zUpperLimit, zLowerLimit, rand);
            Vec3 initialSpeed = new Vec3(this.MAX_VELOCITY, 0, 0, 0, this.MAX_VELOCITY, 0, rand);
            double fitness = this.CalculateFitness(initialSolution, predatorPositions, fixedPrey);
            candidates.Add(new CandidateSolution(initialSolution, initialSpeed, fitness));
        }

        //Main PSO algorithm loop
        for (int i = 0; i <= this.METAHEURISTIC_ITERATIONS; i++)
        {
            //Order the candidates by fitness (the lower the better)
            candidates = candidates.OrderBy(c => c.Fitness).ToList();
            //Fittest Solution
            Vec3 globalBest = candidates[0].PersonalBest.Clone();
            double inertia = this.INITIAL_INERTIA - (i * (this.INITIAL_INERTIA - this.MINIMUM_INERTIA) / this.METAHEURISTIC_ITERATIONS);

            for (int j = 0; j < candidates.Count; j++)
            {
                //w*Velocity
                Vec3 newVelocity = candidates[j].Velocity.Clone();
                newVelocity.Multiply(inertia);

                //c1*r1*(PersonalBest - ActualSolution)
                Vec3 cognitiveComponent = Vec3.Substract(candidates[j].PersonalBest, candidates[j].Solution);
                cognitiveComponent.Multiply(this.C1);
                double r1 = rand.NextDouble();
                cognitiveComponent.Multiply(r1);

                //c2*r2*(GlobalBest - ActualSolution)
                Vec3 socialComponent = Vec3.Substract(globalBest, candidates[j].Solution);
                socialComponent.Multiply(this.C2);
                double r2 = rand.NextDouble();
                socialComponent.Multiply(r2);

                //new velocity = w*Velocity + c1*r1*(PersonalBest - ActualSolution) + c2*r2*(GlobalBest - ActualSolution)
                newVelocity.Add(cognitiveComponent);
                newVelocity.Add(socialComponent);

                //New solution = ActualSolution + NewSpeed
                candidates[j].Velocity = newVelocity;
                Vec3 newSolution = Vec3.Add(candidates[j].Solution, candidates[j].Velocity);
                candidates[j].Solution = newSolution;

                double actualFitness = this.CalculateFitness(newSolution, predatorPositions, fixedPrey);
                if (actualFitness < candidates[j].Fitness)
                {
                    candidates[j].Fitness = actualFitness;
                    candidates[j].PersonalBest = newSolution;
                }
            }
        }

        //Return the optimal solution found
        candidates = candidates.OrderBy(c => c.Fitness).ToList();
        return candidates[0].PersonalBest;
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
