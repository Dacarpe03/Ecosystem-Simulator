using System.Collections.Generic;

interface MetaHeuristic
{
    double ObjectiveFunction(List<Vec3> predatorPositions, Vec3 preyPosition, Vec3 candidateSolution);
    double CalculateFitness(Vec3 candidateSolution, List<Vec3> predatorPositions, Animal prey);

}
