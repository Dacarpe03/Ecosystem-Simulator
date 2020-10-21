using System;

public class SimulationEvolveState : SimulationState
{
    //SECTION: Constructor and main methods
    public override void Update()
    {
        Eco.Iteration++;
        Eco.Preys.Evolve();
        Eco.Predators.Evolve();
        Eco.TransitionTo(new SimulationSurviveState());
    }
    //END: Constructor and main methods
}
