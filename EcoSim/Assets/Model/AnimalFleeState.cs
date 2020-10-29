using System;
using System.Collections.Generic;

public class AnimalFleeState : AnimalState
{


    private const double WEIGHT_AVOID = 3;
    private const double WEIGHT_COHESION = 1;
    private const double WEIGHT_FOLLOW = 5;
    private const double WEIGHT_CENTER = 1;
    private const double WEIGHT_FLEE = 10;


    private Vec3 CENTER = new Vec3(50, 0, 50);

    public override void Update(List<Animal> friendly, List<Animal> foes)
    {
        Vec3 acceleration = this.BoidBehavior(friendly, foes);

        this._agent.UpdateSpeed(acceleration);
        this._agent.Move();
        

        if (this._agent.IsSafe)
        {
            this._agent.TransitionTo(new AnimalStillState());
        }
    }

    private Vec3 BoidBehavior(List<Animal> friendly, List<Animal> foes)
    {

        List<Animal> nearbyAnimals = this.GetNearbyAnimals(friendly, this._agent.SquaredVisionRadius);
        Vec3 avoidanceVector = this.Avoidance(nearbyAnimals);
        Vec3 cohesionVector = this.Cohesion(nearbyAnimals);
        Vec3 followVector = this.Follow(nearbyAnimals);
        Vec3 goToCenterVector = this.Center();
        Vec3 fleeFromPredatorVector = this.Flee(foes);

        Vec3 acceleration = Vec3.Zero();
        acceleration.Add(avoidanceVector);
        acceleration.Add(cohesionVector);
        acceleration.Add(followVector);
        acceleration.Add(goToCenterVector);
        acceleration.Add(fleeFromPredatorVector);
        acceleration.Trim(this._agent.MaxSquaredSpeed);

        return acceleration;
    }

    private Vec3 Avoidance(List<Animal> nearbyAnimals) //Avoid nearby animals creating a repelling force between them
    {
        Vec3 avoidanceVector = Vec3.Zero();
        List<Animal> closeAnimals = this.GetNearbyAnimals(nearbyAnimals, this._agent.SquaredVisionRadius/144);
        int animalCount = closeAnimals.Count;

        if (animalCount > 0) {

            foreach(Animal a in closeAnimals)
            {
                Vec3 force = Vec3.CalculateVectorsBetweenPoints(a.Position, this._agent.Position);
                avoidanceVector.Add(force);
            }

            avoidanceVector.Divide(animalCount);
            avoidanceVector.Multiply(WEIGHT_AVOID);

            return avoidanceVector;
        }
        else
        {
            return Vec3.Zero();
        }
    }

    private Vec3 Cohesion(List<Animal> nearbyAnimals) //Try to stay together by creating a force that attracts to the center of nearby animals
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
            cohesionForce.Multiply(WEIGHT_COHESION);

            return cohesionForce;
        }
        else
        {
            return Vec3.Zero();
        }
    }

    private Vec3 Follow(List<Animal> nearbyAnimals) //Follow the speed of nearby animals
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
            meanSpeed.Multiply(WEIGHT_FOLLOW);

            return meanSpeed;
        }
        else
        {
            return Vec3.Zero();
        }

    }

    private Vec3 Center() //Stay in the scene
    {
        Vec3 goToCenterVector = Vec3.CalculateVectorsBetweenPoints(this._agent.Position, CENTER);
        if (goToCenterVector.SquaredModule > 2500)
        {
            goToCenterVector.Multiply(WEIGHT_CENTER);
            return goToCenterVector;
        }
        else
        {
            return Vec3.Zero();
        }
    }

    private Vec3 Flee(List<Animal> predators) //Avoid predators by creating a repelling force
    {
        Vec3 fleeVector = Vec3.Zero();
        int nearbyPredatorNumber = 0;
        foreach(Animal a in predators)
        {
            if(this._agent.SquareDistanceTo(a) < this._agent.SquaredVisionRadius)
            {
                nearbyPredatorNumber++;
                Vec3 force = Vec3.CalculateVectorsBetweenPoints(a.Position, this._agent.Position);
                fleeVector.Add(force);
            }
        }

        if(nearbyPredatorNumber > 0)
        {
            fleeVector.Divide(nearbyPredatorNumber);
            fleeVector.Multiply(WEIGHT_FLEE);
        }

        return fleeVector;
    }
}
