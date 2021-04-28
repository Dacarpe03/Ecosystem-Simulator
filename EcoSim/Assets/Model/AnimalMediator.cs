using System.Collections.Generic;
using System;
using System.Linq;

public class AnimalMediator
{
    private int _fixedPreyId = -1;
    public int FixedPreyId { get => this._fixedPreyId; set => this._fixedPreyId = value; }

    private Dictionary<int, Animal> _predators = new Dictionary<int, Animal>();
    public Dictionary<int, Animal> Predators { get => this._predators; }

    public void AddAnimal(Animal a)
    {
        this._predators.Add(a.Id, a);
    }

    public void UpdateBestPreyId(Dictionary<int, Animal> preys)
    {
        double minSpeed = double.MaxValue;
        foreach (Animal a in preys.Values)
        {
            if (a.Speed.SquaredModule < minSpeed)
            {
                if (this.IsVisible(a))
                {
                    this.FixedPreyId = a.Id;
                    minSpeed = a.Speed.SquaredModule;
                }
            }
        }
    }

    public Boolean IsVisible(Animal a)
    {
        Boolean isVisible = false;

        foreach(Animal p in this._predators.Values)
        {
            if(p.SquaredVisionRadius >= p.SquareDistanceTo(a))
            {
                return true;
            }
        }

        return isVisible;

    }

    public void Eat()
    {

    }

    public void Reset()
    {
        this._predators = this._predators.Where(a => !a.Value.IsDead & a.Value.IsSafe).Select(a => a).ToDictionary(a => a.Key, a => a.Value);
    }
}
