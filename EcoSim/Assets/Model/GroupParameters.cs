using System.Collections;
using System.Collections.Generic;

public class GroupParameters
{ 

    private double _reproductionProb;
    public double ReproductionProb { get => _reproductionProb; }

    private double _maxSpeed;
    public double MaxSpeed { get => _maxSpeed; }

    private int _groupSize;
    public int GroupSize { get => _groupSize; }

    public GroupParameters(double repProb, double maxSpeed, int size)
    {
        this._reproductionProb = repProb;
        this._maxSpeed = maxSpeed;
        this._groupSize = size;
    }
}
