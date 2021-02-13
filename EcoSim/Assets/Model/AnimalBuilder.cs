using System.Collections;
using System.Collections.Generic;
using System;

public abstract class AnimalBuilder
{
    protected GroupParameters _animalParameters;
    protected int _creationCounter;

    public AnimalBuilder(GroupParameters parameters)
    {
        this._animalParameters = parameters;
        this._creationCounter = 0;
    }

    public abstract Animal CreateAnimal(Random rand);
    public abstract AnimalState GetAnimalState();
}
