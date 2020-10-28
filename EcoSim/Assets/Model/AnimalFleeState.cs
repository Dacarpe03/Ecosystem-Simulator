﻿using System;
using System.Collections.Generic;

public class AnimalFleeState : AnimalState
{

    private const double X_LIMIT = 200;
    private const double Y_LIMIT = 200;
    private const double Z_LIMIT = 200;

    private const double WEIGHT_AVOID = 0.3;
    private const double WEIGHT_COHESION = 0.6;
    private const double WEIGHT_FOLLOW = 0.1;

    public override void Update(List<Animal> friendly, List<Animal> foes)
    {
        Vec3 acceleration = this.BoidBehavior(friendly);
        Vec3 awayFromBorderForce = this.AwayFromBorders();
        acceleration.Add(awayFromBorderForce);
        acceleration.Trim(this._agent.MaxSquaredSpeed);

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

        Vec3 acceleration = Vec3.Zero();
        acceleration.Add(avoidanceVector);
        acceleration.Add(cohesionVector);
        acceleration.Add(followVector);
        acceleration.Trim(this._agent.MaxSquaredSpeed);

        return acceleration;
    }

    public Vec3 Avoidance(List<Animal> nearbyAnimals)
    {
        Vec3 avoidanceVector = Vec3.Zero();
        List<Animal> closeAnimals = this.GetNearbyAnimals(nearbyAnimals, this._agent.SquaredVisionRadius/5);
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

    public Vec3 AwayFromBorders()
    {
        Vec3 agentPosition = this._agent.Position;
        double xCoord = agentPosition.XCoord;
        double yCoord = agentPosition.YCoord;
        double zCoord = agentPosition.ZCoord;

        Vec3  agentSpeed = this._agent.Speed;
        double xSpeed = agentSpeed.XCoord;
        double ySpeed = agentSpeed.YCoord;
        double zSpeed = agentSpeed.ZCoord;

        Vec3 forceFromBorder = Vec3.Zero();
        if (xCoord > X_LIMIT | xCoord < 0)
        {
            forceFromBorder = new Vec3(-xSpeed, ySpeed, zSpeed);
        }
        else if(yCoord > Y_LIMIT | yCoord < 0)
        {
            forceFromBorder = new Vec3(xSpeed, -ySpeed, zSpeed);
        }
        else if(zCoord > Z_LIMIT | zCoord < 0)
        {
            forceFromBorder = new Vec3(xSpeed, ySpeed, -zSpeed);
        }

        return forceFromBorder;
    }
}
