using System;
using System.Text.RegularExpressions;

public class Ecosystem
{
    //SECTION: Attributes and properties
    private const int PREY_GROUP_SIZE = 40;
    private const int PREDATOR_GROUP_SIZE = 6;

    private SimulationState _state;
    public SimulationState State { get => _state; set => _state = value; }
    
    private int _iteration;
    public int Iteration { get => _iteration; set => _iteration = value; }

    private AnimalGroup _preys;
    public AnimalGroup Preys { get => _preys; }

    private AnimalGroup _predators;
    public AnimalGroup Predators { get => _predators; }
    //END: Attributes and properties


    //SECTION: Constructor and main methods
    public Ecosystem()
    {
        this.TransitionTo(new SimulationSurviveState());
        this._iteration = 0;
        this._preys = new PreyGroup(PREY_GROUP_SIZE);
        this._predators = new PredatorGroup(PREDATOR_GROUP_SIZE);
    }

    public void Update()
    {
        this._state.Update();
    }

    public void TransitionTo(SimulationState newState)
    {
        this._state = newState;
        this._state.Eco = this;
    }
    //END: Constructor and main methods
}
