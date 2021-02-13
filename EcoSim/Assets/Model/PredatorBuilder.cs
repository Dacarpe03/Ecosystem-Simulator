using System.Collections;
using System.Collections.Generic;
using System;

public class PredatorBuilder : AnimalBuilder
{
    public PredatorBuilder(GroupParameters parameters) : base(parameters) { }


    //Method to create a prey with Flee State as initial state
    public override Animal CreateAnimal(Random rand)
    {
        AnimalState initialState = new AnimalHuntState();
        Animal a = new Animal(initialState, this._animalParameters.MaxSpeed, this._animalParameters.VisionRadius, this._creationCounter, rand);
        this._creationCounter += 1;

        return a;
    }

    public override AnimalState GetAnimalState()
    {
        return new AnimalHuntState();
    }
}
