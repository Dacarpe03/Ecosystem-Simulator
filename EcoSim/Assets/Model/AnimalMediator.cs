using System.Collections;
using System.Collections.Generic;

public class AnimalMediator
{
    private int _fixedPreyId = -1;
    public int FixedPreyId { get => _fixedPreyId; set => this._fixedPreyId = value; }

    private Dictionary<int, Animal> _predators;
    public Dictionary<int, Animal> Predators { get => this._predators; }

    public void AddAnimal(Animal a)
    {
        this._predators.Add(a.Id, a);
    }


}
