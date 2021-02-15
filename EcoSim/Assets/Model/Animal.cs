using System;
using System.Collections.Generic;

public class Animal
{
    //SECTION: Attributes and properties
    private const double STEER_FORCE = 0.7;

    private int _id;
    public int Id { get => _id; }

    private AnimalState _state;
    public AnimalState State { get => _state; set => _state = value; }


    private double _maxSpeed;
    public double MaxSpeed { get => _maxSpeed; }
    private double _maxSquaredSpeed; //So that the computation of the norm of the vector skips one step, the sqrt
    public double MaxSquaredSpeed { get => _maxSquaredSpeed; }
    

    private double _visionRadius;
    public double VisionRadius { get => _visionRadius; }
    private double _squaredVisionRadius;
    public double SquaredVisionRadius { get => _squaredVisionRadius; }


    private Boolean _isSafe;
    public Boolean IsSafe { get => _isSafe; set => _isSafe = value; }


    private Boolean _isDead = false;
    public Boolean IsDead { get => _isDead; set => _isDead = value; }


    private Vec3 _position;
    public Vec3 Position { get => _position; set => _position = value; }


    private Vec3 _speed;
    public Vec3 Speed { get => _speed; set => _speed = value; }

    private AnimalMediator _predatorCommunication;
    public AnimalMediator AnimalMediator { get => _predatorCommunication; set => _predatorCommunication = value; }
    //END: Attributes and properties

    //SECTION: Constructor and main methods
    public Animal(AnimalState state, double maxSpeed, double visionRadius, int id, Random rand)
    {
        this.TransitionTo(state);

        this._isSafe = false;

        this._maxSpeed = maxSpeed;
        this._maxSquaredSpeed = maxSpeed * maxSpeed;

        this._visionRadius = visionRadius;
        this._squaredVisionRadius = visionRadius * visionRadius;

        this._position = new Vec3(rand);
        this._speed = new Vec3(rand);
        this._speed.Trim(this._maxSquaredSpeed);

        this._id = id;
    }


    //Method to transition between states
    public void TransitionTo(AnimalState newState)
    {
        this._state = newState;
        this._state.Agent = this;
    }//END TransitionTo

    public void Move()
    {
        this._position.Add(Speed);
    }//END Move


    //Update the speed of the animal
    public void UpdateSpeed(Vec3 acceleration)
    {
        if (!acceleration.IsZero())
        {
            Vec3 newSpeed = Vec3.Zero();

            //Given the desired speed we steer towards it, do not go to that speed directly in order to give the sensation of steering
            this._speed.Multiply(STEER_FORCE);
            newSpeed.Add(this._speed);

            acceleration.Multiply(1 - STEER_FORCE);
            newSpeed.Add(acceleration);

            newSpeed.Trim(MaxSquaredSpeed);
            this._speed = newSpeed;
        }
    }//END UpdateSpeed

    //END: Constructor and main methods

    //SECTION: Secondary methods

    //If a foe animal is within his vision radius then it is in danger
    public Boolean InDanger(List<Animal> foes)
    {
        foreach(Animal a in foes)
        {
            if(this.SquareDistanceTo(a) < this._squaredVisionRadius)
            {
                return true;
            }
        }
        return false;
    }//InDanger


    //Returns the squaredDistance to another animal
    public double SquareDistanceTo(Animal other)
    {
        return this._position.SquaredDistanceTo(other.Position);
    }


    //Resets the position and is not safe now
    public void ResetPosition(Random rand)
    {
        this._isSafe = false;
        this._position.RandomizeCoords(rand);
    }
    //END: Secondary methods
}
