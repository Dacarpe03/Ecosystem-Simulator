using System;

public class SimulationEvolveState : SimulationState
{
    //SECTION: Constructor and main methods
    public override void Update()
    {
        this._eco.Iteration++;
        this._eco.Evolve();
        this._eco.Reset = false;
        this._eco.TransitionTo(new SimulationSurviveState());
    }
    //END: Constructor and main methods
}
