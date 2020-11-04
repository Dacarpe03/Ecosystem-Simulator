using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalGroup
{
    //SECTION: Attributes and properties
    private const double REPRODUCTIONPROB = 1;
    public int Size { get => this.Animals.Count; }

    private Boolean _arePrey;

    private double _maxSpeed;
    private double _visionRadius;

    private List<Animal> _animals;
    public List<Animal> Animals { get => _animals;}
    //END: Attributes and properties


    //SECTION: Constructor and main methods

    public AnimalGroup(int size, double maxSpeed, double visionRadius, Boolean prey)
    {
        this._arePrey = prey;
        this._maxSpeed = maxSpeed;
        this._visionRadius = visionRadius;
        this._animals = new List<Animal>();

        System.Random rand = new System.Random();
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
        List<Animal> aliveAllies = this._animals.Where(a => !a.IsDead | !a.IsSafe).Select(a => a).ToList();
        List<Animal> aliveFoes = this._animals.Where(a => !a.IsDead | !a.IsSafe).Select(a => a).ToList();
        foreach (Animal a in this._animals)
        {
            a.State.Update(aliveAllies, foes);
        }
    }

    public void Evolve()
    {
        List<Animal> survivors = this._animals.Where(a => !a.IsDead & a.IsSafe).Select(a => a).ToList();
        Debug.Log("MODELO--Tamaño grupo supervivientes: " + survivors.Count);

        int possibleBreedingCount = survivors.Count / 2;
        var rand = new System.Random();
        for (int i = 0; i < possibleBreedingCount; i++)
        {
            double r = rand.NextDouble();
            if (r <= REPRODUCTIONPROB)
            {
                Animal a = new Animal(new AnimalStillState(), this._maxSpeed, this._visionRadius, i, rand);
                survivors.Add(a);
            }
        }

        this._animals = survivors;
        Debug.Log("Tamaño grupo después de reproducción: " + this.Size);
        this.ResetSafe();
        this.ResetPositions();
    }

    public void ResetSafe()
    {
        foreach(Animal a in this._animals)
        {
            a.IsSafe = false;
        }
    }

    public void ResetPositions()
    {
        System.Random rand = new System.Random();
        foreach(Animal a in this._animals)
        {
            AnimalState initialState = new AnimalHuntState();
            if (this._arePrey)
            {
                initialState = new AnimalFleeState();
            }
            a.TransitionTo(initialState);
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
