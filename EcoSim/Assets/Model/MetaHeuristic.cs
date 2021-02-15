using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface MetaHeuristic
{
    double objectiveFunction(Dictionary<int, Animal> predators, Animal prey);
}
