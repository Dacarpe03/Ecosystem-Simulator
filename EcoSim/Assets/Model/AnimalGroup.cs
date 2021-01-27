using System;
using System.Collections.Generic;
using System.Linq;
//using UnityEngine;

public class AnimalGroup
{
    //SECTION: Attributes and properties
    private  double _reproductionProb = 1;
    public int Size { get => this.Animals.Count; }
    public int SurvivorsNumber { get => this._animals.Where(a => !a.IsDead & a.IsSafe).Select(a => a).ToList().Count; }

    private Boolean _arePrey;

    private double _maxSpeed;
    private double _visionRadius;

    private List<Animal> _animals;
    public List<Animal> Animals { get => _animals;}
    //END: Attributes and properties


    //SECTION: Constructor and main methods
    //TODO: Use Builder pattern to create the different animal groups so delete de booolean prey parameter in builder
    public AnimalGroup(int size, double maxSpeed, double visionRadius, double reproductionProb, Boolean prey)
    {
        this._arePrey = prey;
        this._maxSpeed = maxSpeed;
        this._visionRadius = visionRadius;
        this._reproductionProb = reproductionProb;
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


    //Method to update the position of animals in the iteration
    public void Survive(List<Animal> foes)
    {
        //Get the animals from the same group that are not dead and are not safe
        List<Animal> aliveAllies = this._animals.Where(a => !a.IsDead | !a.IsSafe).Select(a => a).ToList();

        //Get the animals from the other group that are not dead and are not safe
        List<Animal> aliveFoes = foes.Where(a => !a.IsDead | !a.IsSafe).Select(a => a).ToList();
        foreach (Animal a in this._animals)
        {
            a.State.Update(aliveAllies, foes);
        }
    }//End Survive


    //Method to create the next generation of animals
    public void Evolve()
    {
        List<Animal> survivors = this._animals.Where(a => !a.IsDead & a.IsSafe).Select(a => a).ToList();
        //Debug.Log("MODELO--Tamaño grupo supervivientes: " + survivors.Count);

        //Calculate the maximum breeding count
        int possibleBreedingCount = survivors.Count / 2;
        var rand = new System.Random();

        //Now each pair tries to reproduce
        for (int i = 0; i < possibleBreedingCount; i++)
        {
            double r = rand.NextDouble();
            if (r <= _reproductionProb)
            {
                Animal a = new Animal(new AnimalStillState(), this._maxSpeed, this._visionRadius, i, rand);
                survivors.Add(a);
            }
        }

        this._animals = survivors;
        //Debug.Log("Tamaño grupo después de reproducción: " + this.Size);
        //Reset the animals in the model
        this.ResetSafe();
        this.ResetPositions();
    }//END Evolve


    //Reset the safe attribute to False
    public void ResetSafe()
    {
        foreach(Animal a in this._animals)
        {
            a.IsSafe = false;
        }
    }//END ResetSafe


    //Reset Positions
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

            //Reset to the initial state
            a.TransitionTo(initialState);
            //Reset the position in the initial square
            a.ResetPosition(rand);
        }
    }

    //Returns the positions of all its animals
    public List<Vec3> GetPositions()
    {
        List<Vec3> positions = new List<Vec3>();
        foreach(Animal a in _animals)
        {
            positions.Add(a.Position);
        }

        return positions;
    }//END GetPositions


    //Return true if all the animals are safe
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
