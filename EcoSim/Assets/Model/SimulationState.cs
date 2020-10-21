using System;

public abstract class SimulationState
{
    protected Ecosystem _ecosystem;

    public Ecosystem Ecosystem { set => _ecosystem = value; }

    public abstract void Update();
}
