using System;
using System.Collections.Generic;

public class AnimalFleeState : AnimalState
{
    public override void Update(List<Animal> friendly, List<Animal> foes)
    {
        if (this._agent.IsSafe)
        {
            this._agent.TransitionTo(new AnimalStillState());
        }
    }

    public Vec3 Avoidance(List<Animal> nearbyAnimals)
    {
        Vec3 avoidanceVector = Vec3.Zero();
        List<Animal> closeAnimals = this.GetNearbyAnimals(nearbyAnimals, this._agent.SquaredVisionRadius);
        int animalCount = closeAnimals.Count;

        if (animalCount > 0) {

            foreach(Animal a in closeAnimals)
            {
                Vec3 force = Vec3.CalculateVectorsBetweenPoints(a.Position, this._agent.Position);
                avoidanceVector.Add(force);
            }

            avoidanceVector.Divide(animalCount);
            avoidanceVector.Trim(this._agent.MaxSquaredSpeed);

            return avoidanceVector;
        }
        else
        {
            return Vec3.Zero();
        }
    }

    public Vec3 Cohesion(List<Animal> nearbyAnimals)
    {
        int animalCount = nearbyAnimals.Count;
        if(animalCount > 0)
        {
            Vec3 centerPosition = Vec3.Zero();
            foreach(Animal a in nearbyAnimals)
            {
                centerPosition.Add(a.Position);
            }

            centerPosition.Divide(animalCount);
            centerPosition.Trim(this._agent.MaxSquaredSpeed);

            return centerPosition;
        }
        else
        {
            return Vec3.Zero();
        }
    }
}
