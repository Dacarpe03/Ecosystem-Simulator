using System.Collections;
using System.Collections.Generic;

public class GroupParameters
{ 

    private double _reproductionProb;
    public double ReproductionProb { get => _reproductionProb; }

    private double _maxSpeed;
    public double MaxSpeed { get => _maxSpeed; }

    private double _visionRadius;
    public double VisionRadius { get => _visionRadius; }

    private int _groupSize;
    public int GroupSize { get => _groupSize; }

    public GroupParameters(double repProb, double maxSpeed, double visionRadius, int size)
    {
        this._reproductionProb = repProb;
        this._maxSpeed = maxSpeed;
        this._visionRadius = visionRadius;
        this._groupSize = size;
    }

    public string toString()
    {
        string serialized = "|" + this._reproductionProb
                          + "|" + this._maxSpeed
                          + "|" + this._visionRadius;
        return serialized;
    }
}
