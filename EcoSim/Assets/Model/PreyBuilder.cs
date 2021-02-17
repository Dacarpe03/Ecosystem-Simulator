using System.Collections;
using System.Collections.Generic;
using System;

public class PreyBuilder : AnimalBuilder
{

    public PreyBuilder(GroupParameters parameters): base(parameters){}


    //Method to create a prey with Flee State as initial state
    public override Animal CreateAnimal(Random rand)
    {
        AnimalState initialState = new AnimalFleeState();
        //AnimalState initialState = new AnimalStillState();
        Animal a = new Animal(initialState, this._animalParameters.MaxSpeed, this._animalParameters.VisionRadius, this._creationCounter, rand);

        //Animal a = new Animal(initialState, this._animalParameters.MaxSpeed, this._animalParameters.VisionRadius, this._creationCounter);
        this._creationCounter += 1;

        return a;
    }

    public override AnimalState GetAnimalState()
    {
        return new AnimalFleeState();
        //return new AnimalStillState();
    }

    public override void ResetMediator()
    {

    }
}
