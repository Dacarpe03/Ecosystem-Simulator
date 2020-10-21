using System;

public abstract class SimulationState
{
    protected Ecosystem _eco;

    public Ecosystem Eco { get => _eco;  set => _eco = value; }

    public abstract void Update();
}
