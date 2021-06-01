using System.Collections;
using System.Collections.Generic;

public class Boid
{
    private List<BoidRule> _friendlyRules;
    private List<BoidRule> _foeRules;

    private const double WEIGHT_AVOID = 7;
    private const double WEIGHT_COHESION = 2;
    private const double WEIGHT_FOLLOW = 3;
    private const double WEIGHT_CENTER = 10;
    private const double WEIGHT_FLEE = 20;
    private const double AVOIDANCE_RADIUS = 2.5;

    public Boid()
    {
        this._friendlyRules = new List<BoidRule>();
        this._foeRules = new List<BoidRule>();

        this._friendlyRules.Add(new AvoidanceRule(WEIGHT_AVOID, AVOIDANCE_RADIUS));
        this._friendlyRules.Add(new CohesionRule(WEIGHT_COHESION));
        this._friendlyRules.Add(new FollowRule(WEIGHT_FOLLOW));
        this._friendlyRules.Add(new CenterRule(WEIGHT_CENTER));
        this._foeRules.Add(new FleeRule(WEIGHT_FLEE));


    }

    public Vec3 CalculateForces(Animal agent, List<Animal> nearbyAnimals, List<Animal> predators)
    {
        Vec3 acceleration = Vec3.Zero();

        //Rules affected by preys
        foreach(BoidRule rule in this._friendlyRules)
        {
            acceleration.Add(rule.CalculateForce(agent, nearbyAnimals));
        }

        //Rules affected by predators
        foreach(BoidRule rule in this._foeRules)
        {
            acceleration.Add(rule.CalculateForce(agent, predators));
        }

        acceleration.Trim(agent.MaxSquaredSpeed);

        return acceleration;
    }
}
