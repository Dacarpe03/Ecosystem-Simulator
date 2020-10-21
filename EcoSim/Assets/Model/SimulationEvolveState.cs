using System;

public class SimulationEvolveState : SimulationState
{
    public override void Update()
    {
        Eco.Preys.Evolve();
        Eco.TransitionTo(new SimulationSurviveState());
    }
}
