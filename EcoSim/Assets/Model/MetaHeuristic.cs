using System.Collections.Generic;

interface MetaHeuristic
{
    double objectiveFunction(Dictionary<int, Animal> predators, Animal prey);
}
