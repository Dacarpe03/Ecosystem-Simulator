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
        this._fixedPreyId = -1;
        double maxScore = 0;
        foreach (Animal a in preys.Values)
        {
            int counter = 0;
            if (!a.IsDead)
            {
                counter = this.PreyScore(a);
                if (counter > maxScore)
                {
                        this.FixedPreyId = a.Id;
                    maxScore = counter;
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

    public int PreyScore(Animal a)
    {
        int count = 0;
        foreach (Animal p in this._predators.Values)
        {
            if (p.SquaredVisionRadius >= p.SquareDistanceTo(a))
            {
                count += 1;
            }
        }
        return count;
    }

    public void Eat()
    {

    }

    public void Reset()
    {
        //this._predators = this._predators.Where(a => !a.Value.IsDead & a.Value.IsSafe).Select(a => a).ToDictionary(a => a.Key, a => a.Value);
        this._fixedPreyId = -1;
    }
}
