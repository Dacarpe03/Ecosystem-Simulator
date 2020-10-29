using System;
using System.Collections.Generic;

public class Ecosystem
{
    //SECTION: Attributes and properties

    private const double PREY_MAX_SPEED = 0.5;
    private const double PREDATOR_MAX_SPEED = 0.7;

    private const double PREY_VISION_RADIUS = 10;
    private const double PREDATOR_VISION_RADIUS = 10;

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
    public Ecosystem(int preyGroupSize, int predatorGroupSize)
    {
        this.TransitionTo(new SimulationSurviveState());
        this._iteration = 0;
        this._preys = new AnimalGroup(preyGroupSize, PREY_MAX_SPEED, PREY_VISION_RADIUS, true);
        this._predators = new AnimalGroup(predatorGroupSize, PREDATOR_MAX_SPEED, PREDATOR_VISION_RADIUS, false);
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


    //SECTION: Secondary Methods
    public void Evolve() {
        this._preys.Evolve();
        this._predators.Evolve();
    }

    public void Survive()
    {
        this._preys.Survive(this._predators.Animals);
        this._predators.Survive(this._preys.Animals);
    }
    //END: Secondary Methods

    public List<Vec3> GetPreyPositions()
    {
        return this._preys.GetPositions();
    }

    public List<Vec3> GetPredatorPositions()
    {
        return this._preys.GetPositions();
    }
}
