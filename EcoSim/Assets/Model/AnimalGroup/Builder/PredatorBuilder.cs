using System.Collections;
using System.Collections.Generic;
using System;

public class PredatorBuilder : AnimalBuilder
{
    private double X_UPPER = 20;
    private double X_LOWER = 0;

    private double Y_UPPER = 0;
    private double Y_LOWER = 0;

    private double Z_UPPER = 50;
    private double Z_LOWER = 0;

    public PredatorBuilder(GroupParameters parameters) : base(parameters) {
    }


    //Method to create a prey with Flee State as initial state
    public override Animal CreateAnimal(Random rand, AnimalMediator mediator)
    {
        AnimalState initialState = this.GetAnimalState();
        Vec3 position = new Vec3(X_UPPER, X_LOWER, Y_UPPER, Y_LOWER, Z_UPPER, Z_LOWER, rand);
        Animal a = new Animal(initialState, this._animalParameters.MaxSpeed, this._animalParameters.VisionRadius, this._creationCounter, rand, mediator, position);
        this._creationCounter += 1;

        return a;
    }

    public override AnimalState GetAnimalState()
    {
        //return new AnimalHuntState(new SimpleStrategy());
        //return new AnimalHuntState(new GWOStrategy());
        return new AnimalHuntState(new PSOStrategy());
        //return new AnimalHuntState(new WOAStrategy());
    }
}
