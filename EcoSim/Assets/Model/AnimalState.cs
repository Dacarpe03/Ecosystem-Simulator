using System;

public abstract class AnimalState
{
    //SECTION: Attributes and properties
    protected Animal _agent;

    public Animal Agent { get => _agent; set => _agent = value; }

    //END: Attributes and properties


    //SECTION: Constructor and main methods
    public abstract void Update();
    //END: Constructor and main methods
}
