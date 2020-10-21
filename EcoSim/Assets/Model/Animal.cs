using System;


public class Animal
{
    //SECTION: Attributes and properties
    private AnimalState _state;
    public AnimalState State { get => _state; set => _state = value; }
    //END: Attributes and properties

    //SECTION: Constructor and main methods
    public Animal(AnimalState state)
    {
        this.TransitionTo(state);
    }

    public void TransitionTo(AnimalState newState)
    {
        this._state = newState;
        this._state.Eco = this;
    }
    //END: Constructor and main methods
}
