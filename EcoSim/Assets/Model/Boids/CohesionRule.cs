using System.Collections;
using System.Collections.Generic;
public class CohesionRule : BoidRule
{
    public CohesionRule(double w) : base(w) { }

    //Try to stay together by creating a force that attracts to the center of nearby animals
    public override Vec3 CalculateForce(Animal agent, List<Animal> animals)
    {
        int animalCount = animals.Count;
        if (animalCount > 0)
        {
            Vec3 centerPosition = Vec3.Zero();
            foreach (Animal a in animals)
            {
                centerPosition.Add(a.Position);
            }

            centerPosition.Divide(animalCount);
            Vec3 cohesionForce = Vec3.CalculateVectorsBetweenPoints(agent.Position, centerPosition);
            cohesionForce.Multiply(this._weight);

            return cohesionForce;
        }
        else
        {
            return Vec3.Zero();
        }
    }
}
