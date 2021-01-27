using System;

public class SimulationEvolveState : SimulationState
{
    //SECTION: Constructor and main methods
    public override void Update()
    {
        //Increment the number of the iteration
        this._eco.Iteration++;
        //Create the next generation of animals
        this._eco.Evolve();

        //End the 'reset condition' so that in the controller main loop we dont update the view again
        this._eco.Reset = false;
        //Transition to the Survive State
        this._eco.TransitionTo(new SimulationSurviveState());
    }
    //END: Constructor and main methods
}
