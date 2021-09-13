using System.Collections;
using System.Collections.Generic;

public class PSOStrategy : HuntingStrategy, MetaHeuristic
{
    private int METAHEURISTIC_ITERATIONS = 200;
    private int METAHEURISTIC_CANDIDATES = 10; //MUST BE >= 3
    private int SIZE_OF_SPACE = 30;

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

        public CandidateSolution(Vec3 solution, double fitness)
        {
            this.Solution = solution;
            this.Fitness = fitness;
        }
    }

    public override Vec3 GetDesiredPosition(Animal agent, List<Vec3> friendlyPositions, Animal prey)
    {
        throw new System.NotImplementedException();
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
}
