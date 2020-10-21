using System;
using System.Collections.Generic;

public abstract class AnimalGroup
{
    //SECTION: Attributes and properties
    private int _size;

    private List<Animal> _animals;
    private List<Animal> Animals { get => _animals; }
    //END: Attributes and properties

    //SECTION: Constructor and main methods
    public abstract void Survive();
    public abstract void Evolve();

    public List<Vector3> GetPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        foreach(Animal a in _animals)
        {
            positions.Add(a.Position);
        }
    }
    //END: Constructor and main methods
}
