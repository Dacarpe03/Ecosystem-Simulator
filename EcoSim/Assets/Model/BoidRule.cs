using System.Collections;
using System.Collections.Generic;
public abstract class BoidRule
{
    protected double _weight;

    public BoidRule(double w)
    {
        this._weight = w;
    }

    public abstract Vec3 CalculateForce(Animal agent, List<Animal> animals);
}
