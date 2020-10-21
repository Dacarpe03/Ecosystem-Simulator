﻿using System;

public class SimulationEvolveState : SimulationState
{
    //SECTION: Constructor and main methods
    public override void Update()
    {
        _eco.Iteration++;
        _eco..Evolve();
        _eco.TransitionTo(new SimulationSurviveState());
    }
    //END: Constructor and main methods
}
