using System;

public class SimulationSurviveState : SimulationState
{
    //SECTION: Constructor and main methods
    public override void Update()
    {
        //Is all the alive preys are safe, then the iteration is about to finish
        if (_eco.Preys.AreSafe())
        {
            //Activate the 'reset condition' so that the view resets in the thex update call in Controller
            this._eco.Reset = true;
            this._eco.TransitionTo(new SimulationEvolveState());
        }
        else
        {
            this._eco.Preys.Survive(this._eco.Predators.Animals);
            this._eco.Predators.Survive(this._eco.Preys.Animals);
        }
    }
    //END: Constructor and main methods
}
