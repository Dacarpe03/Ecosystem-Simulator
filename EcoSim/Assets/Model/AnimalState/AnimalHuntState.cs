using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalHuntState : AnimalState
{
    private HuntingStrategy _strategy;

    private int _framesUpdate;

    private int _frameCounter = 0;

    private Vec3 _desiredPosition = new Vec3(0, 0, 0);
    private Boolean fixedPosition = false;


    public AnimalHuntState(HuntingStrategy strategy): base()
    {
        this._strategy = strategy;
        this._framesUpdate = strategy.FramesUpdate;
    }

    public override void Update(Dictionary<int, Animal> friendly, Dictionary<int, Animal> foes)
    {
        this._frameCounter += 1;
        //Now fix other prey or calculate new optimal position
        if (!this._strategy.HasFixedPrey(this._agent) || !foes.ContainsKey(this._strategy.GetFixedPreyId(this._agent)))
        {
            this._strategy.SelectPrey(friendly, foes, this._agent);
            this.fixedPosition = false;
        }
        else if (!this.fixedPosition || this._frameCounter > this._framesUpdate)
        {
            this._frameCounter = 0;
            //Get all the positions except the agent one
            List<Vec3> predatorPositions = new List<Vec3>();

            foreach (Animal a in friendly.Values)
            {
                if (this._agent.Id != a.Id)
                {
                    predatorPositions.Add(a.Position);
                }
            }

            //Get data from fixedPrey
            int fixedPreyId = this._strategy.GetFixedPreyId(this._agent);
            Debug.Log(fixedPreyId);
            Animal fixedPrey = foes[fixedPreyId];

            //Calculate optimal position of the predator agent
            this._desiredPosition = this._strategy.GetDesiredPosition(this._agent, predatorPositions, fixedPrey);

            //Update the predator Speed
            this.fixedPosition = true;
        }

        //Check if the agent is near the prey to hunt it
        this.CheckPreyInRangeOfAttack(this._agent, foes, friendly);

        //Move the agent
        Vec3 acceleration = Vec3.CalculateVectorsBetweenPoints(this._agent.Position, this._desiredPosition);
        acceleration.Add(this.Avoidance(this._agent, friendly));
        acceleration.Expand(this._agent.MaxSpeed);

        this._agent.UpdateSpeed(acceleration);
        this._agent.Move();
    }

    private void CheckPreyInRangeOfAttack(Animal agent, Dictionary<int, Animal> preys, Dictionary<int, Animal> predators)
    {
        int preyId = this._strategy.GetFixedPreyId(agent);
        if (preys.ContainsKey(preyId))
        {
            if (preyId != -1 && preys[preyId].SquareDistanceTo(agent) < 4 && !preys[preyId].IsDead)
            {
                preys[preyId].IsDead = true;
                preys[preyId].IsSafe = true;
                preys[preyId].TransitionTo(new AnimalStillState());

                //Kill prey
                this._strategy.HuntPrey(agent, preys[preyId]);

                //Select new prey
                this._strategy.SelectPrey(predators, preys,agent);
            }
        }
    }

    private Vec3 Avoidance(Animal agent, Dictionary<int, Animal> nearbyAnimals) //Avoid nearby animals creating a repelling force between them
    {
        Vec3 avoidanceVector = Vec3.Zero();
        List<Animal> closeAnimals = this.GetNearbyAnimals(agent, nearbyAnimals, 2.5);
        int animalCount = closeAnimals.Count;

        if (animalCount > 0)
        {

            foreach (Animal a in closeAnimals)
            {
                Vec3 force = Vec3.CalculateVectorsBetweenPoints(a.Position, agent.Position);
                avoidanceVector.Add(force);
            }

            avoidanceVector.Divide(animalCount);

            return avoidanceVector;
        }
        else
        {
            return Vec3.Zero();
        }
    }

    private List<Animal> GetNearbyAnimals(Animal agent, Dictionary<int, Animal> animals, double squareRadius)
    {
        List<Animal> nearbyAnimals = new List<Animal>();
        foreach (Animal a in animals.Values)
        {
            if (a.Id != agent.Id & agent.SquareDistanceTo(a) <= squareRadius)
            {
                nearbyAnimals.Add(a);
            }
        }

        return nearbyAnimals;
    }
}
