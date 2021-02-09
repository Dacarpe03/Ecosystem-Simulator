using System;
using System.Collections.Generic;

public class Ecosystem
{
    //SECTION: Attributes and properties

    private SimulationState _state;
    public SimulationState State { get => _state; set => _state = value; }
    
    private int _iteration;
    public int Iteration { get => _iteration; set => _iteration = value; }

    private AnimalGroup _preys;
    public AnimalGroup Preys { get => _preys; }

    private AnimalGroup _predators;
    public AnimalGroup Predators { get => _predators; }

    public Boolean _reset = false;
    public Boolean Reset { get => _reset; set => _reset = value; }

    public Boolean Extinguised { get => this.Predators.Size <= 1; }
    //END: Attributes and properties


    //SECTION: Constructor and main methods
    public Ecosystem(int preyGroupSize, double preyMaxSpeed, double preyVisionRadius, double preyReproductionProb, int predatorGroupSize, double predatorMaxSpeed, double predatorVisionRadius, double predatorReproductionProb)
    {
        this.TransitionTo(new SimulationSurviveState());
        this._iteration = 0;
        this._preys = new AnimalGroup(preyGroupSize, preyMaxSpeed, preyVisionRadius, preyReproductionProb, true);
        this._predators = new AnimalGroup(predatorGroupSize, predatorMaxSpeed, predatorVisionRadius, predatorReproductionProb, false);
    }

    public void Update()
    {
        this._state.Update();
    }

    //To change between states
    public void TransitionTo(SimulationState newState)
    {
        this._state = newState;
        this._state.Eco = this;
    }
    //END: Constructor and main methods


    //SECTION: Secondary Methods

    //TODO: Refactor this method into SimulationEvolveState
    public void Evolve() {
        this._preys.Evolve();
        this._predators.Evolve();
    }

    //TODO: Refactor this method into SimultaionSurviveState
    public void Survive()
    {
        this._preys.Survive(this._predators.Animals);
        this._predators.Survive(this._preys.Animals);
    }
    //END: Secondary Methods


    //Returns a list with the positions from the preys
    public List<Vec3> GetPreyPositions()
    {
        return this._preys.GetPositions();
    }//END GetPreyPositions

    //Returns a list with the positions from the predators
    public List<Vec3> GetPredatorPositions()
    {
        return this._predators.GetPositions();
    }//END GetPredatorPositions
}
