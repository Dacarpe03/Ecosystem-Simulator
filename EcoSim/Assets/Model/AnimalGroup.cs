using System;
using System.Collections.Generic;

public abstract class AnimalGroup
{
    //SECTION: Attributes and properties
    protected const double REPRODUCTIONPROB = 0.1;

    protected int _size;

    protected List<Animal> _animals;
    protected List<Animal> Animals { get => _animals; }
    //END: Attributes and properties

    //SECTION: Constructor and main methods
    public abstract void Survive();
    public void Evolve()
    {
        int possibleBreedingCount = this._size / 2;
        var rand = new Random();
        for (int i = 0; i < possibleBreedingCount; i++)
        {
            double r = rand.NextDouble();
            if (r < REPRODUCTIONPROB)
            {
                Animal a = new Animal();
                this.animal
            }
        }

    }

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
