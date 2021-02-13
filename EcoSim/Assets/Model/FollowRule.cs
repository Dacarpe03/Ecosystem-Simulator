using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRule : BoidRule
{
    public FollowRule(double w) : base(w) { }

    //Follow the speed of nearby animals
    public override Vec3 CalculateForce(Animal agent, List<Animal> animals)
    {
        int animalCount = animals.Count;
        if (animalCount > 0)
        {
            Vec3 meanSpeed = Vec3.Zero();
            foreach (Animal a in animals)
            {
                meanSpeed.Add(a.Speed);
            }

            meanSpeed.Divide(animalCount);
            meanSpeed.Multiply(this._weight);

            return meanSpeed;
        }
        else
        {
            return Vec3.Zero();
        }
    }

}
