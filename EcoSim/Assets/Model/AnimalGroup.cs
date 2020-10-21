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

    public AnimalGroup(int size, float maxSpeed)
    {
        this._size = size;

        for (int i = 0; i < size; i++)
        {
            Animal a = new Animal(new AnimalStillState(), maxSpeed, i);
            this._animals.Add(a);
        }
    }

    public void Survive()
    {
        foreach(Animal a in this._animals)
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
                this._animals.Add(a);
                this._size += 1;
            }
        }

        this.ResetPositions();
    }

    public void ResetPositions()
    {
        foreach(Animal a in this._animals)
        {
            a.TransitionTo(new AnimalStillState);
            a.ResetPosition();
        }
    }

    public List<Vec3> GetPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        foreach(Animal a in _animals)
        {
            positions.Add(a.Position);
        }
    }

    public Boolean AreSafe()
    {
        foreach(Animal a in this._animals)
        {
            if (!a.isSafe())
            {
                return false;
            }
        }

        return true;
    }
    //END: Constructor and main methods
}
