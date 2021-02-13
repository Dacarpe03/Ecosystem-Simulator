using System.Collections;
using System.Collections.Generic;

public class CenterRule : BoidRule
{
    public CenterRule(double w) : base(w) { }

    //Go towards the safe zone
    public override Vec3 CalculateForce(Animal agent, List<Animal> animals)
    {
        Vec3 goToCenter = new Vec3(this._weight, 0, 0);
        return goToCenter;
    }
}
