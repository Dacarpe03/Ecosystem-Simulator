using System.Collections;
using System.Collections.Generic;
using System;

public class PredatorBuilder : AnimalBuilder
{
    private double X_UPPER = 100;
    private double X_LOWER = 0;

    private double Y_UPPER = 0;
    private double Y_LOWER = 0;

    private double Z_UPPER = 100;
    private double Z_LOWER = 0;

    private int iterations;
    private int candidates;
    private int strategy;

    public PredatorBuilder(GroupParameters parameters, int iterations, int candidates, int strategy) : base(parameters) {
        this.iterations = iterations;
        this.candidates = candidates;
        this.strategy = strategy;
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
        switch (strategy) {
            case 1:
                return new AnimalHuntState(new PSOStrategy(iterations, candidates));
            case 2:
                return new AnimalHuntState(new GWOStrategy(iterations, candidates));
            case 3:
                return new AnimalHuntState(new WOAStrategy(iterations, candidates));
            default:   
                return new AnimalHuntState(new SimpleStrategy());
        }
    }
}
