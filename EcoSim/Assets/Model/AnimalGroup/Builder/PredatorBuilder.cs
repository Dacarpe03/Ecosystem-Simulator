using System.Collections;
using System.Collections.Generic;
using System;

public class PredatorBuilder : AnimalBuilder
{
    public PredatorBuilder(GroupParameters parameters) : base(parameters) {
    }


    //Method to create a prey with Flee State as initial state
    public override Animal CreateAnimal(Random rand, AnimalMediator mediator)
    {
        //AnimalState initialState = new AnimalHuntState(new SimpleStrategy());
        AnimalState initialState = new AnimalHuntState(new GWOStrategy());
        Animal a = new Animal(initialState, this._animalParameters.MaxSpeed, this._animalParameters.VisionRadius, this._creationCounter, rand, mediator);
        this._creationCounter += 1;

        return a;
    }

    public override AnimalState GetAnimalState()
    {
        //return new AnimalHuntState(new SimpleStrategy());
        return new AnimalHuntState(new GWOStrategy());
    }
}
