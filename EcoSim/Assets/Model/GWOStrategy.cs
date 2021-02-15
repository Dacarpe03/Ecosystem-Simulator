using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GWOStrategy : HuntingStrategy, MetaHeuristic
{
    public override void Hunt(Animal agent, Dictionary<int, Animal> friendly, Dictionary<int, Animal> foes)
    {
        throw new System.NotImplementedException();
    }

    public double objectiveFunction(Dictionary<int, Animal> predators, Animal prey)
    {
        throw new System.NotImplementedException();
    }
}
