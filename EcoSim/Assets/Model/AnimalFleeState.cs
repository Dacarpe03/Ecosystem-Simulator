using System;
using System.Collections.Generic;

public class AnimalFleeState : AnimalState
{


    private const double WEIGHT_AVOID = 2;
    private const double WEIGHT_COHESION = 1;
    private const double WEIGHT_FOLLOW = 4;
    private const double WEIGHT_CENTER = 1;


    private Vec3 CENTER = new Vec3(50, 0, 50);

    public override void Update(List<Animal> friendly, List<Animal> foes)
    {
        Vec3 acceleration = this.BoidBehavior(friendly);

        this._agent.UpdateSpeed(acceleration);
        this._agent.Move();
        

        if (this._agent.IsSafe)
        {
            this._agent.TransitionTo(new AnimalStillState());
        }
    }

    public Vec3 BoidBehavior(List<Animal> friendly)
    {

        List<Animal> nearbyAnimals = this.GetNearbyAnimals(friendly, this._agent.SquaredVisionRadius);
        Vec3 avoidanceVector = this.Avoidance(nearbyAnimals);
        Vec3 cohesionVector = this.Cohesion(nearbyAnimals);
        Vec3 followVector = this.Follow(nearbyAnimals);
        Vec3 goToCenterVector = this.Center();


        if (avoidanceVector.IsZero())
        {
            cohesionVector.Multiply(WEIGHT_COHESION + WEIGHT_AVOID / 2);
            followVector.Multiply(WEIGHT_FOLLOW + WEIGHT_AVOID / 2);
        }
        else
        {
            avoidanceVector.Multiply(WEIGHT_AVOID);
            cohesionVector.Multiply(WEIGHT_COHESION);
            followVector.Multiply(WEIGHT_FOLLOW);
        }

        goToCenterVector.Multiply(WEIGHT_CENTER);

        Vec3 acceleration = Vec3.Zero();
        acceleration.Add(avoidanceVector);
        acceleration.Add(cohesionVector);
        acceleration.Add(followVector);
        acceleration.Add(goToCenterVector);
        acceleration.Trim(this._agent.MaxSquaredSpeed);

        return acceleration;
    }

    public Vec3 Avoidance(List<Animal> nearbyAnimals)
    {
        Vec3 avoidanceVector = Vec3.Zero();
        List<Animal> closeAnimals = this.GetNearbyAnimals(nearbyAnimals, this._agent.SquaredVisionRadius/12);
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
            Vec3 cohesionForce = Vec3.CalculateVectorsBetweenPoints(this._agent.Position, centerPosition);
            cohesionForce.Trim(this._agent.MaxSquaredSpeed);

            return cohesionForce;
        }
        else
        {
            return Vec3.Zero();
        }
    }

    public Vec3 Follow(List<Animal> nearbyAnimals)
    {
        int animalCount = nearbyAnimals.Count;
        if (animalCount > 0)
        {
            Vec3 meanSpeed = Vec3.Zero();
            foreach(Animal a in nearbyAnimals)
            {
                meanSpeed.Add(a.Speed);
            }

            meanSpeed.Divide(animalCount);
            meanSpeed.Trim(this._agent.MaxSquaredSpeed);

            return meanSpeed;
        }
        else
        {
            return Vec3.Zero();
        }

    }

    public Vec3 Center()
    {
        Vec3 goToCenterVector = Vec3.CalculateVectorsBetweenPoints(this._agent.Position, CENTER);
        if (goToCenterVector.SquaredModule > 2500)
        {
            goToCenterVector.Trim(this._agent.MaxSquaredSpeed);
            return goToCenterVector;
        }
        else
        {
            return Vec3.Zero();
        }
    }
}
