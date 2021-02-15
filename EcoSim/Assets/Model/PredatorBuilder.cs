﻿using System.Collections;
using System.Collections.Generic;
using System;

public class PredatorBuilder : AnimalBuilder
{
    public PredatorBuilder(GroupParameters parameters) : base(parameters) {
        this._mediator = new AnimalMediator();
    }


    //Method to create a prey with Flee State as initial state
    public override Animal CreateAnimal(Random rand)
    {
        AnimalState initialState = new AnimalHuntState(new SimpleStrategy());
        Animal a = new Animal(initialState, this._animalParameters.MaxSpeed, this._animalParameters.VisionRadius, this._creationCounter, rand);
        a.AnimalMediator = this._mediator;
        this._mediator.AddAnimal(a);
        this._creationCounter += 1;

        return a;
    }

    public override AnimalState GetAnimalState()
    {

        return new AnimalHuntState(new SimpleStrategy());
    }

    private void ResetMediator()
    {
        this._mediator.Reset();
    }
}
