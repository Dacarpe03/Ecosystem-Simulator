using System.Collections;
using System.Collections.Generic;

public class AvoidanceRule : BoidRule
{
    private double _squaredAvoidanceRadius;
    public AvoidanceRule(double w, double squareRadius): base(w){
        this._squaredAvoidanceRadius = squareRadius;
    }


    //Avoid nearby animals creating a repelling force between them
    public override Vec3 CalculateForce(Animal agent, List<Animal> animals)
    {
        List<Animal> closeAnimals = GetCloseAnimals(agent, animals);
        Vec3 avoidanceVector = Vec3.Zero();
        int animalCount = closeAnimals.Count;

        if (animalCount > 0)
        {

            foreach (Animal a in closeAnimals)
            {
                Vec3 force = Vec3.CalculateVectorsBetweenPoints(a.Position, agent.Position);
                avoidanceVector.Add(force);
            }

            avoidanceVector.Divide(animalCount);
            avoidanceVector.Multiply(this._weight);

            return avoidanceVector;
        }
        else
        {
            return Vec3.Zero();
        }
    }
    
    private List<Animal> GetCloseAnimals(Animal agent, List<Animal> animals)
    {
        List<Animal> closeAnimals = new List<Animal>();
        foreach (Animal a in animals)
        {
            if (a.Id != agent.Id & agent.SquareDistanceTo(a) <= this._squaredAvoidanceRadius)
            {
                closeAnimals.Add(a);
            }
        }

        return closeAnimals;
    }
}
