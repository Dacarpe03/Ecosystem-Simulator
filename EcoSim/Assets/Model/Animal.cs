using System;
using System.Numerics;

public class Animal
{
    //SECTION: Attributes and properties
    private AnimalState _state;
    public AnimalState State { get => _state; set => _state = value; }

    private float _maxSpeed;
    private float _maxSquareSpeed; //So that the computation of the norm of the vector skips one step, the sqrt

    private Vec3 _position;
    public Vec3 Position { get => _position; set => _position = value }

    private Vec3 _speed;
    public Vec3 Speed { get => _speed; set => _speed = value }

    //END: Attributes and properties

    //SECTION: Constructor and main methods
    public Animal(AnimalState state, float maxSpeed)
    {
        this.TransitionTo(state);
        this._maxSpeed = maxSpeed;
        this._maxSquareSpeed = maxSpeed * maxSpeed;
    }

    public void TransitionTo(AnimalState newState)
    {
        this._state = newState;
        this._state.Eco = this;
    }
    //END: Constructor and main methods
}
