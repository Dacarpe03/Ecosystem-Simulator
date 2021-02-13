using System.Collections;
using System.Collections.Generic;
public abstract class BoidRule
{
    public abstract Vec3 CalculateForce(Animal agent, List<Animal> animals);
}
