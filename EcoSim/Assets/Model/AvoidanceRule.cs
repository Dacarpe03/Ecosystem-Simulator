using System.Collections;
using System.Collections.Generic;

public class AvoidanceRule : BoidRule
{
    public AvoidanceRule(double w): base(w){}

    //Avoid nearby animals creating a repelling force between them
    public override Vec3 CalculateForce(Animal agent, List<Animal> animals)
    {
        List<Animal> closeAnimals = GetCloseAnimals(agent, animals, 2.5);
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
    
    private List<Animal> GetCloseAnimals(Animal agent, List<Animal> animals, double squareRadius)
    {
        List<Animal> closeAnimals = new List<Animal>();
        foreach (Animal a in animals)
        {
            if (a.Id != agent.Id & agent.SquareDistanceTo(a) <= squareRadius)
            {
                closeAnimals.Add(a);
            }
        }

        return closeAnimals;
    }
}
