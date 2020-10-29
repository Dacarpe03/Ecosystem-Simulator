using System;
using System.Collections.Generic;
using System.Linq;

public class AnimalGroup
{
    //SECTION: Attributes and properties
    private const double REPRODUCTIONPROB = 0.1;

    private int _size;
    public int Size { get => _size; }

    private double _maxSpeed;
    private double _visionRadius;

    private List<Animal> _animals;
    public List<Animal> Animals { get => _animals;}
    //END: Attributes and properties


    //SECTION: Constructor and main methods

    public AnimalGroup(int size, double maxSpeed, double visionRadius, Boolean prey)
    {
        this._size = size;
        this._maxSpeed = maxSpeed;
        this._visionRadius = visionRadius;
        this._animals = new List<Animal>();

        Random rand = new Random();
        for (int i = 0; i < size; i++)
        {
            AnimalState initialState = new AnimalHuntState();
            if (prey)
            {
                initialState = new AnimalFleeState();
            }
            Animal a = new Animal(initialState, maxSpeed, visionRadius , i, rand); ;
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
        List<Animal> survivors = this._animals.Where(a => a.IsDead = false).Select(a => a).ToList();
        this._size = survivors.Count;

        int possibleBreedingCount = this._size / 2;
        var rand = new Random();
        for (int i = 0; i < possibleBreedingCount; i++)
        {
            double r = rand.NextDouble();
            if (r < REPRODUCTIONPROB)
            {
                Animal a = new Animal(new AnimalStillState(), this._maxSpeed, this._visionRadius, i, rand);
                survivors.Add(a);
                this._size += 1;
            }
        }

        this._animals = survivors;
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
