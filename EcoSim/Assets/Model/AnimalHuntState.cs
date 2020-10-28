﻿using System;
using System.Collections.Generic;
using System.Linq;

public class AnimalHuntState : AnimalState
{
    private Boolean preyFixed = false;
    private int fixedPreyId = -1;

    public override void Update(List<Animal> friendly, List<Animal> foes)
    {
        if (!preyFixed) 
        {
            List<Animal> nearbyPreys = this.GetNearbyAnimals(foes, this._agent.SquaredVisionRadius);
            fixedPreyId = this.GetSlowestAnimal(nearbyPreys);
            preyFixed = true; 
        }

        Vec3 fixedPreyPosition  = (Vec3) foes.Where(a => a.Id == fixedPreyId).Select(a => a.Position).ToList().First();

        Vec3 acceleration = Vec3.CalculateVectorsBetweenPoints(this._agent.Position, fixedPreyPosition);
        acceleration.Expand(this._agent.MaxSpeed);

        this._agent.UpdateSpeed(acceleration);
        this._agent.Move();
    }
    
    private int GetSlowestAnimal(List<Animal> nearbyPreys)
    {
        double maxSpeed = Double.MaxValue;
        int idFixed = -1;
        foreach(Animal p in nearbyPreys)
        {
            double preySpeed = p.Speed.SquaredModule;
           if(preySpeed < maxSpeed)
            {
                maxSpeed = preySpeed;
                idFixed = p.Id;
            }
        }

        return idFixed;
    }
}
