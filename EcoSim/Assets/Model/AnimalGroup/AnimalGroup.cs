using System;
using System.Collections.Generic;
using System.Linq;
//using UnityEngine;



public class AnimalGroup
{
    //SECTION: Attributes and properties
    public int Size { get => this.Animals.Count; }
    public int SurvivorsNumber { get => this._animals.Where(a => !a.Value.IsDead).Select(a => a).ToList().Count; }
    public double CurrentFood { get => this._mediator.CurrentFood(); }

    private double _reproductionProb;

    private AnimalBuilder _animalBuilder;
    private AnimalMediator _mediator;

    private Dictionary<int, Animal> _animals;
    public Dictionary<int, Animal> Animals { get => _animals;}
    //END: Attributes and properties


    //SECTION: Constructor and main methods
    public AnimalGroup(GroupParameters groupParameters, AnimalBuilder builder, AnimalMediator mediator)
    {
        this._reproductionProb = groupParameters.ReproductionProb;

        this._animalBuilder = builder;

        this._animals = new Dictionary<int, Animal>();
        this._mediator = mediator;
        System.Random rand = new System.Random();
        for (int i = 0; i < groupParameters.GroupSize; i++)
        {
            Animal a = builder.CreateAnimal(rand, mediator);
            this._animals.Add(a.Id, a);
        }
    }


    //Method to update the position of animals in the iteration
    public void Survive(Dictionary<int, Animal> foes)
    {
        //Get the animals from the same group that are not dead and are not safe
        Dictionary<int, Animal> aliveAllies = this._animals.Where(a => !a.Value.IsDead & !a.Value.IsSafe).Select(a => a).ToDictionary(a => a.Key, a => a.Value);

        //Get the animals from the other group that are not dead and are not safe
        Dictionary<int, Animal> aliveFoes = foes.Where(a => !a.Value.IsDead & !a.Value.IsSafe).Select(a => a).ToDictionary(a => a.Key, a => a.Value);
        foreach (Animal a in this._animals.Values)
        {
            a.State.Update(aliveAllies, aliveFoes);
        }
    }//End Survive


    //Method to create the next generation of animals
    public void Evolve()
    {
        //First let the animal eat
        this.Eat();

        //Now evolve
        Dictionary<int, Animal> survivors = this._animals.Where(a => !a.Value.IsDead).Select(a => a).ToDictionary(a => a.Key, a=> a.Value);
        //Debug.Log("MODELO--Tamaño grupo supervivientes: " + survivors.Count);
        
        //Calculate the maximum breeding count
        int possibleBreedingCount = survivors.Count / 2;
        var rand = new System.Random();

        //Now each pair tries to reproduce
        for (int i = 0; i < possibleBreedingCount; i++)
        {
            double r = rand.NextDouble();
            if (r <= this._reproductionProb)
            {
                Animal a = this._animalBuilder.CreateAnimal(rand, this._mediator);
                survivors.Add(a.Id, a);
            }
        }

        this._animals = survivors;
        //Debug.Log("Tamaño grupo después de reproducción: " + this.Size);
        //Reset the animals in the model
        this.ResetSafe();
        this.ResetPositions();
        this.ResetMediator();
    }//END Evolve


    //Function so that each animal eats if it is not dead
    private void Eat()
    {
        //Create a set with all the keys
        List<int> keys = this.Animals.Keys.ToList();

        //Rearrange in a random way
        ListShuffler.Shuffle(keys);
        
        //For each animal try to eat
        foreach(int n in keys)
        {
            this.Animals[n].Eat();
        }
    }//END Eat


    //Reset the safe attribute to False
    public void ResetSafe()
    {
        foreach(Animal a in this._animals.Values)
        {
            a.IsSafe = false;
        }
    }//END ResetSafe


    //Reset Positions
    public void ResetPositions()
    {
        System.Random rand = new System.Random();
        foreach(Animal a in this._animals.Values)
        {
            AnimalState initialState = this._animalBuilder.GetAnimalState();
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
        foreach(Animal a in this._animals.Values)
        {
            positions.Add(a.Position);
        }

        return positions;
    }//END GetPositions


    //Return true if all the animals are safe
    public Boolean AreSafe()
    {
        foreach(Animal a in this._animals.Values)
        {
            if (!a.IsSafe)
            {
                return false;
            }
        }

        return true;
    }
    //END: Constructor and main methods

    public void ResetMediator()
    {
        this._mediator.Reset();
    }
}

static class ListShuffler
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            Random r = new Random();
            n--;
            int k = r.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}