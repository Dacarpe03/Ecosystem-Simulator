using System.Collections.Generic;

interface MetaHeuristic
{
    double ObjectiveFunction(List<Vec3> predatorPositions, Vec3 preyPosition);
}
