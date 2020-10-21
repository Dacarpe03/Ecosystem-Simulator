using System;
using System.Collections.Generic;

public class AnimalGroup
{
    //SECTION: Attributes and properties
    private const double REPRODUCTIONPROB = 0.1;

    private int _size;

    private List<Animal> _animals;
    //END: Attributes and properties


    //SECTION: Constructor and main methods
    public void Survive()
    {
        for (Animal a in this._animals)
        {
            a.Update(this._animals);
        }
    }

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

        this.ResetPositions();
    }

    public void ResetPositions()
    {
        for(Animal a in this._animals)
        {
            a.resetPosition();
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

    public Boolean AreSafe()
    {

    }
    //END: Constructor and main methods
}
