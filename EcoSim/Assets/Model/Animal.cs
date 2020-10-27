using System;
using System.Collections.Generic;

public class Animal
{
    //SECTION: Attributes and properties
    private const double STEER_FORCE = 0.3;

    private int _id;
    public int Id { get => _id; }

    private AnimalState _state;
    public AnimalState State { get => _state; set => _state = value; }

    private double _maxSpeed;
    private double _maxSquaredSpeed; //So that the computation of the norm of the vector skips one step, the sqrt
    public double MaxSquaredSpeed { get => _maxSquaredSpeed; }
    
    private double _visionRadius;
    private double _squaredVisionRadius;
    public double SquaredVisionRadius { get => _squaredVisionRadius; }

    private Boolean _isSafe;
    public Boolean IsSafe { get => _isSafe; set => _isSafe = value; }

    private Vec3 _position;
    public Vec3 Position { get => _position; set => _position = value; }

    private Vec3 _speed;
    public Vec3 Speed { get => _speed; set => _speed = value; }

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
        this._speed = Vec3.Zero();

        this._id = id;
    }

    public void TransitionTo(AnimalState newState)
    {
        this._state = newState;
        this._state.Agent = this;
    }

    public void Move()
    {
        this._position.Add(Speed);
    }

    public void UpdateSpeed(Vec3 acceleration)
    {
        Vec3 newSpeed = Vec3.Zero();

        this._speed.Multiply(STEER_FORCE);
        newSpeed.Add(this._speed);

        acceleration.Multiply(1 - STEER_FORCE);
        newSpeed.Add(acceleration);

        newSpeed.Trim(MaxSquaredSpeed);
        this._speed = newSpeed;
    }

    //END: Constructor and main methods

    //SECTION: Secondary methods
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
    }

    public double SquareDistanceTo(Animal other)
    {
        return this._position.SquaredDistanceTo(other.Position);
    }

    public void ResetPosition(Random rand)
    {
        this._position.RandomizeCoords(rand);
    }
    //END: Secondary methods
}
