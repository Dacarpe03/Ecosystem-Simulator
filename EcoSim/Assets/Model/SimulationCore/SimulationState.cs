using System;

public abstract class SimulationState
{
    //SECTION: Attributes and properties
    protected Ecosystem _eco;

    public Ecosystem Eco { get => _eco;  set => _eco = value; }

    //END: Attributes and properties


    //SECTION: Constructor and main methods
    public abstract void Update();
    //END: Constructor and main methods
}
