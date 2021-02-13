using System;
using System.Collections.Generic;

public class AnimalFleeState : AnimalState
{

    private const double WEIGHT_AVOID = 7;
    private const double WEIGHT_COHESION = 2;
    private const double WEIGHT_FOLLOW = 3;
    private const double WEIGHT_CENTER = 10;
    private const double WEIGHT_FLEE = 20;

    private Boid _boidBehaviour = new Boid();

    //Creates a force vector which is the result from Boids algorithm
    public override void Update(List<Animal> friendly, List<Animal> foes)
    {
        Vec3 acceleration = this.BoidBehavior(friendly, foes);

        this._agent.UpdateSpeed(acceleration);
        this._agent.Move();
        

        if (this._agent.Position.XCoord > 400 | this._agent.IsDead)
        {
            this._agent.IsSafe = true;
            this._agent.TransitionTo(new AnimalStillState());
        }
    }

    //Returns a force vector resulting from all the forces of the Boids algorithm 
    private Vec3 BoidBehavior(List<Animal> friendly, List<Animal> foes)
    {

        List<Animal> nearbyAnimals = this.GetNearbyAnimals(friendly, this._agent.SquaredVisionRadius);

        Vec3 acceleration = this._boidBehaviour.CalculateForces(this._agent, nearbyAnimals, foes);

        return acceleration;
    }//END BoidBehavior


    
}
