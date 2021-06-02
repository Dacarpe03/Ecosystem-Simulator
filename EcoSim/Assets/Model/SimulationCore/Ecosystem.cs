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

    public Boolean Extinguised { get => this.Predators.Size <= 1 || this.Preys.Size <= 1; }
    //END: Attributes and properties


    //SECTION: Constructor and main methods
    public Ecosystem(GroupParameters preyParameters, GroupParameters predatorParameters, Resource plants)
    {
        this.TransitionTo(new SimulationSurviveState());
        this._iteration = 0;

        AnimalMediator preyMediator = new AnimalMediator();
        AnimalBuilder preyBuilder = new PreyBuilder(preyParameters);
        this._preys = new AnimalGroup(preyParameters, preyBuilder, preyMediator);

        Resource meat = new Resource(0, 0)
        AnimalMediator predatorMediator = new AnimalMediator();
        AnimalBuilder predatorBuilder = new PredatorBuilder(predatorParameters);
        this._predators = new AnimalGroup(predatorParameters, predatorBuilder, predatorMediator);
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
