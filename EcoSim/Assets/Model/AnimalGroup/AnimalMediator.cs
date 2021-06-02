using System.Collections.Generic;
using System;
using System.Linq;

public class AnimalMediator
{
    private Resource _resource;
    private int _fixedPreyId = -1;
    public int FixedPreyId { get => this._fixedPreyId; set => this._fixedPreyId = value; }
    
    public AnimalMediator(Resource resource)
    {
        this._resource = resource;
    }

    public void UpdateBestPreyId(Dictionary<int, Animal> predators, Dictionary<int, Animal> preys)
    {
        this._fixedPreyId = -1;
        double maxScore = 0;
        foreach (Animal a in preys.Values)
        {
            int counter = 0;
            if (!a.IsDead)
            {
                counter = this.PreyScore(a, predators);
                if (counter > maxScore)
                {
                        this.FixedPreyId = a.Id;
                    maxScore = counter;
                }
            }
        }
    }

    public Boolean IsVisible(Animal prey, Dictionary<int, Animal> predators)
    {
        Boolean isVisible = false;

        foreach(Animal p in predators.Values)
        {
            if(p.SquaredVisionRadius >= p.SquareDistanceTo(prey))
            {
                return true;
            }
        }

        return isVisible;

    }

    public int PreyScore(Animal prey, Dictionary<int, Animal> predators)
    {
        int count = 0;
        foreach (Animal p in predators.Values)
        {
            if (p.SquaredVisionRadius >= p.SquareDistanceTo(prey))
            {
                count += 1;
            }
        }
        return count;
    }

    public void Reset()
    {
        //this._predators = this._predators.Where(a => !a.Value.IsDead & a.Value.IsSafe).Select(a => a).ToDictionary(a => a.Key, a => a.Value);
        this._fixedPreyId = -1;
        this._resource.Grow();
    }

    public void PreyHunted(Animal a)
    {
        this._resource.addUnits(4);
    }
}
