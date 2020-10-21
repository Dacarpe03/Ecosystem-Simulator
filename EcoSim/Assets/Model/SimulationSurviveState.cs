using System;

public class SimulationSurviveState : SimulationState
{
    public SimulationSurviveState(Ecosystem eco)
    {
        Eco = eco;
    }

    public override void Update()
    {
        if (Eco.Preys.AreSafe())
        {
            Eco.TransitionState(new SimulationEvolveState());
        }
        else
        {
            Eco.Preys.Update();
            Eco.Predators.Update();
        }
    }
}
