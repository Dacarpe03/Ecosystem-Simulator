using System.Collections;
using System.Collections.Generic;

public abstract class AnimalBuilder
{
    protected GroupParameters _animalParameters;


    public AnimalBuilder(GroupParameters parameters)
    {
        this._animalParameters = parameters;
    }

    public abstract Animal CreateAnimal();
    public abstract AnimalState GetAnimalState();
}
