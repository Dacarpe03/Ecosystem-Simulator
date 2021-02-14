using System;
using System.Collections.Generic;

public abstract class AnimalState
{
    //SECTION: Attributes and properties
    protected Animal _agent;

    public Animal Agent { get => _agent; set => _agent = value; }

    //END: Attributes and properties


    //SECTION: Constructor and main methods
    public abstract void Update(Dictionary<int, Animal> friendly, Dictionary<int, Animal> foes);

    protected List<Animal> GetNearbyAnimals(Dictionary<int, Animal> animals, double squareRadius)
    {
        List<Animal> nearbyAnimals = new List<Animal>();
        foreach(Animal a in animals.Values)
        {
            if(a.Id != this._agent.Id & this._agent.SquareDistanceTo(a) <= squareRadius){
                nearbyAnimals.Add(a);
            }
        }

        return nearbyAnimals;
    }
    //END: Constructor and main methods
}
