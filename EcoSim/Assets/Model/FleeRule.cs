using System.Collections;
using System.Collections.Generic;

public class FleeRule : BoidRule
{
    public FleeRule(double w): base(w) { }

    //Avoid predators by creating a repelling force
    public override Vec3 CalculateForce(Animal agent, List<Animal> animals)
    {
        Vec3 fleeVector = Vec3.Zero();
        int nearbyPredatorNumber = 0;
        foreach (Animal a in animals)
        {
            double squaredDistanceToPredator = agent.SquareDistanceTo(a);
            if (squaredDistanceToPredator < agent.SquaredVisionRadius / 4)
            {
                nearbyPredatorNumber++;
                Vec3 force = Vec3.CalculateVectorsBetweenPoints(a.Position, agent.Position);
                fleeVector.Add(force);
            }
        }

        if (nearbyPredatorNumber > 0)
        {
            fleeVector.Divide(nearbyPredatorNumber);
            fleeVector.Multiply(this._weight);
        }

        return fleeVector;
    }
}
