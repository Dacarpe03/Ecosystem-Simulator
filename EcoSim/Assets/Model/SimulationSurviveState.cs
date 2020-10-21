using System;

public class SimulationSurviveState : SimulationState
{
    //SECTION: Constructor and main methods
    public override void Update()
    {
        if (Eco.Preys.AreSafe())
        {
            Eco.TransitionTo(new SimulationEvolveState());
        }
        else
        {
            Eco.Preys.Update();
            Eco.Predators.Update();
        }
    }
    //END: Constructor and main methods
}
