using System;

public class SimulationSurviveState : SimulationState
{
    //SECTION: Constructor and main methods
    public override void Update()
    {
        if (_eco.Preys.AreSafe())
        {
            _eco.TransitionTo(new SimulationEvolveState());
        }
        else
        {
            _eco.Preys.Survive();
            _eco.Predators.Survive();
        }
    }
    //END: Constructor and main methods
}
