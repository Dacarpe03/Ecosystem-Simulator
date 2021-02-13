using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimalBuilder
{
    protected GroupParameters animalParameters;

    public abstract Animal CreateAnimal();
    public abstract AnimalState GetAnimalState();
}
