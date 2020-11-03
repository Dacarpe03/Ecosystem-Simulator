using System;

public class SimulationSurviveState : SimulationState
{
    //SECTION: Constructor and main methods
    public override void Update()
    {
        if (_eco.Preys.AreSafe())
        {
            this._eco.Reset = true;
            this._eco.TransitionTo(new SimulationEvolveState());
        }
        else
        {
            this._eco.Survive();
        }
    }
    //END: Constructor and main methods
}
