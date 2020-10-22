using System;
using System.Collections.Generic;

public class AnimalGroup
{
    //SECTION: Attributes and properties
    private const double REPRODUCTIONPROB = 0.1;

    private int _size;
    private float _maxSpeed;

    private List<Animal> _animals;
    public List<Animal> Animals { get => _animals;}
    //END: Attributes and properties


    //SECTION: Constructor and main methods

    public AnimalGroup(int size, float maxSpeed)
    {
        this._size = size;
        this._maxSpeed = maxSpeed;
        this._animals = new List<Animal>();

        Random rand = new Random();
        for (int i = 0; i < size; i++)
        {
            Animal a = new Animal(new AnimalStillState(), maxSpeed, i, rand);
            this._animals.Add(a);
        }
    }

    public void Survive(List<Animal> foes)
    {
        foreach(Animal a in this._animals)
        {
            a.State.Update(this._animals, foes);
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
                Animal a = new Animal(new AnimalStillState(), this._maxSpeed, i, rand);
                this._animals.Add(a);
                this._size += 1;
            }
        }

        this.ResetPositions();
    }

    public void ResetPositions()
    {
        Random rand = new Random();
        foreach(Animal a in this._animals)
        {
            a.TransitionTo(new AnimalStillState());
            a.ResetPosition(rand);
        }
    }

    public List<Vec3> GetPositions()
    {
        List<Vec3> positions = new List<Vec3>();
        foreach(Animal a in _animals)
        {
            positions.Add(a.Position);
        }

        return positions;
    }

    public Boolean AreSafe()
    {
        foreach(Animal a in this._animals)
        {
            if (!a.IsSafe)
            {
                return false;
            }
        }

        return true;
    }
    //END: Constructor and main methods
}
