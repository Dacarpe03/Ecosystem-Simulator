using System.Collections;
using System.Collections.Generic;
public abstract class BoidRule
{
    protected double _weight;
    public abstract Vec3 CalculateForce(Animal agent, List<Animal> animals);
}
