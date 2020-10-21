using System;

public class SimulationSurviveState : SimulationState
{
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
}
