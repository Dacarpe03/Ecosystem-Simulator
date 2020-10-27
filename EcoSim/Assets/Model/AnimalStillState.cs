using System;
using System.Collections.Generic;

public class AnimalStillState : AnimalState
{
    //SECTION: Constructor and main methods

    //END: Constructor and main methods
    public override void Update(List<Animal> friendly, List<Animal> foes)
    {
        Console.WriteLine("Estoy quieto");
        if (this._agent.InDanger(foes))
        {
            this._agent.TransitionTo(new AnimalFleeState());
        }
    }
}
